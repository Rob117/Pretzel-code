using UnityEngine;
using System.Collections;

// http://answers.unity3d.com/questions/161084/coroutine-without-monobehaviour.html
public class CoroutineManager : Singleton<CoroutineManager> {
    public void StartChildCoroutine(IEnumerator coroutineMethod) {
        StartCoroutine(coroutineMethod);
    }

    public void StartChildCoroutine(string name) {
        StartCoroutine(name);
    }
    public void StopChildCoroutine(IEnumerator coroutineMethod) {
        StopCoroutine(coroutineMethod);
    }
    public void StopChildCoroutine(string name) {
        StopCoroutine(name);
    }
}
