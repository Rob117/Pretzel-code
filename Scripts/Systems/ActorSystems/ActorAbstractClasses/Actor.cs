using UnityEngine;
using System.Collections;
using Team;

/// <summary>
/// Anything in the scene that has agency
/// </summary>
[RequireComponent(typeof(ActorTeamRegister))]
public abstract class Actor : MonoBehaviour {
    protected ActorTeamRegister myRegister;

    protected virtual void Awake() {
        myRegister = GetComponent<ActorTeamRegister>();
    }

    public virtual void IntializeActor(Teams team) {
        myRegister.SetMyTeam(team);
    }

    public Teams GetTeam() {
        return myRegister.GetMyTeam();
    }
}
