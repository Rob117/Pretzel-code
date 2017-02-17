using UnityEngine;
using Team;

public abstract class Damageable : VictoryObject {

    #region HP and Damage
    [SerializeField][Tooltip("It might be necessary to set this from the design tool")]
    protected int hp;
    private float maxhp; // float for HP bar calculations
    [SerializeField][Tooltip("This will be automatically set to true on 0 HP")]
    protected bool isInvincible = false;

    public delegate void onDamageEvent(GameObject obj);
    public event onDamageEvent OnHPZero;
    public event onDamageEvent OnDamage;
    #endregion

    #region AIHandling
    [SerializeField][Tooltip("AI will ignore this when picking specific targets. It is NOT invincible")]
    protected bool invisibleToAI = false;
    [Tooltip("Higher priority = damage this first")]
    public int targetPriority;
    #endregion

    #region UI
    [SerializeField]
    private HealthBar healthBarPrefab;
    #endregion

    protected virtual void Start() {
        RegisterAsVictoryObject();
        maxhp = hp;
    }

    public virtual void ReceiveDamage(DamagePackage dmgPackage) {
        if (dmgPackage == null)
            return;
        if (OnDamage != null)
            OnDamage(dmgPackage.sourceObject);
        if (!dmgPackage.canDealFriendlyFire && dmgPackage.sourceTeam == GetTeam())
            return;
        foreach (var damage in dmgPackage.allDamage) {
            ProcessDamage(damage);
        }
		if (healthBarPrefab != null)
			healthBarPrefab.SetFillAmount(hp/maxhp);
        if (hp <= 0) {
			if (OnHPZero != null && !isInvincible)
                OnHPZero(dmgPackage.sourceObject);
            isInvincible = true;
        }
    }

    protected override void RegisterAsVictoryObject() {
        if (isVictoryObject) {
            VictoryConditions.Instance.RegisterAsVictoryObject(this);
        }
    }

    public bool isUndetectableToDamageAI() {
        return invisibleToAI;
    }

    protected override void DeregisterAsVictoryObject() {
        VictoryConditions.Instance.DeregisterVictoryObject(this);
    }

    abstract protected void ProcessDamage(Damage dmg);

    protected virtual void OnDestroy() {
        DeregisterAsVictoryObject();
    }

}
