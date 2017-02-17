using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSceneController : MonoBehaviour {

    [SerializeField]
    Button resetButton;

	bool gameEnded; // when objects are destroyed on scene load, they call endgame by mistake

	void OnEnable(){
		
	}

    public void Draw () {
        EndGame("Draw");
    }

    public void TeamOneWin() {
        EndGame("P1 Win!");
    }

    public void TeamTwoWin() {
        EndGame("P2 Win");
    }

    void EndGame(string displayTextForButton) {
		if (gameEnded)
			return;
		gameEnded = true;
        Time.timeScale = 0.1f;
        if (resetButton != null) {
            resetButton.gameObject.SetActive(true);
            resetButton.GetComponentInChildren<Text>().text = displayTextForButton;
        }
    }

    public void ReturnToMainMenu() {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}
