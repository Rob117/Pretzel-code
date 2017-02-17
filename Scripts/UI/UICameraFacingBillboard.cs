using UnityEngine;
using System.Collections;

// Source: http://wiki.unity3d.com/index.php?title=CameraFacingBillboard
namespace Pretzel.UI {
    public class UICameraFacingBillboard : MonoBehaviour {
        Camera cam;

        void Start() {
            cam = Camera.main;
        }

        void Update() {
            transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
                cam.transform.rotation * Vector3.up);
        }
    }
}