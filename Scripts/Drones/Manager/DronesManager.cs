using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DronesManager : MonoBehaviour
{
    [Header("Compass")]
    [SerializeField] private GameObject droneCompass = null;

    private Dictionary<string, GameObject> drones;
    private Dictionary<GameObject, FlyingController> dronesWithFlyingData;
    private Dictionary<string, List<float>> dronesCoordinates;

    private GameObject droneReferencePoint;
    private GameObject selectedDrone;
    private bool isSeparationActive = true;
    private bool firstStart;
    private bool dronePointChecked;
    private bool dronePointsCreated;
    private bool dronePointsDestroyed = true;
    private bool separationCollisionError = false;

    private ChoreographyManager choreographyManagerShortcut;
    
    private void Awake() {
        drones = new Dictionary<string, GameObject>();
        dronesWithFlyingData = new Dictionary<GameObject, FlyingController>();
        dronesCoordinates = new Dictionary<string, List<float>>();

        firstStart = true;
        
        SetDelegates();
    }

    private void Start() {
        choreographyManagerShortcut = DroneGameManager.Instance.ChoreographyManager;
    }

    private void SetDelegates() {
        GameEventsManager.DroneRegisterInitiated = RegisterDroneInChoreography;
        GameEventsManager.LoadInitiated += LoadDroneReferencePoint;
        GameEventsManager.LoadInitiated += LoadOtherDronesData;
        GameEventsManager.LoadInitiated += LoadDrones;
        GameEventsManager.SaveInitiated += SaveDroneReferencePoint;
        GameEventsManager.SaveInitiated += SaveDroneData;
        GameEventsManager.DroneSelectInitiated = SetSelectedDrone;
        GameEventsManager.DroneDeselectInitiated = UnsetSelectedDrone;
        GameEventsManager.SeparationToggleInitiated = ToggleDronesSeparation;
        GameEventsManager.SeparationCheckInitiated += CheckSeparation;
        GameEventsManager.DroneDeleteInitiated = DeleteDrone;
        GameEventsManager.RewindChoreographyInitiated += RewindDronesChoreography;
        GameEventsManager.DroneReferencePointDeleteInitiated += DeleteDroneReferencePoint;
        GameEventsManager.ChoreographyUpdateInitiated += UpdateDroneChoreographyProperties;
        GameEventsManager.DroneMoveToCoordinatesInitiated += MoveSelectedDroneToCoordinates;
        GameEventsManager.DronePointSettingInitiated += ToggleDronePoint;
        GameEventsManager.ModeChangeInitiated += SetDronePoints;
        GameEventsManager.ModeChangeInitiated += ToggleDroneColliders;
        GameEventsManager.ModeChangeInitiated += ToggleDronesSeparationColliders;
        GameEventsManager.UpdateTimerDividerInitiated += SetCoordinatesTimeCounter;
        GameEventsManager.SeparationCollisionInitiated += SeparationCollisionOccured;
        GameEventsManager.DroneQueueCheckInitiated += CheckDronesQueues;

        GUIEventsManager.HideReferencePointInitiated += HideReferencePoint;
    }
    
    private void ToggleDronesSeparation() {
        foreach (var drone in drones.Values) {
            drone.GetComponent<SeparationController>().ToggleSeparation();
        }

        isSeparationActive = !isSeparationActive;
    }

    private void ToggleDronesSeparationColliders() {
        foreach (var drone in drones.Values) {
            drone.GetComponent<SeparationController>().ToggleSeparationCollider();
        }
    }

    private void ResetDronesChoreography() {
        foreach (GameObject drone in drones.Values) {
            FlyingController myFK = drone.GetComponent<FlyingController>();
            myFK.SetChoreography();
            myFK.ClearExport();
        }
    }

    private void RewindDronesChoreography() {
        if (DroneGameManager.Instance.ChoreographyManager.IsRunning) {
            DroneGameManager.Instance.ToggleTime();
        }
        
        MoveDronesToStartingPositions();
        ResetDronesChoreography();
    }

    private void MoveDronesToStartingPositions() {
        foreach (string droneName in dronesCoordinates.Keys) {
            GameObject drone = drones[droneName];
            List<float> startPosition = dronesCoordinates[droneName];
            drone.transform.position = new Vector3(startPosition[0], startPosition[1], startPosition[2]); 
            
        }
    }
    
    private void LoadDrones(Choreography choreographyData) {

        for (int i = 1; i <= choreographyData.DroneAmount; i++) {
            string droneName = "drone_" + i;
            Vector3 dronePosition = new Vector3(dronesCoordinates[droneName][0],dronesCoordinates[droneName][1],dronesCoordinates[droneName][2]);
            Queue<Step> droneChoreography = DataManager.Load<Queue<Step>>(choreographyData.Name, droneName);
            GameObject drone = (GameObject) Instantiate(Resources.Load("Prefabs/Drones/Quad"), droneReferencePoint.transform, true);
            drone.transform.position = dronePosition;

            while (droneChoreography.Count != 0) {
                drone.GetComponent<FlyingController>().CreateStep(droneChoreography.Dequeue());
            }
            
            float separationSize = drone.GetComponent<SeparationController>().SeparationSize;
        
            drone.GetComponent<FlyingController>().UpdateMove();
            
            drone.AddComponent<BoxCollider>();
            drone.GetComponent<BoxCollider>().size = new Vector3(separationSize, separationSize, separationSize);
            drone.GetComponent<BoxCollider>().isTrigger = true;
            drone.GetComponent<BoxCollider>().enabled = false;
            
            drone.name = droneName;
            drones[droneName] = drone;
            dronesWithFlyingData[drone] = drone.GetComponent<FlyingController>();
            
            drone.GetComponent<SeparationController>().CheckCollisionsAfterPlacement();
        }
    }

    private void LoadOtherDronesData(Choreography choreographyData) {
        DronesCoordinates = DataManager.Load<Dictionary<string, List<float>>>(choreographyData.Name, "coordinates");
    }

    private void SaveDroneData() {
        Choreography choreographyData = choreographyManagerShortcut.ChoreographyData;
        
        DataManager.Save(dronesCoordinates, choreographyData.Name, "coordinates");
        
        foreach (var drone in dronesWithFlyingData.Keys) {
            DataManager.Save(dronesWithFlyingData[drone].ChoreographySteps, choreographyData.Name, drone.name);
        }
    }

    private void SaveDroneReferencePoint() {
        Choreography choreographyData = choreographyManagerShortcut.ChoreographyData;
        Vector3 position = droneReferencePoint.transform.position;
        List<float> positionList = new List<float>{ position.x, position.y, position.z };
        DataManager.Save(positionList, choreographyData.Name, droneReferencePoint.name);
    }

    private void LoadDroneReferencePoint(Choreography choreography) {
        List<float> positionList = DataManager.Load<List<float>>(choreography.Name, "DroneReferencePoint");
        Vector3 position = new Vector3(positionList[0], positionList[1], positionList[2]);
        droneReferencePoint = (GameObject) Instantiate(Resources.Load("Prefabs/Drones/DroneReferencePoint"));
        droneReferencePoint.transform.position = position;
        droneReferencePoint.name = "DroneReferencePoint";
    }
    
    private void SetSelectedDrone(GameObject drone) {
        selectedDrone = drone;
    }
    
    private void UnsetSelectedDrone() {
        selectedDrone.GetComponent<DroneController>().SetDroneDeselected();
        selectedDrone = null;
    }
    
    private void DeleteDrone() {
        drones.Remove(selectedDrone.name);
        dronesCoordinates.Remove(selectedDrone.name);
        dronesWithFlyingData.Remove(selectedDrone);

        DestroyImmediate(selectedDrone);
        selectedDrone = null;
        ToggleCompass();
        DroneGameManager.Instance.SelectionManager.SelectedDroneDeleted();
    }

    private void DeleteDroneReferencePoint() {
        if (droneReferencePoint != null) {
            Destroy(droneReferencePoint);
        }
    }

    private void HideReferencePoint() {
        if (droneReferencePoint != null) {
            droneReferencePoint.GetComponent<MeshRenderer>().enabled 
                = droneReferencePoint.GetComponent<MeshRenderer>().enabled != true;
        }
    }

    private float UpdateStepSpeed(Step step, Choreography chor) {
        float speed = 0;

        switch (step.State) {
            case MoveMachine.Start:
            case MoveMachine.Stop:
            case MoveMachine.LightOn:
            case MoveMachine.LightOff:
                speed = 0;
                break;
            
            case MoveMachine.FlyVerticallyUp:
            case MoveMachine.FlyVerticallyToXPlusOnYPlus:
            case MoveMachine.FlyVerticallyToXMinusOnYPlus:
            case MoveMachine.FlyVerticallyToZPlusOnYPlus:
            case MoveMachine.FlyVerticallyToZMinusOnYPlus:
            case MoveMachine.FlyVerticallyToXPlusZPlusOnYPlus:
            case MoveMachine.FlyVerticallyToXPlusZMinusOnYPlus:
            case MoveMachine.FlyVerticallyToXMinusZPlusOnYPlus:
            case MoveMachine.FlyVerticallyToXMinusZMinusOnYPlus:
                speed = chor.VerticalRisingSpeed;
                break;
            
            case MoveMachine.FlyVerticallyDown:
            case MoveMachine.FlyVerticallyToXPlusOnYMinus:
            case MoveMachine.FlyVerticallyToXMinusOnYMinus:
            case MoveMachine.FlyVerticallyToZPlusOnYMinus:
            case MoveMachine.FlyVerticallyToZMinusOnYMinus:
            case MoveMachine.FlyVerticallyToXPlusZPlusOnYMinus:
            case MoveMachine.FlyVerticallyToXPlusZMinusOnYMinus:
            case MoveMachine.FlyVerticallyToXMinusZPlusOnYMinus:
            case MoveMachine.FlyVerticallyToXMinusZMinusOnYMinus:
                speed = chor.VerticalFallingSpeed;
                break;

            case MoveMachine.FlyHorizontallyToXPlusOnYZero:
            case MoveMachine.FlyHorizontallyToXMinusOnYZero:
            case MoveMachine.FlyHorizontallyToZPlusOnYZero:
            case MoveMachine.FlyHorizontallyToZMinusOnYZero:
            case MoveMachine.FlyHorizontallyToXPlusZPlusOnYZero:
            case MoveMachine.FlyHorizontallyToXPlusZMinusOnYZero:
            case MoveMachine.FlyHorizontallyToXMinusZPlusOnYZero:
            case MoveMachine.FlyHorizontallyToXMinusZMinusOnYZero:
                speed = chor.HorizontalSpeed;
                break;

            case MoveMachine.HalfCircleToXPlusOnZMinus:
            case MoveMachine.HalfCircleToXPlusOnZPlus:
            case MoveMachine.HalfCircleToXMinusOnZMinus:
            case MoveMachine.HalfCircleToXMinusOnZPlus:
            case MoveMachine.HalfCircleToYPlusOnXMinus:
            case MoveMachine.HalfCircleToYPlusOnxPlus:
            case MoveMachine.HalfCircleToYMinusOnXMinus:
            case MoveMachine.HalfCircleToYMinusOnXPlus:
            case MoveMachine.HalfCircleToZPlusOnXMinus:
            case MoveMachine.HalfCircleToZPlusOnXPlus:
            case MoveMachine.HalfCircleToZMinusOnXMinus:
            case MoveMachine.HalfCircleToZMinusOnXPlus:
                speed = 1f;
                break;
            
            
            // default:
            //     Debug.LogError("ERROR -- can't update speed of drone step!");
            //     break;
        }

        return speed;
    }
    
    private void UpdateDroneChoreographyProperties(Choreography newChor) {
        foreach (GameObject drone in drones.Values) {
            List<Step> droneSteps = new List<Step>(drone.GetComponent<FlyingController>().ChoreographySteps);

            foreach (Step step in droneSteps) {
                step.Speed = UpdateStepSpeed(step, newChor);
            }

            SeparationController droneSC = drone.GetComponent<SeparationController>();
            droneSC.SetSeparationSize(newChor.Separation, newChor.DroneSize);
            droneSC.UpdateSeparationSize();
        }
        
        CheckSeparation();
        CheckSeparationError();
    }

    private void MoveSelectedDroneToCoordinates(Vector3 newPosition) {
        selectedDrone.transform.localPosition = newPosition;
        Vector3 selectedDronePosition = selectedDrone.transform.position;
        
        SetCompassPosition(selectedDronePosition);
        UpdateDroneCoordinates(selectedDrone, selectedDronePosition);
        CheckSeparation();
    }

    private void ToggleDronePoint() {
        dronePointChecked = !dronePointChecked;
    }

    private void SetDronePoints() {
        if (dronePointChecked && dronePointsDestroyed) {
            foreach (GameObject drone in drones.Values) {
                GameObject dronePoint = new GameObject("point_" + drone.name);
                
                Transform dronePointTransform = dronePoint.transform;
                Transform droneTransform = drone.transform;
                
                dronePointTransform.SetParent(droneReferencePoint.transform);
                dronePointTransform.position = droneTransform.position;
                
                drone.transform.SetParent(dronePoint.transform);
            }
            dronePointsCreated = true;
            dronePointsDestroyed = false;
        } else if(!dronePointsDestroyed && dronePointsCreated && dronePointChecked){
            foreach (GameObject drone in drones.Values) {
                Transform droneTransform = drone.transform;
                GameObject dronePoint = droneTransform.parent.gameObject;
                droneTransform.parent = droneReferencePoint.transform;
                
                Destroy(dronePoint);
            }
            dronePointsCreated = false;
            dronePointsDestroyed = true;
        }
    }

    private void SetCoordinatesTimeCounter(float time) {
        foreach (GameObject drone in drones.Values) {
            drone.GetComponent<FlyingController>().SaveCoordinatesCounter = time;
        }
    }

    private void UpdateDroneCoordinates(GameObject drone, Vector3 position) {
        dronesCoordinates[drone.name] = new List<float> { position.x, position.y, position.z };
    }
    
    private void ToggleDroneColliders() {
        foreach (GameObject drone in drones.Values) {
            drone.GetComponent<BoxCollider>().enabled = !drone.GetComponent<BoxCollider>().enabled;
            drone.GetComponent<MeshCollider>().enabled = !drone.GetComponent<MeshCollider>().enabled;
            drone.GetComponent<DroneController>().PlayingModeActive =
                !drone.GetComponent<DroneController>().PlayingModeActive;
        }

        separationCollisionError = false;
    }

    private void SeparationCollisionOccured(GameObject declarant, GameObject collider) {
        if (!separationCollisionError) {
            separationCollisionError = true;
            GameEventsManager.OnTimeToggleInitiated();
            DroneGameManager.Instance.GuiManager.SeparationCollisionOccured(declarant, collider);
        }
    }
    
    private bool CheckDronesQueues() {
        List<GameObject> emptyDrones = new List<GameObject>();

        foreach (GameObject drone in drones.Values) {
            if (drone.GetComponent<FlyingController>().ChoreographySteps.Count == 0) {
                emptyDrones.Add(drone);
            }
        }

        if (emptyDrones.Count > 0) {
            DroneGameManager.Instance.GuiManager.WarnEmptyDroneQueue(emptyDrones);
            return false;
        }

        return true;
    }

    public void DeleteAllDrones() {
        foreach (GameObject drone in drones.Values) {
            Destroy(drone);
        }
        
        drones = new Dictionary<string, GameObject>();
        dronesCoordinates = new Dictionary<string, List<float>>();
        dronesWithFlyingData = new Dictionary<GameObject, FlyingController>();
        selectedDrone = null;
    }

    public void ToggleDroneTime(bool isRunning) { 
        if (firstStart) {
            MoveDronesToStartingPositions();
            ResetDronesChoreography();
            firstStart = false;
        }
        
        foreach (var flyingController in dronesWithFlyingData.Values) { flyingController.ChoreographyIsRunning = isRunning; }
    }
    
    public void RegisterDroneInChoreography(GameObject drone) {
        if (drones.ContainsValue(drone)) {
            UpdateDroneCoordinates(drone);
        }
        else {
            DroneGameManager.Instance.ChoreographyManager.ChoreographyData.DroneAmount += 1;
            String droneName = "drone_" + choreographyManagerShortcut.ChoreographyData.DroneAmount;
            List<float> droneCoordinates = new List<float>();
            var position = drone.transform.position;
            droneCoordinates.Add(position.x);
            droneCoordinates.Add(position.y);
            droneCoordinates.Add(position.z);
            drone.name = droneName;
            drone.transform.SetParent(droneReferencePoint.transform);

            float separationSize = drone.GetComponent<SeparationController>().SeparationSize;
        
            drone.AddComponent<BoxCollider>();
            drone.GetComponent<BoxCollider>().size = new Vector3(separationSize, separationSize, separationSize);
            drone.GetComponent<BoxCollider>().isTrigger = true;
            drone.GetComponent<BoxCollider>().enabled = false;
        
            drones[droneName] = drone;
            dronesWithFlyingData[drone] = drone.GetComponent<FlyingController>();
            dronesCoordinates[droneName] = new List<float>(droneCoordinates);
        
            drone.GetComponent<SeparationController>().CheckCollisionsAfterPlacement();
        }
    }

    public void UpdateDroneCoordinates(GameObject drone) {
        List<float> droneCoordinates = new List<float>();
        var position = drone.transform.position;
        droneCoordinates.Add(position.x);
        droneCoordinates.Add(position.y);
        droneCoordinates.Add(position.z);
        dronesCoordinates[drone.name] = new List<float>(droneCoordinates);
    }

    public void DeactivateBoxColliders() {
        foreach (var drone in drones.Values) {
            drone.GetComponent<BoxCollider>().enabled = false; 
            drone.GetComponent<MeshCollider>().enabled = true;
        }
    }

    public void ActivateBoxColliders() {
        foreach (var drone in drones.Values) {
            drone.GetComponent<BoxCollider>().enabled = true;
            drone.GetComponent<MeshCollider>().enabled = false;
        }
    }

    public void MoveDrone() {
        GameEventsManager.OnDroneMovementInitiated(selectedDrone);
    }

    public void CheckSeparation() {
        foreach (var drone in drones.Values) {
            drone.GetComponent<SeparationController>().CheckCollisionsAfterPlacement();
        }
    }

    public bool CheckSeparationError() {
        foreach (var drone in drones.Values) {
            if (drone.GetComponent<SeparationController>().SeparationHasError) { return true; }
        }
        return false;
    }

    public void SetDronePoint(GameObject point) {
        if (droneReferencePoint == null) {
            droneReferencePoint = point;
            droneReferencePoint.transform.name = "DroneReferencePoint";
        }
    }

    public void ToggleCompass() {
        droneCompass.SetActive(!droneCompass.activeSelf);
    }

    public void SetCompassPosition(Vector3 position) {
        droneCompass.transform.position = position + new Vector3(0, 2, 0);
    }

    public bool IsSeparationActive => isSeparationActive;
    
    public Dictionary<string, GameObject> Drones {
        get => drones;
        set => drones = value;
    }

    public Dictionary<GameObject, FlyingController> DronesWithData {
        get => dronesWithFlyingData;
        set => dronesWithFlyingData = value;
    }

    public Dictionary<string, List<float>> DronesCoordinates {
        get => dronesCoordinates;
        set => dronesCoordinates = value;
    }

    public GameObject SelectedDrone => selectedDrone;

    public GameObject DroneCompass => droneCompass;

    public GameObject DroneReferencePoint => droneReferencePoint;

    public bool DronePointChecked => dronePointChecked;

    public bool SeparationCollisionError => separationCollisionError;
}
