using UnityEngine;
using System.Collections;

public class CannonBallDamage : MonoBehaviour {

    DamagePackage damageOnImpact;
    [SerializeField] GameObject model;
    [SerializeField] GameObject hitEffect;

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag != "Tower")
            ProcessSuccessfulImpact(col);
    }

    void ProcessSuccessfulImpact(Collision col) {
        var target = col.gameObject.GetComponent<Damageable>();
        if (target != null)
            target.ReceiveDamage(damageOnImpact);
        DestroyThis();
    }

    public void SetImpactDamage(DamagePackage damage) {
        damageOnImpact = damage;
    }

    void DestroyThis() {
        Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
