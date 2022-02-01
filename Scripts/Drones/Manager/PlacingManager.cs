using UnityEngine;

public class PlacingManager : MonoBehaviour {

    [Header("Object references")]
    [SerializeField] private GameObject dronePrefab = null;
    [SerializeField] private GameObject dronePointPrefab = null;
    [SerializeField] private Camera mainCamera = null;
    
    [Header("Key Codes")] 
    [SerializeField] private KeyCode dronePlacingKeyCode;
    [SerializeField] private KeyCode pointPlacingKeyCode;

    [Header("Rendering layers")] 
    [SerializeField] private LayerMask groundMask;

    private bool isPlacing;
    private GameObject currentPlacedObject;
    private float mouseWheelRotating;
    private Vector3 droneOffsetOnY = new Vector3(0, 0.205f, 0);
    private Vector3 pointOffsetOnY = new Vector3(0, 0.07f, 0);

    private void Awake() {
        dronePlacingKeyCode = KeyCode.P;
        pointPlacingKeyCode = KeyCode.O;
        isPlacing = false;
        
        SetDelegates();
    }

    // Update is called once per frame
    void Update()
    {
        if (!DroneGameManager.Instance.GuiManager.PlayingIsActive) {
            PlaceNewObjectHotKey();
        }

        if (isPlacing) {
            if (currentPlacedObject != null) {
                MoveCurrentPlacingObject();
                RotateDroneFromMouseWheel();
                PlaceObjectOnClick();
            }
        }
    }

    private void SetDelegates() {
        GameEventsManager.DroneMovementInitiated += SetDroneToMove;
    }

    private void SetDroneToMove(GameObject drone) {
        DroneGameManager.Instance.DronesManager.ToggleCompass();
        currentPlacedObject = drone;
        TogglePlacing();
    }

    private void MoveCurrentPlacingObject() {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask)) {
            if (currentPlacedObject.CompareTag("Drone")) {
                currentPlacedObject.transform.position = hitInfo.point + droneOffsetOnY;
            }
            else { currentPlacedObject.transform.position = hitInfo.point + pointOffsetOnY; }
            
            currentPlacedObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
    }

    private void RotateDroneFromMouseWheel() {
        mouseWheelRotating += Input.mouseScrollDelta.y;
        currentPlacedObject.transform.Rotate(Vector3.up, mouseWheelRotating * 10f);
    }
    
    private void PlaceNewObjectHotKey() {
        if (DroneGameManager.Instance.SelectionManager.SelectedDrone == null) {
            if (Input.GetKeyDown(dronePlacingKeyCode)) {
                if (DroneGameManager.Instance.DronesManager.DroneReferencePoint != null) {
                    TogglePlacing();
                    if (currentPlacedObject == null) { currentPlacedObject = Instantiate(dronePrefab); }
                    else {
                        if (!DroneGameManager.Instance.DronesManager.Drones.ContainsValue(currentPlacedObject)) 
                            Destroy(currentPlacedObject);
                    }
                } else {
                    DroneGameManager.Instance.ShowNoDroneReferencePointAlert();
                }
            } else if (Input.GetKeyDown(pointPlacingKeyCode)) {
                if (DroneGameManager.Instance.DronesManager.DroneReferencePoint == null) {
                    TogglePlacing();
                    if (currentPlacedObject == null) { currentPlacedObject = Instantiate(dronePointPrefab); }
                    else {
                        DroneGameManager.Instance.DronesManager.DeleteAllDrones();
                        Destroy(currentPlacedObject);
                    }
                }
                else {
                    if (DroneGameManager.Instance.SelectionManager.SelectedDrone == null) {
                        DroneGameManager.Instance.ShowDeleteReferencePointAlert();
                    }
                }
            } 
        }
    }

    private void PlaceObjectOnClick() {
        if (Input.GetMouseButtonDown(0)) {
            if (currentPlacedObject.CompareTag("Drone")) {
                DroneGameManager.Instance.RegisterDrone(currentPlacedObject);
                DroneGameManager.Instance.SelectionManager.DeselectDrone();
                DroneGameManager.Instance.DronesManager.CheckSeparation();
                currentPlacedObject = null;
                TogglePlacing();
                DroneGameManager.Instance.SelectionManager.SelectDroneAfterPlacing();
            } else if (currentPlacedObject.CompareTag("DronePoint")) {
                DroneGameManager.Instance.DronesManager.SetDronePoint(currentPlacedObject);
                currentPlacedObject = null;
                TogglePlacing();
            }
        }
    }

    private void TogglePlacing() {
        isPlacing = !isPlacing;
    }

    public bool IsPlacing => isPlacing;
}
