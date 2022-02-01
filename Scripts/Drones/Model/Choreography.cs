using System;

[Serializable]
public class Choreography {
    private string name;
    private int droneAmount;
    private float horizontalSpeed = 4f;
    private float verticalRisingSpeed = 3f;
    private float verticalFallingSpeed = 1f;
    private float separation = 5f;
    private float droneSize = 1f;

    public Choreography() {
        
    }
    
    public Choreography(string name, int droneAmount) {
        this.name = name;
        this.droneAmount = droneAmount;
    }

    public Choreography(Choreography choreography) {
        name = choreography.name;
        droneAmount = choreography.droneAmount;
        horizontalSpeed = choreography.horizontalSpeed;
        verticalRisingSpeed = choreography.verticalRisingSpeed;
        verticalFallingSpeed = choreography.verticalFallingSpeed;
        separation = choreography.separation;
        droneSize = choreography.droneSize;
    }
    
    public Choreography(string name, float horizontalSpeed, float verticalRisingSpeed, float verticalFallingSpeed, float separation, float droneSize) {
        this.name = name;
        this.horizontalSpeed = horizontalSpeed;
        this.verticalRisingSpeed = verticalRisingSpeed;
        this.verticalFallingSpeed = verticalFallingSpeed;
        this.separation = separation;
        this.droneSize = droneSize;
    }

    public string Name {
        get => name;
        set => name = value;
    }

    public int DroneAmount {
        get => droneAmount;
        set => droneAmount = value;
    }

    public float HorizontalSpeed {
        get => horizontalSpeed;
        set => horizontalSpeed = value;
    }

    public float VerticalRisingSpeed {
        get => verticalRisingSpeed;
        set => verticalRisingSpeed = value;
    }

    public float VerticalFallingSpeed {
        get => verticalFallingSpeed;
        set => verticalFallingSpeed = value;
    }

    public float Separation {
        get => separation;
        set => separation = value;
    }

    public float DroneSize {
        get => droneSize;
        set => droneSize = value;
    }
}
