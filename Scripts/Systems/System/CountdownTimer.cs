using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class CountdownTimer : MonoBehaviour {
    [SerializeField]
    int maxTimeInSec = 120;
    int currentTime;
    // Can add an event system so that it broadcasts the time every second for any listeners (static)
    public Text timerText;
    public bool freezeTime;

    [SerializeField]
    UnityEvent gameOverEvent;

    void Awake() {
        currentTime = maxTimeInSec;
        CalculateTime();
    }

    void Start() {
        InvokeRepeating("CalculateTime", 1, 1);
    }

    void CalculateTime() {
        if (freezeTime)
            return;
        if (currentTime > 0)
            currentTime -= 1;
        else {
            gameOverEvent.Invoke();
        }

        var seconds = currentTime % 60;
        string secondsString = (seconds < 10) ? ("0" + seconds).ToString() : seconds.ToString();
        int minutes = currentTime / 60;

        timerText.text = string.Format("{0}:{1}", minutes, secondsString);
        }
    }
