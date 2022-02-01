using UnityEngine;

public class SeparationController : MonoBehaviour {
    
    [Header("Game objects")]
    [SerializeField] private GameObject separationGO = null;

    [Header("Materials")]
    [SerializeField] private Material separationGreen = null;
    [SerializeField] private Material separationRed = null;
    [SerializeField] private Material separationInvisible = null;
    
    private MeshRenderer separationRenderer;
    private float separation;
    private float droneSize;
    private float separationSize;
    private bool separationHasError;
    private bool invisible = false;

    private void Awake() {
        separationRenderer = separationGO.GetComponent<MeshRenderer>();
        separation = DroneGameManager.Instance.ChoreographyManager.ChoreographyData.Separation;
        droneSize = DroneGameManager.Instance.ChoreographyManager.ChoreographyData.DroneSize;
        separationSize = separation + droneSize;
        separationHasError = false;
        UpdateSeparationSize();
    }

    public void CheckCollisionsAfterPlacement() {
        Transform droneTransform = transform;
        DroneGameManager.Instance.DronesManager.ActivateBoxColliders();
        bool hadCollision = false;
        
        Collider[] objectsInDroneArea =
            Physics.OverlapBox(droneTransform.position, separationGO.transform.localScale / 2, separationGO.transform.rotation);
        
        foreach (var objectOnHit in objectsInDroneArea) {
            if (objectOnHit.CompareTag("Drone")) {
                if (!objectOnHit.transform.name.Equals(droneTransform.name)) {
                    objectOnHit.GetComponent<SeparationController>().SetSeparationWrong();
                    hadCollision = true;
                }
            }
        }

        if (hadCollision) SetSeparationWrong(); 
        else SetSeparationOk(); 
        
        DroneGameManager.Instance.DronesManager.DeactivateBoxColliders();
    }
    
    public void UpdateSeparationSize() {
        separationGO.transform.localScale = new Vector3(separationSize, separationSize, separationSize);
    }

    public void SetSeparationSize(float separation, float droneSize) {
        this.separation = separation;
        this.droneSize = droneSize;

        separationSize = separation + droneSize;
    }
    
    public void SetSeparationOk() {
        separationRenderer.material = separationGreen;
        separationHasError = false;
    }
    
    public void SetSeparationWrong() {
        separationRenderer.material = separationRed;
        separationHasError = true;
    }

    public void ToggleSeparation() {
        if (invisible) {
            CheckCollisionsAfterPlacement();
            invisible = false;
        }
        else {
            separationRenderer.material = separationInvisible;
            invisible = true;
        }
    }

    public void ToggleSeparationCollider() {
        separationGO.GetComponent<BoxCollider>().enabled = !separationGO.GetComponent<BoxCollider>().enabled;
    }
    
    public bool SeparationHasError => separationHasError;

    public float SeparationSize => separationSize;
}
