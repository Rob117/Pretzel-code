using UnityEngine;
using System.Collections;
using System.Linq;
using Team;
using System.Collections.Generic;

public class ControllableActorManager : Singleton<ControllableActorManager> {

    List<ControllableActor> controllableSceneActors = new List<ControllableActor>();

    public void AddActorToScene(ControllableActor actor) {
        if (actor != null && !controllableSceneActors.Contains(actor))
            controllableSceneActors.Add(actor);
        if (OnActorRegister != null)
            OnActorRegister(actor);
    }

    public void RemoveActorFromScene(ControllableActor actor) {
        if (actor != null && controllableSceneActors.Contains(actor))
            controllableSceneActors.Remove(actor);
    }

    public LinkedList<ControllableActor> GetAlliedUncontrolledActors(Teams alliedTeam, ControllableActor current) {
        List<ControllableActor> eligibleActors = new List<ControllableActor>();
        foreach (var actor in controllableSceneActors) {
            var validActor = actor != null && actor.GetTeam() == alliedTeam;
            var currentlyControllable = actor.IsUncontrolled() || actor == current;
            if (validActor && currentlyControllable)
                eligibleActors.Add(actor);
        }
        eligibleActors.OrderBy(a => a.isHighPriorityActor);
        return new LinkedList<ControllableActor>(eligibleActors);
    }

    public List<ControllableActor> GetAllActors() {
        return controllableSceneActors;
    }

    public delegate void registerActor(ControllableActor actor);
    public event registerActor OnActorRegister;
}
