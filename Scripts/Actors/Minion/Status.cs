using UnityEngine;
using UnityEngine.Events;

public class Status : Damageable {
    public bool isHero;
	[SerializeField] UnityEvent onHitReceivedEvent;

    override protected void Start() {
        hp = isHero ? DesignerTool.Instance.HeroHealth : DesignerTool.Instance.MinionHealth;
        targetPriority = isHero ? DesignerTool.Instance.HeroTargetPriority : DesignerTool.Instance.MinionTargetPriority;
        base.Start();
    }

    protected override void ProcessDamage(Damage dmg) {
		if (!isInvincible)
        	hp -= dmg.amount;
		if (onHitReceivedEvent != null)
			onHitReceivedEvent.Invoke();
    }

    void DeathCallback(GameObject obj) {
        Destroy(gameObject);
    }
    
    protected override void OnDestroy() {
        base.OnDestroy();
    }

}
