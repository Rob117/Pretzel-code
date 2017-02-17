using UnityEngine;
using System.Collections;

public class MatchRotationOfObjectBelow : MonoBehaviour {

    float rotationSpeed;
    float delayBeforeStart;
    bool rotating;
    Transform myTransform;

    public Transform myBase;


    void Start() {
        rotationSpeed = DesignerTool.Instance.TowerCatchupSpeed;
        delayBeforeStart = DesignerTool.Instance.TowerRotationDelay;
        myTransform = transform;
    }

    void Update() {
        if (rotating)
            return;
        var myRotation = myTransform.localRotation;
        var baseRotation = myBase.localRotation;
        if (myRotation != baseRotation) {
            StartCoroutine(Rotate());
            rotating = true;
        }
    }
    IEnumerator Rotate() {
        yield return new WaitForSeconds(delayBeforeStart);
        while (myTransform.localRotation != myBase.localRotation) {
            myTransform.localRotation = Quaternion.RotateTowards(myTransform.localRotation, myBase.localRotation, rotationSpeed);
            yield return null;
        }
        rotating = false;
    }
}
