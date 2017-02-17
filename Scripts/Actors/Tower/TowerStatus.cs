using UnityEngine;
using System.Collections;

public class TowerStatus : Damageable {

    [SerializeField]
    GameObject deathPrefab;
    override protected void Start() {
        hp = DesignerTool.Instance.TowerHealth;
        targetPriority = DesignerTool.Instance.TowerTargetPriority;
        VictoryConditionPriority = DesignerTool.Instance.TowerTargetPriority;
        OnHPZero += processDeath;
        base.Start();
    }

    void processDeath(GameObject obj) {
        Instantiate(deathPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    protected override void OnDestroy() {
        OnHPZero -= processDeath;
        base.OnDestroy();
    }

    protected override void ProcessDamage(Damage dmg) {
		if (!isInvincible)
        	hp -= dmg.amount;
    }
}
