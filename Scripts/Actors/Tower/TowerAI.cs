using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TowerAI : MonoBehaviour {
    Damageable currentTarget;
    float firingSpeed;
    int detectRange = 10;
    [SerializeField] ActorTeamRegister myTeam;
    [SerializeField] CannonBallSpawner myCannon;
    [SerializeField] Transform towerBase;
    [SerializeField] Transform cannonLayer;
    [SerializeField] float minimumFiringAngle = 10f;

    int distanceToCenterAdjustment = 3;

    bool active = false;

    void Start() {
        firingSpeed = DesignerTool.Instance.WaitTimeBetweenShotsInSeconds;
        StartCoroutine(AttemptToFire());
        detectRange = DesignerTool.Instance.TowerRange;
        GetComponent<SphereCollider>().radius = detectRange;
        detectRange = detectRange + distanceToCenterAdjustment; // Hack to get the detectors working correctly. Should be different variable.
    }

    void AlignCannons() {
        if (currentTarget == null)
            return; 
        towerBase.LookAt(currentTarget.transform);
        myCannon.transform.LookAt(currentTarget.transform);
    }

    bool CannonsAreAligned() {
        var targetPos = currentTarget.transform.position;
        var myPos = cannonLayer.position;

        var directionToTarget = new Vector3(targetPos.x, 0, targetPos.z) - new Vector3(myPos.x, 0, myPos.z);
        return (Vector3.Angle(directionToTarget, cannonLayer.forward) < minimumFiringAngle);
    }

    bool DistanceToTargetIsValid(Transform target) {
        return Mathf.Abs(Vector3.SqrMagnitude(target.position - transform.position)) < detectRange * detectRange;
    }

    bool ProcessTarget(GameObject obj) {
        var damageableObject = obj.GetComponent<Damageable>();
        return (damageableObject != null && damageableObject.GetTeam() != myTeam.GetMyTeam() && !damageableObject.isUndetectableToDamageAI() && DistanceToTargetIsValid(obj.transform));
    }

    void OnTriggerEnter(Collider col) {
        if (currentTarget == null && ProcessTarget(col.gameObject)) {
            currentTarget = col.gameObject.GetComponent<Damageable>();
            active = true;
        }
    }

    void Fire() {
        myCannon.Fire();
    }

    void Update() {
        if (active) {
            if (currentTarget != null && DistanceToTargetIsValid(currentTarget.transform)) {
                AlignCannons();
            } else if (!DamageHelperMethods.AquireDamageableTargetInArea(transform.position, detectRange, ref currentTarget, myTeam.GetMyTeam())) {
                currentTarget = null;
                active = false;
            }
        }
    }

    IEnumerator AttemptToFire() {
        while (true) {
            yield return new WaitForSeconds(firingSpeed);
            //ERROR - Cannons are aligned is not calculated correctly for the left cannon
            if (currentTarget != null && CannonsAreAligned())
                Fire();
        }
    }

    void OnDestroy() {
        StopAllCoroutines();
    }
}
