using System;
using UnityEngine;
using UnityEngine.Serialization;

public class DroneController : MonoBehaviour {
    
    [Header("Drone materials")]
    [SerializeField] private Material droneNormal = null;
    [SerializeField] private Material droneSelected = null;
    [SerializeField] private Material lightOn = null;
    [SerializeField] private Material lightOff = null;

    [Header("Drone parts")]
    [SerializeField] private MeshRenderer droneRenderer = null;
    [SerializeField] private GameObject sphereLight = null;
    [SerializeField] private GameObject[] propellers = null;

    private bool playingModeActive = false;
    private bool lightIsOn = false;
    private FlyingController myFlyingController;

    private void Start() {
        myFlyingController = transform.GetComponent<FlyingController>();
        
        SetDelegates();
    }

    private void Update() {
        if (myFlyingController.ChoreographyIsRunning) {
            if (myFlyingController.IsMoving) {
                foreach (GameObject propeller in propellers) { 
                    propeller.transform.Rotate(new Vector3(0.0f, 0.0f, 30.0f) * (50 * Time.deltaTime));
                }
            }
        }
    }

    private void SetDelegates() {
        GameEventsManager.RewindChoreographyInitiated += ResetLight;
    }

    private void ResetLight() {
        sphereLight.GetComponent<MeshRenderer>().material = lightOff;
    }
    
    public void SetDroneSelected() {
        droneRenderer.material = droneSelected;
    }

    public void SetDroneDeselected() {
        droneRenderer.material = droneNormal;
    }

    public void ToggleLight() {
        sphereLight.GetComponent<MeshRenderer>().material = !lightIsOn ? lightOn : lightOff;
    }

    private void OnTriggerEnter(Collider other) {
        if (playingModeActive) {
            if (other.transform.CompareTag("Drone")) {
                if (other.transform.parent.name != transform.name) {
                    DroneGameManager.Instance.SeparationCollisionOccured(transform.gameObject, other.transform.parent.gameObject);
                }
            }
        }
    }
    
    public bool PlayingModeActive {
        get => playingModeActive;
        set => playingModeActive = value;
    }
    
    
    
}