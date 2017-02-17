using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MainMenuController : MonoBehaviour {

    [SerializeField]
    GameObject selectedOnOpen;


    void OnEnable() {
        VictoryConditions.canCallEndOfGame = true;
    }
    public void StartGame() {
        SceneManager.LoadScene("Main");
    }

    public void ExitGame() {
        Application.Quit();
    }
}
