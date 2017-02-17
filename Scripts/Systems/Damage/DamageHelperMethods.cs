using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Team;

public class DamageHelperMethods : MonoBehaviour {
    
    public static bool AquireDamageableTargetInArea(Vector3 centerOfArea, float radiusOfTargetArea, ref Damageable currentTarget, Teams myTeam ) {
        Collider[] hitColliders = Physics.OverlapSphere(centerOfArea, radiusOfTargetArea);
        List<Damageable> damageableObjects = new List<Damageable>();
        foreach (var col in hitColliders) {
            var damageableComponent = col.GetComponent<Damageable>();
            if (damageableComponent != null)
                damageableObjects.Add(damageableComponent);
        }
        List<Damageable> eligibleTarget = new List<Damageable>();
        foreach (var target in damageableObjects) {
            var validTarget = target.GetComponent<Damageable>();
            if (validTarget != null && validTarget.GetTeam() != myTeam && !validTarget.isUndetectableToDamageAI())
                eligibleTarget.Add(validTarget);
        }
        if (eligibleTarget.Count > 0) {
            currentTarget = eligibleTarget.OrderByDescending(x => x.targetPriority).First();
            return currentTarget != null;
        }
        return false;
    }
}
