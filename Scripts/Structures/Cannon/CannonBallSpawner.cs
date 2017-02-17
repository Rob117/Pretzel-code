using UnityEngine;
using System.Collections;
using Team;
using UnityEngine.Events;

public class CannonBallSpawner : MonoBehaviour {
    [SerializeField] CannonBallDamage cannonBall;
    DamagePackage myDamage;
    [SerializeField] GameObject fireFX;
    int cannonImpactDamage;
    [SerializeField] UnityEvent fireEvent;


    void Start() {
        cannonImpactDamage = DesignerTool.Instance.CannonBallDamage;
        myDamage = new DamagePackage(GetComponentInParent<ActorTeamRegister>().GetMyTeam(), gameObject);
        myDamage.AddDamage(new Damage(cannonImpactDamage, DamageTypes.normal, false));
    }

    public void Fire() {
        Instantiate(fireFX, transform.position, transform.localRotation);
        var cb = Instantiate(cannonBall, transform.position, transform.rotation) as CannonBallDamage;
        cb.SetImpactDamage(myDamage);
        fireEvent.Invoke();
    }

}
