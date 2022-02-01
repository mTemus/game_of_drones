using UnityEngine;

public class ChoreographyManager : MonoBehaviour {
    
    private Choreography choreographyData;
    private bool isRunning;
    
    private void Awake() {
        SetDelegates();
        isRunning = false;
    }

    void Start() {
        if (CheckChoreographyMode()) {
            string name = PlayerPrefs.GetString("name");
            float separation = PlayerPrefs.GetFloat("separation");
            float horizontalSpeed = PlayerPrefs.GetFloat("horSpeed");
            float verticalRisingSpeed = PlayerPrefs.GetFloat("verRisSpeed");
            float verticalFallingSpeed = PlayerPrefs.GetFloat("verFalSpeed");
            float droneSize = PlayerPrefs.GetFloat("droneSize");
            
            choreographyData = new Choreography(name, horizontalSpeed, verticalRisingSpeed, verticalFallingSpeed, separation, droneSize);
        }
        else {
            LoadChoreographyData();
            DroneGameManager.Instance.LoadData(choreographyData);
        }
        
        DroneGameManager.Instance.SetChoreographyName(choreographyData.Name);
    }

    private void SetDelegates() {
        GameEventsManager.SaveInitiated += SaveChoreographyData;
        GameEventsManager.TimeToggleInitiated += ToggleTime;
        GameEventsManager.ChoreographyUpdateInitiated += UpdateChoreography;
    }
    
    private bool CheckChoreographyMode() {
        // new - return true
        // load - return false
        return PlayerPrefs.GetString("mode").Equals("new");
    }

    private void LoadChoreographyData() {
        string choreography = PlayerPrefs.GetString("name");
        choreographyData = DataManager.Load<Choreography>(choreography, "choreography data");
    }

    private void SaveChoreographyData() {
        DataManager.Save(choreographyData, choreographyData.Name, "choreography data");
    }

    private void UpdateChoreography(Choreography choreography) {
        choreographyData.Separation = choreography.Separation;
        choreographyData.DroneSize = choreography.DroneSize;
        choreographyData.HorizontalSpeed = choreography.HorizontalSpeed;
        choreographyData.VerticalFallingSpeed = choreography.VerticalFallingSpeed;
        choreographyData.VerticalRisingSpeed = choreography.VerticalRisingSpeed;
    }

    public void ToggleTime() {
        isRunning = !isRunning;
        DroneGameManager.Instance.DronesManager.ToggleDroneTime(isRunning);
    }

    public Choreography ChoreographyData {
        get => choreographyData;
        set => choreographyData = value;
    }

    public bool IsRunning => isRunning;
}
