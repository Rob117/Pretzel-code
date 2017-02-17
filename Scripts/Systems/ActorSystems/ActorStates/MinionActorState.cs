using UnityEngine;

public class ActorState {
    public bool isAttacking;
    public Vector3 movement;
    public float speed;

    public ActorState(bool isAttacking, float speed) {
        this.isAttacking = isAttacking;
        this.speed = speed;
    }
}
