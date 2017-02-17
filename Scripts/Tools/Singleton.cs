using UnityEngine;

// Rob's note - lifted AS IS from the Unity Wiki - http://wiki.unity3d.com/index.php/Singleton

// This class will automatically call the class's built-in designer initializer, if one exists.
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    private static T _instance;

    private static object _lock = new object();

    public static T Instance {
        get {
            if (applicationIsQuitting) {
                return null;
            }

            lock (_lock) {
                if (_instance == null) {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1) {
                        Debug.LogError("[Singleton] Something went really wrong " +
                            " - there should never be more than 1 singleton!" +
                            " Reopening the scene might fix it.");
                        return _instance;
                    }

                    if (_instance == null) {
                        GameObject singleton = new GameObject();
                        //singleton.hideFlags = HideFlags.HideInHierarchy;
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) " + typeof(T).ToString();

                        DontDestroyOnLoad(singleton);

                       
                    } 
                }

                return _instance;
            }
        }
    }

    private static bool applicationIsQuitting = false;
    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    public void OnDestroy() {
        applicationIsQuitting = true;
    }
}