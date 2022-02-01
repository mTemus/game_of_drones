using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour {
    
    [Header("Camera")] 
    [SerializeField] private Camera mainCamera = null;
    
    private GameObject selectedDrone;
    
    private void Start() {
        SetDelegates();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && !DroneGameManager.Instance.PlacingManager.IsPlacing) {
            if (!DroneGameManager.Instance.GuiManager.PlayingIsActive) {
                TryToSelectDrone();
            }
        }
    }

    private void SetDelegates() {
        GameEventsManager.ModeChangeInitiated += DeselectDrone;
    }
    
    private void TryToSelectDrone() {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo)) {
            if (hitInfo.transform.CompareTag("Drone") && !IsMouseOverUI()) {
                if (selectedDrone == null) {
                    SelectDrone(hitInfo);
                }
                else {
                    DeselectDrone();
                    SelectDrone(hitInfo);
                }
            } else if (!IsMouseOverUI()) {
                if (selectedDrone != null) {
                    DeselectDrone();
                }
            }
        }
    }

    private void SelectDrone(RaycastHit hitInfo) {
        selectedDrone = hitInfo.transform.gameObject;
        
        DroneController dc = selectedDrone.GetComponent<DroneController>();
        dc.SetDroneSelected();
        
        DroneGameManager.Instance.SelectDrone(selectedDrone);
        DroneGameManager.Instance.DronesManager.SetCompassPosition(selectedDrone.transform.position);
        DroneGameManager.Instance.DronesManager.ToggleCompass();
        GUIEventsManager.OnSelectDroneInitiated(selectedDrone.transform.name);
        GUIEventsManager.OnShowDroneStepsInitiated(selectedDrone);
    }

    public void SelectDroneAfterPlacing() {
        TryToSelectDrone();
    }

    public void DeselectDrone() {
        if (selectedDrone != null) {
            DroneController dc = selectedDrone.GetComponent<DroneController>();
            dc.SetDroneDeselected();

            if (DroneGameManager.Instance.DronesManager.DroneCompass.activeSelf) {
                DroneGameManager.Instance.DronesManager.ToggleCompass();
            }
            DroneGameManager.Instance.DeselectDrone();
            
            selectedDrone = null;
            GUIEventsManager.OnDeselectDroneInitiated();
        }
    }

    public void SelectedDroneDeleted() {
        selectedDrone = null;
    }

    private bool IsMouseOverUI() {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public GameObject SelectedDrone => selectedDrone;
    
    
}
