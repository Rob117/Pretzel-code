using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rewired;


namespace Team {
    public class TeamManager : Singleton<TeamManager> {
        public const int MAX_PLAYERS = 8;

        List<TeamMember> playerMembers = new List<TeamMember>();
        List<ControllableActor> newRegisteredActors = new List<ControllableActor>();

        public void AddMember(Player player) {
            // If the list is not empty and the player is already in it, do not double add
            if (playerMembers.Count > 0 && playerMembers.Any(m => m.player == player)) return;

            if (playerMembers.Count < MAX_PLAYERS)
                playerMembers.Add(new TeamMember(player, GetSmallerTeam()));
        }

        IEnumerator RemoveMember(Player player) {
            yield return 0;
            var PlayerToBeRemoved = playerMembers.First(m => m.player == player);
            if (PlayerToBeRemoved != null) {
                PlayerToBeRemoved.ReleaseControllOfActorOnDisconnect();
                playerMembers.Remove(PlayerToBeRemoved);
            }
        }

        public Teams GetSmallerTeam() {
            // If we have no players, Player one is assigned first.
            if (playerMembers.Count == 0)
                return Teams.teamOne;
            //This will let us return teamOne for edge cases
            var currentSmallest = new KeyValuePair<Teams, int> (Teams.teamOne, 100000);
            foreach (Teams team in Teams.GetValues(typeof(Teams))) {
                if (team == Teams.neutral)
                    continue; // we don't want to add a player to neutral. EVER.
                int count = 0;
                foreach (var member in playerMembers) {
                    if (member.GetTeam() == team)
                        ++count;
                }
                if (currentSmallest.Value > count)
                    currentSmallest = new KeyValuePair<Teams, int>(team, count);
            }
            return currentSmallest.Key;
        }

        public void AddPlayerOnDeviceConnect(ControllerStatusChangedEventArgs args) {
            AddMember(GetPlayerFromControllerID(args.controllerId));
        }

        public void RemovePlayerOnDeviceDisconnect(ControllerStatusChangedEventArgs args) {
            RemoveMember(GetPlayerFromControllerID(args.controllerId));
        }

        void Start() {
            ReInput.ControllerConnectedEvent += AddPlayerOnDeviceConnect;
            ReInput.ControllerDisconnectedEvent += RemovePlayerOnDeviceDisconnect;
            
            // the controller id starts at 0, so we just mirror it 1/1 with players
            foreach (var connectedController in ReInput.controllers.Controllers) {
                var correspondingPlayer = GetPlayerFromControllerID(connectedController.id);
                AddMember(correspondingPlayer);
            }
            ControllableActorManager.Instance.OnActorRegister += ActorRegistered; // this should never be unregistered, ever.
        }

        void Update() {
            if (newRegisteredActors.Count > 0) {
                foreach (var actor in newRegisteredActors) {
                    foreach (var player in playerMembers) {
                        if (player.GetTeam() == actor.GetTeam())
                            if (player.TryEquipNewActor(actor))
                                break;
                    }
                }
            }
        }

        void ActorRegistered(ControllableActor actor) {
            newRegisteredActors.Add(actor);
        }

        Player GetPlayerFromControllerID(int id) {
            return ReInput.players.Players[id];
        }
    }

    /// <summary>
    /// A list of teams, with the last team being unaffiliated
    /// </summary>
    public enum Teams { teamOne, teamTwo, neutral }

}