using UnityEngine;
using System.Collections;
using Team;

public class ContinuousDamageController : MonoBehaviour {

    float damagePerSecond;
    int intervalsPerSecond;
    float timeBetweenAttacks;
    float currentTimer = 0;
    bool attack = false;
    float attackDistance;

    public bool isHero;

    Teams myTeam;

    DamagePackage myDamage;

    Vector3 centerOnMe = new Vector3(0, .5f, 0f);

    void Start() {
        damagePerSecond = isHero? DesignerTool.Instance.HeroDamagePerSecond : DesignerTool.Instance.MinionDamagePerSecond;
        attackDistance = isHero ? 3.9f : 1.7f;
        intervalsPerSecond = 10;
        timeBetweenAttacks = 1f / intervalsPerSecond;
        myTeam = GetComponent<GenericMoveController>().GetTeam();
        myDamage = new DamagePackage(myTeam, gameObject);

        int damagePerHit = (int)(damagePerSecond / intervalsPerSecond);
        myDamage.AddDamage(new Damage(damagePerHit, DamageTypes.normal, false));
    }

    void Update() {
        currentTimer += Time.deltaTime;
    }

    void FixedUpdate() {
        if (attack)
            LaunchDamageSpere();
        attack = false;
    }

    public void ProcessAttack(ActorState state) {
        if (!state.isAttacking || currentTimer < timeBetweenAttacks)
            return;
        else {
            attack = true;
            currentTimer = 0;
        }
    }

    void LaunchDamageSpere() {
        // Known bug - will not hit if minions are stacking
        RaycastHit hit;
        if(Physics.SphereCast(transform.position + centerOnMe, .15f, transform.forward, out hit, attackDistance)) {
            var damage = (Damageable)hit.transform.GetComponent(typeof(Damageable));
            if (damage != null) {
                damage.ReceiveDamage(myDamage);
            }
        }
    }
}
