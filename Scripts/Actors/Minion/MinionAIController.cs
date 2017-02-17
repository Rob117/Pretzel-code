using UnityEngine;
using System.Collections;
using Team;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(ActorTeamRegister))]
public class MinionAIController : MonoBehaviour {
    
    // our controller, if we have one
    ControllableActor controllableActor;
    // Who we have in our sights
    Damageable currentTarget;
    
    // They cross the line, we end a life.
    SphereCollider aggroTrigger;

    [SerializeField]
    float attackRange = .5f; // minion arm length
    // How far we chase - design tool
    float aggroRange;
    //How fast we blaze it, design tool
    float speed;
    // Are we aggroed on this thing or just moving towards it b/c victory condition?
    bool tracking;

    bool isControllable;

    bool attackJustStarted = false;
    bool attackTimerFinished = true;
    float minAttackDuration;

    ActorTeamRegister myRegister;
    Transform myTrans;
    AnimationState animState;
    ContinuousDamageController damageState;
    Status status;

    Vector3 characterCenter = new Vector3(0, .5f, 0);

    bool attacking;

    void Awake() {
        aggroTrigger = GetComponent<SphereCollider>();
        controllableActor = GetComponent<ControllableActor>();
        animState = GetComponent<AnimationState>();
        damageState = GetComponent<ContinuousDamageController>();
        myRegister = GetComponent<ActorTeamRegister>();
        myTrans = transform;
        status = GetComponent<Status>();
        status.OnDamage += SwitchTargetOnDamage;
        status.OnHPZero += HPZeroCallback;
    }

    void Start() {
        aggroRange = DesignerTool.Instance.MinionAggroRange;
        speed = DesignerTool.Instance.MinionSpeed;
        SetupTrigger();
        isControllable = (controllableActor != null);
        minAttackDuration = DesignerTool.Instance.MinionMinimumAttackDuration;
    }

    void SetupTrigger() {
        aggroTrigger.radius = aggroRange - .2f; // We want this to be a bit smaller than aggro so we can pursue correctly
        aggroTrigger.isTrigger = true;
        aggroTrigger.center = characterCenter;
    }

    void Update() {
        // If we actually are controllable, and we are controlled, do nothing
        if (isControllable && !controllableActor.IsUncontrolled())
            return;
        // if we don't have a target, get one or shut down ai
        if (currentTarget == null) {
            AquireTarget();
        } else if (!ProcessCurrentTargetValidity(currentTarget)) { // If current target is not valid, set to null
            currentTarget = null;
        }
        var state = ProcessControls();
        animState.ProcessAnimation(state);
        damageState.ProcessAttack(state);
    }

    void AquireTarget() {
        if (!DamageHelperMethods.AquireDamageableTargetInArea(transform.position + characterCenter, aggroRange - .2f, ref currentTarget, myRegister.GetMyTeam())) {
            currentTarget = VictoryConditions.Instance.GetHighestPriorityDamageableVictoryObject(myRegister.GetMyTeam());
            if (currentTarget == null) {
                this.enabled = false;
            }
        }

    }

    bool ProcessCurrentTargetValidity(Damageable target) {
        var dist = Vector3.Distance(transform.position, target.transform.position);
        tracking = (dist < aggroRange + 5); // we add the five here because of animation issues
        return (tracking || target.IsCurrentlyVictoryObject()); // If we are moving on a victory object, no problem. Else, set to null somewere else
    }

    bool ProcessNewTargetValidity(GameObject obj) {
        var damageableTarget = obj.GetComponent<Damageable>();
        if (damageableTarget == null)
            return false; // Don't attack if it won't bleed. Waste of time.
        if ((damageableTarget.GetTeam() == myRegister.GetMyTeam()) || damageableTarget.isUndetectableToDamageAI())
            return false;
        return true;
    }

    void OnTriggerEnter(Collider col) {
        //Something has entered our sphere of funs
        if (tracking) // Busy. Come back later.
            return;
        if (ProcessNewTargetValidity(col.gameObject))
            currentTarget = col.gameObject.GetComponent<Damageable>();
    }

    ActorState ProcessControls() { // Use a state machine for this in the actual game
        if (currentTarget != null) {
            //Always look at the target
            var targetTrans = currentTarget.transform;
            myTrans.LookAt(targetTrans);
            //Attack the target if in range
            if (targetIsAttackable(currentTarget)) {
                if (!attackJustStarted) {
                    attackJustStarted = true;
                    StartCoroutine(waitForMinAttackTime());
                }
                return new ActorState(true, 0);
            }
            // We aren't on the min attack timer and aren't in range
            attackJustStarted = false;
            myTrans.position = Vector3.MoveTowards(myTrans.position, targetTrans.position, speed * Time.deltaTime * 7);
            return new ActorState(false, 1);
        }
        return new ActorState(false, 0);
    }

    bool targetIsAttackable(Damageable obj) {
        // Vector3.Distance(myTrans.position, obj.transform.position) < attackRange - .2f
        RaycastHit hit;
        if (obj != null && !attackTimerFinished)
            return true;
        if (Physics.Raycast(transform.position + characterCenter, transform.TransformDirection(Vector3.forward), out hit, attackRange - .2f)) {
            var damageableObject = hit.collider.gameObject.GetComponent<Damageable>();
            if (damageableObject == currentTarget) {
                return true;
            }
        }
        return false;
    }

    IEnumerator waitForMinAttackTime() {
        attackTimerFinished = false;
        yield return new WaitForSeconds(minAttackDuration);
        attackTimerFinished = true;
    }

   void SwitchTargetOnDamage(GameObject obj) {
        // Exit if this is the same object as out current target
        if (currentTarget != null && obj == currentTarget.gameObject)
            return;

        var target = obj.GetComponent<Damageable>();
        //Exit if this isn't damageable, we are forced to be attacking something, we have a target in attack range, or this is undetectable
        if (target == null || !attackTimerFinished || targetIsAttackable(currentTarget) || target.isUndetectableToDamageAI() || target.GetTeam() == myRegister.GetMyTeam())
            return;
        currentTarget = target;
    }

   void HPZeroCallback(GameObject obj) {
        this.enabled = false;
    }

   void OnDestroy() {
        status.OnDamage -= SwitchTargetOnDamage;
        status.OnHPZero -= HPZeroCallback;
    }
}
