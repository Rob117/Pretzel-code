using UnityEngine;
using Team;
using System.Collections;

[RequireComponent(typeof(ActorTeamRegister))]
public abstract class VictoryObject : MonoBehaviour {
    [SerializeField][Tooltip("Does this need to be dealt with to win?")]
    protected bool isVictoryObject = false;
    [Tooltip("Higher Priority = AI goes after this sooner")]
    public int VictoryConditionPriority = 0;

    ActorTeamRegister myRegister;

    protected abstract void RegisterAsVictoryObject();
    protected abstract void DeregisterAsVictoryObject();

    public bool IsCurrentlyVictoryObject() {
        return isVictoryObject;
    }

    /// <summary>
    /// Do not call this outside of the deregister field
    /// </summary>
    public void SetVictoryObjectStatusToFalseOnDeregister() {
        isVictoryObject = false;
    }

    void Start() {
        if (isVictoryObject && VictoryConditionPriority == 0) {
            throw new System.Exception("You have an object set to prioriy 0 but marked as necessary to win");
        }
    }

    public Teams GetTeam() {
        if (myRegister == null)
            myRegister = GetComponent<ActorTeamRegister>();
        return myRegister.GetMyTeam();
    }
}
