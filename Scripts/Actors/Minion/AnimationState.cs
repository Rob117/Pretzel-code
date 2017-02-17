using UnityEngine;

public class AnimationState : MonoBehaviour {
    Animator anim;
    Status status;
    bool overrideAllInput;

    void Awake() {
        anim = GetComponent<Animator>();
        status = GetComponent<Status>();
        status.OnHPZero += OnZero;
        if (anim == null) this.enabled = false;
    }

    public void ProcessAnimation(ActorState state) {
        if (overrideAllInput)
            return;
        anim.SetBool(AnimTags.attackingHash, state.isAttacking);
        if (state.isAttacking) {
            anim.SetFloat(AnimTags.speedHash, 0);
        } else {
            anim.SetFloat(AnimTags.speedHash, state.speed);
        }
    }

    void OnZero(GameObject dmg) {
        overrideAllInput = true;
        anim.SetTrigger(AnimTags.deadHash);
    }

    void OnDestroy() {
        status.OnHPZero -= OnZero;
    }
}
