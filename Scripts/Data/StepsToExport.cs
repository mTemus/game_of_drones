using System.Collections.Generic;
using UnityEngine;

public class StepsToExport {
    private List<Vector3> droneCoordinates;
    private string droneName;
    private string choreographyName;

    public StepsToExport(string droneName, string choreographyName) {
        this.droneName = droneName;
        this.choreographyName = choreographyName;
        droneCoordinates = new List<Vector3>();
    }

    public void AddCoordinates(Vector3 coordinates) {
        droneCoordinates.Add(coordinates);
    }

    public List<Vector3> DroneCoordinates => droneCoordinates;
    
    public string DroneName => droneName;
    
    public string ChoreographyName => choreographyName;
}
