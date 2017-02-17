using Team;
using Rewired;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GenericMoveController : ControllableActor {
    float speed;
    const float speedMultiplier = 10;
    float maxTurnSpeed;

    float minAttackTime;
    float myAttackTimer = 10;

    float horizontalSpeed;
    float verticalSpeed;
    bool firing;
    Rigidbody rb;
    AnimationState myAnim;
    ContinuousDamageController myDamage;
    Status status;

    public bool isHero;

    protected override void Awake() {
        rb = gameObject.GetComponent<Rigidbody>();
        myAnim = GetComponent<AnimationState>();
        myDamage = GetComponent<ContinuousDamageController>();
        status = GetComponent<Status>();
        status.OnHPZero += HPZero;
        base.Awake();
    }


    void Start() {
        speed = isHero ? DesignerTool.Instance.HeroSpeed : DesignerTool.Instance.MinionSpeed;
        maxTurnSpeed = isHero ? DesignerTool.Instance.HeroTurnSpeed : DesignerTool.Instance.MinionTurnSpeed;
        minAttackTime = isHero ? DesignerTool.Instance.HeroMinimumAttackDuration : DesignerTool.Instance.MinionMinimumAttackDuration;
        base.IntializeActor(myRegister.GetMyTeam()); // handle all the scene registration, etc
    }

    bool dying;
    void HPZero(GameObject obj) {
        dying = true;
    }
    void Update() {
        rb.velocity = Vector3.zero;
        if (!isControlled || dying){
            return;
        }
        myAttackTimer += Time.deltaTime;
        // This is a naive calculation that will allow for faster-than-possible diagonal movement, but for now it will suffice
        Vector3 moveVector = new Vector3(horizontalSpeed, 0, verticalSpeed);
        float currentSpeed = Mathf.Abs(Mathf.Abs(horizontalSpeed) + Mathf.Abs(verticalSpeed)) / 2;
        if (!firing && myAttackTimer < minAttackTime) // If we are not firing, but we haven't attacked for long enough, keep attacking
            firing = true;
        ActorState state = new ActorState(firing, currentSpeed);
        ProcessMovement(state, moveVector);
        if (myAnim != null) myAnim.ProcessAnimation(state);
        if (myDamage != null) myDamage.ProcessAttack(state);
        if (firing)
            base.VibrateSticks(.5f, .1f, false);
    }

    void ProcessMovement(ActorState state, Vector3 moveVector) {
        ProcessRotation(moveVector);
        if (state.isAttacking) {
            return;
        }
        rb.MovePosition(transform.position + moveVector * speed * speedMultiplier * Time.deltaTime); 
    }

    void ProcessRotation(Vector3 move) {
        if (move.sqrMagnitude != 0) // if we aren't moving, don't process or he will snap north constantly
            transform.rotation = Quaternion.LookRotation(transform.position+move * maxTurnSpeed);
    }

    void OnFire(InputActionEventData data) {
        bool attacking = data.GetButton();
        if (myAttackTimer > minAttackTime && data.GetButtonDown())
            myAttackTimer = 0; // If we just started doing the attack AND the timer should have lapsed, set the minimum timer over again
        firing = attacking;
    }

    void AxisUpdates(InputActionEventData data) {
        // if we don't have at least .15 movement && we aren't snapping back to zero from active movement
        bool isDeadZone = Mathf.Abs(data.GetAxis()) < .15f && data.GetAxisTimeInactive() < 0.01;
        if (isDeadZone) {
            return;
        }
        switch (data.actionName) { // store these variables to process movement in our update
            case RewiredActions.horizontal:
                horizontalSpeed = data.GetAxis();
                break;
            case RewiredActions.vertical:
                verticalSpeed = data.GetAxis();
                break;
        }
    }

    public override void SetToUncontrolled() {
        base.SetToUncontrolled();
        myAnim.ProcessAnimation(new ActorState(false, 0));
    }

    public override ActionPackage[] GetAllAvailableActions() {
        return new ActionPackage[] { // Fire is just a button press, but I want all the data for the axis so we write it all here
            new ActionPackage(OnFire, UpdateLoopType.Update, InputActionEventType.Update, RewiredActions.fire),
            new ActionPackage(AxisUpdates, UpdateLoopType.Update, InputActionEventType.AxisActive, RewiredActions.horizontal),
            new ActionPackage(AxisUpdates, UpdateLoopType.Update, InputActionEventType.AxisActive, RewiredActions.vertical),
            new ActionPackage(AxisUpdates, UpdateLoopType.Update, InputActionEventType.AxisInactive, RewiredActions.horizontal),
            new ActionPackage(AxisUpdates, UpdateLoopType.Update, InputActionEventType.AxisInactive, RewiredActions.vertical),
        };
    }

    void OnDestroy() {
        status.OnHPZero -= HPZero;
    }

}
