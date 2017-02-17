using UnityEngine;
using System.Collections.Generic;

public class AnimationEffectHandler : MonoBehaviour {
    Animator anim;
    GameObject currentFX;
    AnimationClip previousClip;
    public AnimationTool fxManager;
    bool hasManager = false;

    void Awake() {
        anim = GetComponent<Animator>();
        hasManager = (fxManager != null);
    }

    void PlayOnce(GameObject fx) {
        Instantiate(fx, gameObject.transform.position, gameObject.transform.rotation);
    }

    void Update() {
        if (!hasManager)
            return;
        var CurrentClipInfo = anim.GetCurrentAnimatorClipInfo(0);
        if (CurrentClipInfo.Length < 1)
            return;
        var clip = CurrentClipInfo[0].clip;
        if (previousClip == null || previousClip != clip) {
            ClearCurrentFX();
            previousClip = clip;
            // get an FX package from the FX manager if it exists
            var result = fxManager.ClipFXPairs.Find(x => x.clip == clip);
            if (result != null) {
                currentFX = Instantiate(result.fx);
                currentFX.transform.parent = transform;
                currentFX.transform.localPosition = Vector3.zero;
                currentFX.transform.localRotation = Quaternion.identity;
            }
        }
    }

    void ClearCurrentFX() {
        if (currentFX != null) {
            Destroy(currentFX);
        }
    }
}
