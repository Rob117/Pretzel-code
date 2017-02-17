using UnityEngine;
using System.Collections;
using Rewired;

public class RewiredTest : MonoBehaviour {
    // Get all players
    // Get all active players
    // Test to see what happens when we connect controllers
    // Test to see what happens when we disconnect controllers
    // Test to get input from a specific controller on an event
    // Get the controller to vibrate in different amounts
    public bool playerOneUsesKeyboard = true; // Do manual joystick assignment later!

    private Player player;
    void Start() {
        GetPlayerCount();
        GetAllActivePlayers();
        GetControllerCount();
        player = ReInput.players.Players[0]; // Assign the first player
        player.AddInputEventDelegate(SimulateEventPress, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Test");
        player.AddInputEventDelegate(SimulateEventPress2, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Vibrate");
        // If there is a keyboard, player one uses the keyboard.
        // If there is a controller, player 2 + can use the controller - AFTER

        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnConrollerDisconnected;

    }

    void GetPlayerCount() {
        XDebug.Log("Registered Players : <{0}>", ReInput.players.playerCount);
    }

    void GetControllerCount() {
        XDebug.Log("Keyboards: <{0}>, Controllers: <{1}>", ReInput.controllers.GetControllerCount(ControllerType.Keyboard),
            ReInput.controllers.GetControllerCount(ControllerType.Joystick));
    }

    void GetAllActivePlayers() {
        int activePlayers = 0;
        foreach (var player in ReInput.players.Players) {
            if (player.isPlaying) activePlayers++;
        }
        XDebug.Log("Active Players: <{0}>", activePlayers);
    }

    void SimulateEventPress(InputActionEventData data) {
        Debug.Log("SimulateEventPress was Triggered");
    }

    void SimulateEventPress2(InputActionEventData data) {
        Debug.Log("VIBRATE TIME!");
        foreach (var j in player.controllers.Joysticks) {
            if (j.supportsVibration && !(j.GetVibration(0) > 0)) {
                StartCoroutine(Vibrate(j));
                break;
            }
        }
    }

    void OnControllerConnected(ControllerStatusChangedEventArgs args) {
        XDebug.Log("Controller Connected <{0}>", args.name);
        GetPlayerCount();
        GetAllActivePlayers();
        GetControllerCount();
    }

    void OnConrollerDisconnected(ControllerStatusChangedEventArgs args) {
        XDebug.Log("Controller Disconnected <{0}>", args.name);
        GetPlayerCount();
        GetAllActivePlayers();
        GetControllerCount();
    }

    IEnumerator Vibrate(Joystick joy) {
        if (joy != null && joy.supportsVibration) {
            joy.SetVibration(1, 1);
            yield return new WaitForSeconds(.3f);
            joy.SetVibration(.5f, .5f);
            yield return new WaitForSeconds(.5f);
            joy.StopVibration();
        }
    }
}
