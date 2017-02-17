using Rewired;
using System.Linq;
using UnityEngine;

namespace Team {

    public class TeamMember {
        public Player player;
        ControllableActor controlledActor;
        ActionPackage[] currentActorActions;
        Teams myTeam;

        // Initialize the team member with a team, then link them to inputs, then try to get an actor
        public TeamMember(Player player, Teams team) {
            this.player = player;
            controlledActor = null;
            myTeam = team;
            RegisterPlayerInput();
            ChangeControlledActor(SearchForNewActor());
        }

        bool changing = false;
        void ChangeControlledActor(ControllableActor newActor) {
            changing = true;
            if (newActor != null && newActor.IsUncontrolled()) {
                newActor.SetToControlled(player.controllers.Joysticks);
                ReleaseControlledActor();
                controlledActor = newActor;
                currentActorActions = controlledActor.GetAllAvailableActions();
                LinkInputToControlledActor(currentActorActions);
                controlledActor.OnRemoveFromScene += ReleaseControlledActor;
            }
            changing = false;
        }

        public void ReleaseControllOfActorOnDisconnect() {
            ReleaseControlledActor();
        }

        void ReleaseControlledActor() {
            if (controlledActor != null) {
                Debug.Log("Releasing control over the actor");
                controlledActor.OnRemoveFromScene -= ReleaseControlledActor;
                controlledActor.SetToUncontrolled();
                DelinkInputToControlledActor(currentActorActions);
                controlledActor = null;
            }
            if (!changing) {
                // This will still be called when you switch teams
                ChangeControlledActor(SearchForNewActor());
            }
        }

        /// <summary>
        /// Switch to event-driven input polling model
        /// </summary>
        public void RegisterPlayerInput() {
            player.AddInputEventDelegate(ChangeActorAction, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, RewiredActions.change_actor);
            player.AddInputEventDelegate(ChangeActorAction, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, RewiredActions.change_actor_minus);
            player.AddInputEventDelegate(ChangeTeam, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, RewiredActions.change_team);
            player.AddInputEventDelegate(TryToChangePlayer, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, RewiredActions.change_to_player_character);
        }

        void ChangeActorAction(InputActionEventData data) {
            bool isReverse = data.actionName == RewiredActions.change_actor_minus;
            ChangeControlledActor(SearchForNewActor(isReverse));
        }

        void TryToChangePlayer(InputActionEventData data) {
            XDebug.Log("Trying to change");
            if (controlledActor == null) return;
            var firstEligible = ControllableActorManager.Instance.GetAlliedUncontrolledActors(myTeam, controlledActor)
                .First(actor => actor.GetComponent<Status>().isHero);
            if (firstEligible && firstEligible != controlledActor)
                ChangeControlledActor(firstEligible);
        }

        public bool TryEquipNewActor(ControllableActor actor) {
            if (!HasActor())
                ChangeControlledActor(actor);
            return HasActor();
            
        }

        bool HasActor() {
            return controlledActor != null;
        }

        ControllableActor SearchForNewActor(bool reverseCycle = false) {
            var eligibleActors = ControllableActorManager.Instance.GetAlliedUncontrolledActors(myTeam, controlledActor);
            // XDebug.Log("count: {0}", eligibleActors.Count);
            if (controlledActor != null) {

                if (eligibleActors.Count > 1) { // If we aren't null and there is at least one other eligible actor
                    var myNode = eligibleActors.Find(controlledActor); // Get our actor's node so we can move to the next/previous actor

                if (!reverseCycle)
                    return myNode.Next != null ? myNode.Next.Value : eligibleActors.First.Value;
                else
                    return myNode.Previous != null ? myNode.Previous.Value : eligibleActors.Last.Value;
                }

                return null; // Otherwise, there isn't anything but this actor anyway, and return;
            }

            else { // If our controlled actor is null
                if (eligibleActors.Count > 0) { // grab the first actor in the scene that is available
                    return eligibleActors.First.Value;
                }
                return null; // or return nothing
            }
        }

        void ChangeTeam(InputActionEventData data) {
            ++myTeam;
            if (myTeam >= Teams.neutral)
                myTeam = 0;
            ReleaseControlledActor();
        }

        public Teams GetTeam() {
            return myTeam;
        }

        void LinkInputToControlledActor(ActionPackage[] package) {
            foreach (var action in package) {
                if (string.IsNullOrEmpty(action.inputActionName))
                    player.AddInputEventDelegate(action.onActiveInput, action.typeOfLoop);
                else
                    player.AddInputEventDelegate(action.onActiveInput, action.typeOfLoop, action.typeOfEvent, action.inputActionName);
            }
        }

        void DelinkInputToControlledActor(ActionPackage[] package) {
            if (package != null) {
                foreach (var action in package) {
                    player.RemoveInputEventDelegate(action.onActiveInput);
                }
                currentActorActions = null;
            }
        }
    }
}