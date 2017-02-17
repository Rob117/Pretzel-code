using UnityEngine;
using System.Collections;

public class AnimationBoolEvent : MonoBehaviour {

    [SerializeField]
    Animator myAnimator;

    [SerializeField]
    string myTrigger;

    public void ExecuteBool() {
        myAnimator.SetTrigger(myTrigger);
    }
}
