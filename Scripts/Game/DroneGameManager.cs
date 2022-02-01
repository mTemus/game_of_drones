using System;
using System.Collections.Generic;
using UnityEngine;

public class DroneGameManager : MonoBehaviour
{
    [Header("Managers:")]
    [SerializeField] private ChoreographyManager choreographyManager = null;
    [SerializeField] private DronesManager dronesManager = null;
    [SerializeField] private PlacingManager placingManager = null;
    [SerializeField] private SelectionManager selectionManager = null;
    [SerializeField] private GUIManager guiManager = null;
    [SerializeField] private CamerasManager camerasManager = null;
    [SerializeField] private DataManager dataManager = null;
    [SerializeField] private EnvironmentManager environmentManager = null;
    [SerializeField] private TimeManager timeManager = null;
    // [SerializeField] private ReportingManager reportingManager = null;

    public static DroneGameManager Instance { get; protected set; }
    
    private void Awake() {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    public void ExitToMainMenu() {
        guiManager.GoBackToMainMenu();
    }

    public void SaveData() {
        GameEventsManager.OnSaveInitiated();
        guiManager.ShowSavedAlert();
    }

    public void SaveDataAndExit() {
        GameEventsManager.OnSaveInitiated();
        guiManager.GoBackToMainMenu();
    }

    public void LoadData(Choreography choreography) {
        GameEventsManager.OnLoadInitiated(choreography);
    }

    public void RegisterDrone(GameObject drone) {
        GameEventsManager.OnDroneRegisterInitiated(drone);
    }

    public void SelectDrone(GameObject drone) {
        GameEventsManager.OnDroneSelectInitiated(drone);
    }

    public void DeselectDrone() {
        GameEventsManager.OnDroneDeselectInitiated();
    }

    public void ToggleTime() {
        //TODO: fix bug, after starting first time application is changing mode if there was no mouse click

        if (!dronesManager.SeparationCollisionError) {
            GameEventsManager.OnTimeToggleInitiated();
            guiManager.ChangeTimeButtonIcon();
        }
    }

    public void MoveDrone() {
        DronesManager.MoveDrone();
        GUIEventsManager.OnDeselectDroneInitiated();
    }

    public void ToggleApplicationMode() {
        if (!GameEventsManager.OnDroneQueueCheckInitiated()) return;
        
        GameEventsManager.OnModeChangeInitiated();
        GUIEventsManager.OnModeChangeInitiated();
    }

    public void SetChoreographyName(String name) {
        GUIEventsManager.OnChoreographyNameInitiated(name);
    }

    public void ToggleSeparation() {
        GameEventsManager.OnSeparationToggleInitiated();
    }

    public void CheckDronesSeparation() {
        GameEventsManager.OnSeparationCheckInitiated();
    }

    public bool CheckDronesSeparationError() {
        return dronesManager.CheckSeparationError();
    }

    public void DeleteDrone() {
        GameEventsManager.OnDroneDeleteInitiated();
        GUIEventsManager.OnDeselectDroneInitiated();
        GameEventsManager.OnSeparationCheckInitiated();
    }

    public void UpdateDroneSteps() {
        GUIEventsManager.OnDroneStepsUpdateInitiated();
    }

    public void DeleteDroneSteps() {
        GUIEventsManager.OnDroneStepsDeleteInitiated();
    }

    public void RewindChoreography() {
        GameEventsManager.OnChoreographyRewindInitiated();
    }

    public void DeleteDroneReferencePoint() {
        GameEventsManager.OnDroneReferencePointDeleteInitiated();
    }

    public void ShowDeleteReferencePointAlert() {
        GUIEventsManager.OnDeleteDroneReferencePointInitiated();
    }

    public void ShowNoDroneReferencePointAlert() {
        GUIEventsManager.OnNoDroneReferencePointInitiated();
    }

    public void HideDroneReferencePointAlert() {
        GUIEventsManager.OnDeleteAlertHideInitiated();
    }

    public void HideDroneReferencePoint() {
        GUIEventsManager.OnHideReferencePointInitiated();
    }

    public void MoveDroneToCoordinates(Vector3 newPosition) {
        GameEventsManager.OnDroneMoveToCoordinatesInitiated(newPosition);
    }

    public void ShowUpdatedDroneCoordinates() {
        string droneName = dronesManager.SelectedDrone.name;
        GUIEventsManager.OnDroneCoordinatesUpdateInitiated(droneName);
    }

    public void ToggleDronePoint() {
        GameEventsManager.OnDronePointSettingInitiated();
        GUIEventsManager.OnDronePointSettingInitiated();
    }

    public void UpdateTimerDivider(float time) {
        GameEventsManager.OnUpdateTimerDividerInitiated(time);
    }

    public void SeparationCollisionOccured(GameObject declarant, GameObject collider) {
        GameEventsManager.OnSeparationCollisionInitiated(declarant, collider);
    }
    
    public ChoreographyManager ChoreographyManager => choreographyManager;

    public DronesManager DronesManager => dronesManager;

    public PlacingManager PlacingManager => placingManager;

    public SelectionManager SelectionManager => selectionManager;
    
    public GUIManager GuiManager => guiManager;

    public CamerasManager CamerasManager => camerasManager;

    public DataManager DataManager => dataManager;

    public EnvironmentManager EnvironmentManager => environmentManager;

    public TimeManager TimeManager => timeManager;
    
    private void OnDestroy() {
        GameEventsManager.ClearDelegates();
        GUIEventsManager.ClearDelegates();
    }
}
