using UnityEngine;
using Team;
using UnityEngine.UI;

public class ActorTeamRegister : MonoBehaviour {
    [SerializeField]
    Teams myTeam;

	[SerializeField]  Text teamText;

    public void SetMyTeam(Teams team) {
        myTeam = team;
		if (teamText == null)
			return;
		switch (team) {
		case Teams.teamOne:
			teamText.text = "1";
			teamText.color = Color.blue;
			break;
		case Teams.teamTwo:
			teamText.text = "2";
			teamText.color = Color.red;
			break;
		default:
			break;
		}
    }

    public Teams GetMyTeam() {
        return myTeam;
    }
}
