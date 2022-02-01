using System;
using System.Collections.Generic;
using UnityEngine;

public class FlyingController : MonoBehaviour {
    
    private Queue<Step> choreographySteps;
    private Queue<Step> choreographyInProgress;
    
    private bool started;
    private Vector3 destinationPosition;

    private Step currentStep;
    private bool isMoving;
    private bool choreographyIsRunning;

    private Vector3 circularMotion;
    private Vector3 startingPosition;
    private Vector3 endingPosition;
    private Vector3 centerPoint;
    private Vector3 startRelCenter;
    private Vector3 endRelCenter;
    private Vector3 localPositionForSlerp;
    

    private float saveCoordinatesCounter = 4f;
    private float coordinatesTimer;
    private float fractionComplete;
    private float fraction;
    private float journeyTime;

    private float totalDistance;
    
    
    private StepsToExport myExport;
    private DroneController myDC;
    
    
    private void Awake() {
        choreographySteps = new Queue<Step>();
        choreographyInProgress = new Queue<Step>();
        isMoving = false;
        choreographyIsRunning = false;
        myDC = transform.GetComponent<DroneController>();
    }

    private void Update() {
        if (myExport == null) {
            myExport = new StepsToExport(transform.name, DroneGameManager.Instance.ChoreographyManager.ChoreographyData.Name);
        }

        if (choreographyIsRunning) {
            if (isMoving) {
                float dest = Vector3.Magnitude(destinationPosition);
                Move(currentStep, dest);

                coordinatesTimer += Time.deltaTime;
                
                if (coordinatesTimer >= saveCoordinatesCounter) {
                    myExport.AddCoordinates(transform.localPosition);
                    coordinatesTimer = 0;
                }

            } else if (choreographyInProgress.Count != 0) {
                if (!isMoving) {
                    ResetVariablesBeforeStart();
                    // Debug.LogWarning("Getting next choreography step.");
                    currentStep = GetNextStep();
                    Transform myTransform = transform;
                    destinationPosition = myTransform.position + currentStep.Distance;
                    SetStepProperties(currentStep);
                    isMoving = true;
                }
            } 
            //TODO: Delete this on release
            else if (choreographyInProgress.Count == 0) {
                // Debug.LogError("No steps in queue!");
            }
        }
    }
    
    // Separacja pozioma i pionowa: 5 m+szerokość drona, bo 2.5m to dokładność 1 GPSa - zrobione
    //     Prędkość pozioma: 4m/s,
    //     prędkość wznoszenia: 3m/s,
    //     prędkość opadania 1m/s
    // drony zamieniąjące się miejscami (ruch po okręgu)
    // drony latające w kółko (ruch po okręgu)
    
    private void Move(Step step, float destPosition) {
        if (step.State == MoveMachine.Start || started) { started = true; }
        else { return; }

        Transform myTransform = transform;
        Vector3 currentPosition = myTransform.position;
        Vector3 currentLocalPosition = myTransform.localPosition;
        
        switch (step.State) {
            case MoveMachine.Start:
                // Debug.Log(step.State.ToString());
                myExport.AddCoordinates(transform.localPosition);
                isMoving = false;
                break;
            
            case MoveMachine.Stop:
                started = false;
                // Debug.LogError(transform.name + " has stopped.");
                DroneGameManager.Instance.DataManager.SaveToCSV(myExport);
                isMoving = false;
                break;
            
            
            // Vertical
            
            case MoveMachine.FlyVerticallyUp:
                if (currentPosition.y < destinationPosition.y) CalculateNewLerpPosition(step.Speed);
                else EndLinearMotion();
                break;
            
            case MoveMachine.FlyVerticallyDown:
                if (currentPosition.y > destinationPosition.y) CalculateNewLerpPosition(step.Speed);
                else EndLinearMotion();
                break;
            
            case MoveMachine.FlyVerticallyToXPlusOnYPlus:
                if (currentPosition.y < destinationPosition.y) CalculateNewLerpPosition(step.Speed);
                else EndLinearMotion();
                break;
            
            case MoveMachine.FlyVerticallyToXMinusOnYPlus:
                if (currentPosition.y < destinationPosition.y) CalculateNewLerpPosition(step.Speed);
                else EndLinearMotion();
                break;
            
            case MoveMachine.FlyVerticallyToZPlusOnYPlus:
                if (currentPosition.y < destinationPosition.y) CalculateNewLerpPosition(step.Speed);
                else EndLinearMotion();
                break;
            
            case MoveMachine.FlyVerticallyToZMinusOnYPlus:
                if (currentPosition.y < destinationPosition.y) CalculateNewLerpPosition(step.Speed);
                else EndLinearMotion();
                break;
            
            case MoveMachine.FlyVerticallyToXPlusZPlusOnYPlus:
                if (currentPosition.y < destinationPosition.y) CalculateNewLerpPosition(step.Speed);
                else EndLinearMotion();
                break;
            
            case MoveMachine.FlyVerticallyToXPlusZMinusOnYPlus:
                if (currentPosition.y < destinationPosition.y) CalculateNewLerpPosition(step.Speed);
                else EndLinearMotion();
                break;
            
            case MoveMachine.FlyVerticallyToXMinusZPlusOnYPlus:
                if (currentPosition.y < destinationPosition.y) CalculateNewLerpPosition(step.Speed);
                else EndLinearMotion();
                break;
                
            case MoveMachine.FlyVerticallyToXMinusZMinusOnYPlus:
                if (currentPosition.y < destinationPosition.y) CalculateNewLerpPosition(step.Speed);
                else EndLinearMotion();
                break; 
                
            case MoveMachine.FlyVerticallyToXPlusOnYMinus:
                if (currentPosition.y > destinationPosition.y) CalculateNewLerpPosition(step.Speed);
                else EndLinearMotion();
                break;
                
            case MoveMachine.FlyVerticallyToXMinusOnYMinus:
                if (currentPosition.y > destinationPosition.y) CalculateNewLerpPosition(step.Speed);
                else EndLinearMotion();
                break;
            
            case MoveMachine.FlyVerticallyToZPlusOnYMinus:
                if (currentPosition.y > destinationPosition.y) CalculateNewLerpPosition(step.Speed);
                else EndLinearMotion();
                break;
            
            case MoveMachine.FlyVerticallyToZMinusOnYMinus:
                if (currentPosition.y > destinationPosition.y) CalculateNewLerpPosition(step.Speed);
                else EndLinearMotion();
                break;
            
            case MoveMachine.FlyVerticallyToXPlusZPlusOnYMinus:
                if (currentPosition.y > destinationPosition.y) CalculateNewLerpPosition(step.Speed);
                else EndLinearMotion();
                break;
            
            case MoveMachine.FlyVerticallyToXPlusZMinusOnYMinus:
                if (currentPosition.y > destinationPosition.y) CalculateNewLerpPosition(step.Speed);
                else EndLinearMotion();
                break;
            
            case MoveMachine.FlyVerticallyToXMinusZPlusOnYMinus:
                if (currentPosition.y > destinationPosition.y) CalculateNewLerpPosition(step.Speed);
                else EndLinearMotion();
                break;
            
            case MoveMachine.FlyVerticallyToXMinusZMinusOnYMinus:
                if (currentPosition.y > destinationPosition.y) CalculateNewLerpPosition(step.Speed);
                else EndLinearMotion();
                break;
            
            // Horizontal

            case MoveMachine.FlyHorizontallyToXPlusOnYZero:
                if (currentPosition.x < destinationPosition.x) CalculateNewLerpPosition(step.Speed); 
                else EndLinearMotion();
                break;
            
            case MoveMachine.FlyHorizontallyToXMinusOnYZero:
                if (currentPosition.x > destinationPosition.x) CalculateNewLerpPosition(step.Speed);
                else EndLinearMotion();
                break;
            
            case MoveMachine.FlyHorizontallyToZPlusOnYZero:
                if (currentPosition.z < destinationPosition.z) CalculateNewLerpPosition(step.Speed); 
                else EndLinearMotion();
                break;

            case MoveMachine.FlyHorizontallyToZMinusOnYZero:
                if (currentPosition.z > destinationPosition.z) CalculateNewLerpPosition(step.Speed);
                else EndLinearMotion();
                break;
            
            case MoveMachine.FlyHorizontallyToXPlusZPlusOnYZero:
                if (currentPosition.x < destinationPosition.x) CalculateNewLerpPosition(step.Speed);
                else EndLinearMotion();
                break;
            
            case MoveMachine.FlyHorizontallyToXPlusZMinusOnYZero:
                if (currentPosition.x < destinationPosition.x) CalculateNewLerpPosition(step.Speed);
                else EndLinearMotion();
                break;
            
            case MoveMachine.FlyHorizontallyToXMinusZPlusOnYZero:
                if (currentPosition.x > destinationPosition.x) CalculateNewLerpPosition(step.Speed);
                else EndLinearMotion();
                break;
            
            case MoveMachine.FlyHorizontallyToXMinusZMinusOnYZero:
                if (currentPosition.x > destinationPosition.x) CalculateNewLerpPosition(step.Speed);
                else EndLinearMotion();
                break;
                
            // Other
            
            case MoveMachine.LightOn:
                myDC.ToggleLight();
                isMoving = false;
                break;
            
            case MoveMachine.LightOff:
                myDC.ToggleLight();
                isMoving = false;
                break;
            
            // Half circles
            
            case MoveMachine.HalfCircleToXPlusOnZMinus:
                GetCenterForSlerp(Vector3.right);
                CalculateNewSlerpPosition(step.Speed);

                if (currentLocalPosition.x >= endingPosition.x) EndCircleMotion();
                break;
            
            case MoveMachine.HalfCircleToXPlusOnZPlus:
                GetCenterForSlerp(Vector3.forward);
                CalculateNewSlerpPosition(step.Speed);
                
                if (currentLocalPosition.x >= endingPosition.x) EndCircleMotion();
                break;
            
            case MoveMachine.HalfCircleToXMinusOnZMinus:
                GetCenterForSlerp(Vector3.right);
                CalculateNewSlerpPosition(step.Speed);
                
                if (currentLocalPosition.x <= endingPosition.x) EndCircleMotion();
                break;
            
            case MoveMachine.HalfCircleToXMinusOnZPlus:
                GetCenterForSlerp(Vector3.forward);
                CalculateNewSlerpPosition(step.Speed);
                
                if (transform.localPosition.x <= endingPosition.x) EndCircleMotion();
                break;
            
            case MoveMachine.HalfCircleToYPlusOnXMinus:
                GetCenterForSlerp(Vector3.left);
                CalculateNewSlerpPosition(step.Speed);
                
                if (transform.localPosition.y >= endingPosition.y) EndCircleMotion();
                break;

            case MoveMachine.HalfCircleToYPlusOnxPlus:
                GetCenterForSlerp(Vector3.right);
                CalculateNewSlerpPosition(step.Speed);
                
                if (transform.localPosition.y >= endingPosition.y) EndCircleMotion();
                break;
                
            case MoveMachine.HalfCircleToYMinusOnXMinus:
                GetCenterForSlerp(Vector3.left);
                CalculateNewSlerpPosition(step.Speed);
                if (transform.localPosition.y <= endingPosition.y) EndCircleMotion();
                break;
                
            case MoveMachine.HalfCircleToYMinusOnXPlus:
                GetCenterForSlerp(Vector3.right);
                CalculateNewSlerpPosition(step.Speed);
                if (transform.localPosition.y <= endingPosition.y) EndCircleMotion();
                break;
            
            case MoveMachine.HalfCircleToZPlusOnXMinus:
                GetCenterForSlerp(Vector3.left);
                CalculateNewSlerpPosition(step.Speed);
                if (transform.localPosition.z >= endingPosition.z) EndCircleMotion();
                break;
            
            case MoveMachine.HalfCircleToZPlusOnXPlus:
                GetCenterForSlerp(Vector3.right);
                CalculateNewSlerpPosition(step.Speed);
                if (transform.localPosition.z >= endingPosition.z) EndCircleMotion();
                break;

            case MoveMachine.HalfCircleToZMinusOnXMinus:
                GetCenterForSlerp(Vector3.left);
                CalculateNewSlerpPosition(step.Speed);
                if (transform.localPosition.z <= endingPosition.z) EndCircleMotion();
                break;
            
            case MoveMachine.HalfCircleToZMinusOnXPlus:
                GetCenterForSlerp(Vector3.right);
                CalculateNewSlerpPosition(step.Speed);
                if (transform.localPosition.z <= endingPosition.z) EndCircleMotion();
                break;
            
            // default:
            //     Debug.LogError("Drone moving state error. No such state for moving.");
            //     break;
        }
    }
    
    private void SetStepProperties(Step step) {
        switch (step.State) {
            
            case MoveMachine.FlyVerticallyUp:
            case MoveMachine.FlyVerticallyToXPlusOnYPlus:
            case MoveMachine.FlyVerticallyToXMinusOnYPlus:
            case MoveMachine.FlyVerticallyToZPlusOnYPlus:
            case MoveMachine.FlyVerticallyToZMinusOnYPlus:
            case MoveMachine.FlyVerticallyToXPlusZPlusOnYPlus:
            case MoveMachine.FlyVerticallyToXPlusZMinusOnYPlus:
            case MoveMachine.FlyVerticallyToXMinusZPlusOnYPlus:
            case MoveMachine.FlyVerticallyToXMinusZMinusOnYPlus:
            case MoveMachine.FlyVerticallyDown:
            case MoveMachine.FlyVerticallyToXPlusOnYMinus:
            case MoveMachine.FlyVerticallyToXMinusOnYMinus:
            case MoveMachine.FlyVerticallyToZPlusOnYMinus:
            case MoveMachine.FlyVerticallyToZMinusOnYMinus:
            case MoveMachine.FlyVerticallyToXPlusZPlusOnYMinus:
            case MoveMachine.FlyVerticallyToXPlusZMinusOnYMinus:
            case MoveMachine.FlyVerticallyToXMinusZPlusOnYMinus:
            case MoveMachine.FlyVerticallyToXMinusZMinusOnYMinus:
            case MoveMachine.FlyHorizontallyToXPlusOnYZero:
            case MoveMachine.FlyHorizontallyToXMinusOnYZero:
            case MoveMachine.FlyHorizontallyToZPlusOnYZero:
            case MoveMachine.FlyHorizontallyToZMinusOnYZero:
            case MoveMachine.FlyHorizontallyToXPlusZPlusOnYZero:
            case MoveMachine.FlyHorizontallyToXPlusZMinusOnYZero:
            case MoveMachine.FlyHorizontallyToXMinusZPlusOnYZero:
            case MoveMachine.FlyHorizontallyToXMinusZMinusOnYZero:
                startingPosition = transform.localPosition;
                endingPosition = startingPosition + step.Distance;
                totalDistance = Vector3.Distance(startingPosition, endingPosition);
                break;

            case MoveMachine.HalfCircleToXPlusOnZMinus:
            case MoveMachine.HalfCircleToXPlusOnZPlus:
                startingPosition = transform.localPosition;
                endingPosition = new Vector3(startingPosition.x + step.Distance.x, startingPosition.y, startingPosition.z);
                journeyTime = step.ExtraVariables["time"];
                break;
            
            case MoveMachine.HalfCircleToXMinusOnZMinus:
            case MoveMachine.HalfCircleToXMinusOnZPlus:
                startingPosition = transform.localPosition;
                endingPosition = new Vector3(startingPosition.x - step.Distance.x, startingPosition.y, startingPosition.z);
                journeyTime = step.ExtraVariables["time"];
                break;
            
            case MoveMachine.HalfCircleToYPlusOnXMinus:
            case MoveMachine.HalfCircleToYPlusOnxPlus:
                startingPosition = transform.localPosition;
                endingPosition = new Vector3(startingPosition.x, startingPosition.y + step.Distance.y, startingPosition.z);
                journeyTime = step.ExtraVariables["time"];
                break;
            
            case MoveMachine.HalfCircleToYMinusOnXMinus:
            case MoveMachine.HalfCircleToYMinusOnXPlus:
                startingPosition = transform.localPosition;
                endingPosition = new Vector3(startingPosition.x, startingPosition.y - step.Distance.y, startingPosition.z);
                journeyTime = step.ExtraVariables["time"];
                break;
                
            case MoveMachine.HalfCircleToZPlusOnXMinus:
            case MoveMachine.HalfCircleToZPlusOnXPlus:
                startingPosition = transform.localPosition;
                endingPosition = new Vector3(startingPosition.x, startingPosition.y , startingPosition.z + step.Distance.z);
                journeyTime = step.ExtraVariables["time"];
                break;
            
            case MoveMachine.HalfCircleToZMinusOnXMinus:
            case MoveMachine.HalfCircleToZMinusOnXPlus:
                startingPosition = transform.localPosition;
                endingPosition = new Vector3(startingPosition.x, startingPosition.y , startingPosition.z - step.Distance.z);
                journeyTime = step.ExtraVariables["time"];
                break;
        }
    }
    

    private void CalculateNewLerpPosition(float speed) {
        fraction += Time.deltaTime;
        float journeyFraction = (fraction * speed) / totalDistance;
        transform.localPosition = Vector3.Lerp(startingPosition, endingPosition, journeyFraction);
    }

    private void CalculateNewSlerpPosition(float speed) {
        fraction += Time.deltaTime;
        fractionComplete = fraction / journeyTime * speed;
        localPositionForSlerp = Vector3.Slerp(startRelCenter, endRelCenter, fractionComplete * speed);
        localPositionForSlerp += centerPoint;
        transform.localPosition = localPositionForSlerp;
    }
    
    private void GetCenterForSlerp(Vector3 direction) {
        centerPoint = (startingPosition + endingPosition) * 0.5f;
        centerPoint -= (direction * 0.2f);
        startRelCenter = startingPosition - centerPoint;
        endRelCenter = endingPosition - centerPoint;
    }

    private void EndLinearMotion() {
        startingPosition = Vector3.zero;
        endingPosition = Vector3.zero;
        fraction = 0;
        totalDistance = 0;
        isMoving = false;
    }
    
    private void EndCircleMotion() {
        transform.localPosition = endingPosition;
        
        fraction = 0;
        fractionComplete = 0;
        startRelCenter = Vector3.zero;
        endRelCenter = Vector3.zero;
        centerPoint = Vector3.zero;
        localPositionForSlerp = Vector3.zero;
        isMoving = false;
    }

    private void ResetVariablesBeforeStart() {
        fraction = 0;
        fractionComplete = 0;
        totalDistance = 0;
        startRelCenter = Vector3.zero;
        endRelCenter = Vector3.zero;
        centerPoint = Vector3.zero;
        localPositionForSlerp = Vector3.zero;
        startingPosition = Vector3.zero;
        endingPosition = Vector3.zero;
    }

    public void SetChoreography() {
        if (choreographySteps.Count == 0) return;
        
        choreographyInProgress = new Queue<Step>(choreographySteps);
        currentStep = GetNextStep();
    }
    
    public void UpdateMove() {
        List<Step> currentSteps = new List<Step>(choreographySteps);

        foreach (Step step in currentSteps) {
            step.UpdateStepAfterLoad();
        }
    }
    
    public void ClearExport() {
        myExport = null;
    }

    public void CreateStep(Step step) {
        choreographySteps.Enqueue(step);
    }

    public Step GetNextStep() {
        isMoving = true;
        return choreographyInProgress.Dequeue();
    }

    public bool IsMoving => isMoving;

    public bool ChoreographyIsRunning {
        get => choreographyIsRunning;
        set => choreographyIsRunning = value;
    }

    public Queue<Step> ChoreographySteps {
        get => choreographySteps;
        set => choreographySteps = value;
    }

    public float SaveCoordinatesCounter {
        get => saveCoordinatesCounter;
        set => saveCoordinatesCounter = value;
    }
}