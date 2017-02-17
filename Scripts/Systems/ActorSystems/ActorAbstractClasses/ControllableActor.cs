using UnityEngine;
using System.Collections;
using Team;
using System;
using Rewired;
using System.Collections.Generic;

public abstract class ControllableActor : Actor {
    [SerializeField]
    SelectionRing myRing;

    protected bool isControlled = false;

    public bool isHighPriorityActor = false;
    
    [SerializeField] [Tooltip("This decides if this prefab can be controlled or not.")]
    protected bool isControllable = true;

    public delegate void RemoveAction();
    [HideInInspector] // This is called when the actor is destroyed. An object pool should call this.
    public event RemoveAction OnRemoveFromScene;
    
    // The only way to vibrate joysticks is to capture them from the player when we are controlled
    protected List<JoystickVibrator> vibratableJoysticks = new List<JoystickVibrator>();

    public override void IntializeActor(Teams team) {
        base.IntializeActor(team);
        if (isControllable)
            AddToControllableSceneActors();
        myRing.SetRingColor(team);
    }


    // Called by a player that has successfullly added us as their controlling member
    public void SetToControlled(IList<Joystick> joysticks) {
        isControlled = true;
        myRing.gameObject.SetActive(true);
        if (joysticks != null) {
            foreach (var joy in joysticks) {
                if (joy.supportsVibration)
                    vibratableJoysticks.Add(new JoystickVibrator(joy));
            }
        }
    }

    // Called by the player removing us from their control
    public virtual void SetToUncontrolled() {
        isControlled = false;
        myRing.gameObject.SetActive(false);
        vibratableJoysticks = null;
        vibratableJoysticks = new List<JoystickVibrator>();
    }

    // Used to determine if we have a controller or not, and thus, if we are assignable when looking for new actors
    public bool IsUncontrolled() {
        return !isControlled;
    }

    // Only called if we have been set to controllable before being added to the scene. 
    void AddToControllableSceneActors() {
        ControllableActorManager.Instance.AddActorToScene(this);
    }

    void OnDestroy() {
        RemoveFromControllableSceneActors();
    }

    // Called in OnDestroy; also polls our OnRemove event
    void RemoveFromControllableSceneActors() {
        ControllableActorManager.Instance.RemoveActorFromScene(this);
        if (OnRemoveFromScene != null)
            OnRemoveFromScene();
    }

    // This method is called by the controlling team member when they initialize us so that they can pass us their input
    // Only passes the input that the class specifically wants
    public abstract ActionPackage[] GetAllAvailableActions();

    /// <summary>
    /// If we fade off, the amount graded down gradually over the time of the effect
    /// </summary>
    public void VibrateSticks(float amount, float duration, bool fadeOff = false) {
        foreach (var stick in vibratableJoysticks) {
            // Pass in a value for it to vibrate with
            stick.Vibrate(amount, duration, fadeOff);
        }
    }
}

public struct ActionPackage {
    public Action<InputActionEventData> onActiveInput;
    public UpdateLoopType typeOfLoop;
    public InputActionEventType typeOfEvent;
    public string inputActionName;
    // All inputs require all four of these variables. This is intentional, and should not be overridden.
    public ActionPackage(Action<InputActionEventData> actionToCall, UpdateLoopType loop, InputActionEventType eventType, string nameForAction) {
        onActiveInput = actionToCall;
        typeOfLoop = loop;
        typeOfEvent = eventType;
        inputActionName = nameForAction;
    }
}


/// <summary>
/// Uses a coroutine manager to start and stop IEnumerator instances
/// </summary>
public class JoystickVibrator {
    private Joystick stick;

    // Kept here so we can ovveride this if we need to shake harder
    private float currentVibrationAmount;
    // Managed here so we can stop the coroutine before starting a new one through the Coroutine singleton
    private IEnumerator lastRoutine;

    public JoystickVibrator(Joystick stick) {
        this.stick = stick;
    }

    public void Vibrate(float incomingAmount, float duration, bool fadingOff) {
        if (incomingAmount > 1) incomingAmount = 1f;
        if (incomingAmount > currentVibrationAmount) {
            stick.StopVibration();
            if (lastRoutine != null) // if our last coroutine is still running, let's stop it.
                CoroutineManager.Instance.StopChildCoroutine(lastRoutine);
            lastRoutine = VibrateSticks(fadingOff, duration); // Set up our new coroutine
            currentVibrationAmount = incomingAmount; // Set the amount of vibration that we have
            //Note: we keep all of our variables in the class instead of in the coroutine because of the way the manager works
            CoroutineManager.Instance.StartChildCoroutine(lastRoutine);
        }
    }

    IEnumerator VibrateSticks(bool fade, float duration) {
        // If we don't fade, simply vibrate at full power the whole time, then stop.
        if (!fade) {
            stick.SetVibration(currentVibrationAmount, currentVibrationAmount);
            yield return new WaitForSeconds(duration);
            stick.StopVibration();
        } else {
            // This code will gently fade down our vibrations in ten steps over the course of the event
            int iterations = 0; // What step are we on?
            float remainingDuration = duration; // how much time do we have left? Set to max for illustrative purposes
            float maxAmount = currentVibrationAmount; // The strongest it has ever been in this cycle
            while (remainingDuration > 0) {
                iterations++; // increment our cycle
                remainingDuration = duration - (duration * ((float)iterations / 10)); // reduce remaining duration by 1/10th
                stick.StopVibration();
                stick.SetVibration(currentVibrationAmount, currentVibrationAmount);
                yield return new WaitForSeconds(duration / 10);
                currentVibrationAmount = maxAmount * (remainingDuration / duration); // set the new vibration proportional to our remaining duration
                // so, for example, if we have 10% of our remaining duration, max * (.1 / 1) = (.1)max = 10% of max power
            }
        }
         stick.StopVibration();
         currentVibrationAmount = 0;
         lastRoutine = null; // Set this to null, because we finished it successfully.
    }
}
