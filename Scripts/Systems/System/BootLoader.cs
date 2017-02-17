using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BootLoader : MonoBehaviour {
    static string savedScene;
    public static bool loaded;

    void Start() {
        if (loaded)
            return;
        else if (SceneManager.GetActiveScene().name == "bootLoader") {
            loaded = true;
            if (!string.IsNullOrEmpty(savedScene)) {
                SceneManager.LoadScene(savedScene);
            } else
                SceneManager.LoadScene(1);
        } else {
            savedScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("bootLoader");
        }
    }
}
