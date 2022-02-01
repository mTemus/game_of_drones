using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Step {
    [NonSerialized] private Vector3 distance;
    
    private List<float> distanceList;
    private Dictionary<string, float> extraVariables;
    
    private float speed;
    private MoveMachine state;

    public Step(MoveMachine state, List<float> distanceList, float speed) {
        this.state = state;
        this.distanceList = distanceList;
        this.speed = speed;

        distance = new Vector3(distanceList[0], distanceList[1], distanceList[2]);
        extraVariables = new Dictionary<string, float>();
    }

    public void UpdateStepAfterLoad() {
        distance = new Vector3(distanceList[0], distanceList[1], distanceList[2]);
    }

    public void SetAdditionalVariable(string name, float value) {
        extraVariables[name] = value;
    }

    public Dictionary<string, float> ExtraVariables => extraVariables;

    public MoveMachine State => state;

    public Vector3 Distance => distance;

    public float Speed {
        get => speed;
        set => speed = value;
    }
}
