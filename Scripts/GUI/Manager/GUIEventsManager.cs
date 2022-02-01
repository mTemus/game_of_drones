using System;
using UnityEngine;

public class GUIEventsManager : MonoBehaviour {
    public static Action StopTimeInitiated;
    public static Action ResumeTimeInitiated;
    public static Action DeleteAlertShowInitiated;
    public static Action DeleteAlertHideInitiated;
    public static Action<string> ChoreographyNameInitiated;
    public static Action ModeChangeInitiated;
    public static Action<string> SelectDroneInitiated;
    public static Action DeselectDroneInitiated;
    public static Action DroneStepsUpdateInitiated;
    public static Action<GameObject> ShowDroneStepsInitiated;
    public static Action DeleteDroneStepsInitiated;
    public static Action DeleteDroneReferencePointInitiated;
    public static Action NoDroneReferencePointInitiated;
    public static Action HideReferencePointInitiated;
    public static Action<String> DroneCoordinatesUpdateInitiated;
    public static Action DronePointSettingInitiated;

    public static void OnDronePointSettingInitiated() {
        DronePointSettingInitiated?.Invoke();
    }

    public static void OnDroneCoordinatesUpdateInitiated(String droneName) {
        DroneCoordinatesUpdateInitiated?.Invoke(droneName);
    }

    public static void OnHideReferencePointInitiated() {
        HideReferencePointInitiated?.Invoke();
    }

    public static void OnNoDroneReferencePointInitiated() {
        NoDroneReferencePointInitiated?.Invoke();
    }

    public static void OnDeleteDroneReferencePointInitiated() {
        DeleteDroneReferencePointInitiated?.Invoke();
    }

    public static void OnDroneStepsDeleteInitiated() {
        DeleteDroneStepsInitiated?.Invoke();
    }

    public static void OnShowDroneStepsInitiated(GameObject drone) {
        ShowDroneStepsInitiated?.Invoke(drone);
    }
    
    public static void OnDroneStepsUpdateInitiated() {
        DroneStepsUpdateInitiated?.Invoke();
    }

    public static void OnChoreographyNameInitiated(string name) {
        ChoreographyNameInitiated?.Invoke(name);
    }

    public static void OnDeleteAlertShowInitiated() {
        DeleteAlertShowInitiated?.Invoke();
    }
    
    public static void OnDeleteAlertHideInitiated() {
        DeleteAlertHideInitiated?.Invoke();
    }
    
    public static void OnStopTimeInitiated() {
        StopTimeInitiated?.Invoke();
    }
    
    public static void OnResumeTimeInitiated() {
        ResumeTimeInitiated?.Invoke();
    }

    public static void OnModeChangeInitiated() {
        ModeChangeInitiated?.Invoke();
    }

    public static void OnSelectDroneInitiated(string name) {
        SelectDroneInitiated?.Invoke(name);
    }

    public static void OnDeselectDroneInitiated() {
        DeselectDroneInitiated?.Invoke();
    }
    
    public static void ClearDelegates() {
        DeleteAlertShowInitiated = null;
        DeleteAlertHideInitiated = null;
        ChoreographyNameInitiated = null;
        StopTimeInitiated = null;
        ResumeTimeInitiated = null;
        ModeChangeInitiated = null;
        SelectDroneInitiated = null;
        DeselectDroneInitiated = null;
        DroneStepsUpdateInitiated = null;
        ShowDroneStepsInitiated = null;
        DeleteDroneReferencePointInitiated = null;
        NoDroneReferencePointInitiated = null;
        HideReferencePointInitiated = null;
        DroneCoordinatesUpdateInitiated = null;
        DronePointSettingInitiated = null;
    }
}
