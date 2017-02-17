using UnityEngine;
using UnityEngine.UI;
using Team;
using System.Collections;

public class SelectionRing : MonoBehaviour {
    [SerializeField]
    Image selectionRing;

    public void SetRingColor(Teams team) {
        switch (team) {
            case Teams.teamOne:
                selectionRing.color = Color.blue;
                break;
            case Teams.teamTwo:
                selectionRing.color = Color.red;
                break;
            case Teams.neutral:
                selectionRing.color = Color.white;
                break;
            default:
                break;
        }
    }
}
