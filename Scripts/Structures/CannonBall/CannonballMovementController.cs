using UnityEngine;
using System.Collections;

public class CannonballMovementController : MonoBehaviour {
    float speed;

    void Start() {
        speed = DesignerTool.Instance.CannonBallSpeed;
    }

    void Update() {
        transform.Translate(new Vector3(0,0,1) * speed * Time.deltaTime);
    }
}
