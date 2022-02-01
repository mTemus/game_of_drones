using System;
using UnityEngine;

public class GameEventsManager : MonoBehaviour {
    public static Action SaveInitiated;
    public static Action<Choreography> LoadInitiated;

    public static Action<GameObject> DroneRegisterInitiated;
    public static Action<GameObject> DroneSelectInitiated;
    public static Action DroneDeselectInitiated;
    public static Action TimeToggleInitiated;
    public static Action SeparationToggleInitiated;
    public static Action<GameObject> DroneMovementInitiated;
    public static Action SeparationCheckInitiated;
    public static Action DroneDeleteInitiated;
    public static Action RewindChoreographyInitiated;
    public static Action DroneReferencePointDeleteInitiated;
    public static Action<Choreography> ChoreographyUpdateInitiated;
    public static Action<Vector3> DroneMoveToCoordinatesInitiated;
    public static Action DronePointSettingInitiated;
    public static Action ModeChangeInitiated;
    public static Action<float> UpdateTimerDividerInitiated;
    public static Action<GameObject, GameObject> SeparationCollisionInitiated;
    public static Func<bool> DroneQueueCheckInitiated;

    public static bool OnDroneQueueCheckInitiated() {
        if (DroneQueueCheckInitiated.Invoke()) return true; 
        return false;
    }

    public static void OnSeparationCollisionInitiated(GameObject declarant, GameObject collider) {
        SeparationCollisionInitiated?.Invoke(declarant, collider);
    }
    
    public static void OnUpdateTimerDividerInitiated(float time) {
        UpdateTimerDividerInitiated?.Invoke(time);
    }

    public static void OnModeChangeInitiated() {
        ModeChangeInitiated?.Invoke();
    }
    
    public static void OnDronePointSettingInitiated() {
        DronePointSettingInitiated?.Invoke();
    }
    
    public static void OnDroneMoveToCoordinatesInitiated(Vector3 newPosition) {
        DroneMoveToCoordinatesInitiated?.Invoke(newPosition);
    }

    public static void OnChoreographyUpdateInitiated(Choreography choreography) {
        ChoreographyUpdateInitiated?.Invoke(choreography);
    }

    public static void OnDroneReferencePointDeleteInitiated() {
        DroneReferencePointDeleteInitiated?.Invoke();
    }
    
    public static void OnChoreographyRewindInitiated() {
        RewindChoreographyInitiated?.Invoke();
    }
    
    public static void OnDroneDeleteInitiated() {
        DroneDeleteInitiated?.Invoke();
    }
    
    public static void OnSeparationCheckInitiated() {
        SeparationCheckInitiated?.Invoke();
    }
    
    public static void OnSaveInitiated() { 
        SaveInitiated?.Invoke();
    }

    public static void OnLoadInitiated(Choreography choreography) {
        LoadInitiated?.Invoke(choreography);
    }

    public static void OnTimeToggleInitiated() {
        TimeToggleInitiated?.Invoke();
    }

    public static void OnDroneRegisterInitiated(GameObject drone) {
        DroneRegisterInitiated?.Invoke(drone);
    }

    public static void OnDroneSelectInitiated(GameObject drone) {
        DroneSelectInitiated?.Invoke(drone);
    }

    public static void OnDroneDeselectInitiated() {
        DroneDeselectInitiated?.Invoke();
    }

    public static void OnSeparationToggleInitiated() {
        SeparationToggleInitiated?.Invoke();
    }

    public static void OnDroneMovementInitiated(GameObject droneToMove) {
        DroneMovementInitiated?.Invoke(droneToMove);
    }

    public static void ClearDelegates() {
        SaveInitiated = null;
        LoadInitiated = null;
        TimeToggleInitiated = null;
        DroneRegisterInitiated = null;
        DroneSelectInitiated = null;
        DroneDeselectInitiated = null;
        SeparationToggleInitiated = null;
        DroneMovementInitiated = null;
        SeparationCheckInitiated = null;
        DroneDeleteInitiated = null;
        DroneReferencePointDeleteInitiated = null;
        ChoreographyUpdateInitiated = null;
        DroneMoveToCoordinatesInitiated = null;
        DronePointSettingInitiated = null;
        ModeChangeInitiated = null;
        UpdateTimerDividerInitiated = null;
        SeparationCollisionInitiated = null;
        DroneQueueCheckInitiated = null;
    }
}
