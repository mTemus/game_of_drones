using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepsCreator : MonoBehaviour {
    [Header("Inputs")]
    [SerializeField] private InputField xInput = null;
    [SerializeField] private InputField yInput = null;
    [SerializeField] private InputField zInput = null;

    [SerializeField] private InputField[] extraInputs = null;
    
    public Step CreateStep() {
        Step step;
        List<float> moveList;
        MoveMachine moveStep = (MoveMachine) Enum.Parse(typeof(MoveMachine), transform.name);

        float x;
        float y;
        float z;

        float time;
        float radius;

        float speed;
        
        switch (moveStep) {
            case MoveMachine.Start:
            case MoveMachine.LightOn:
            case MoveMachine.LightOff:
            case MoveMachine.Stop:
                moveList = new List<float> {0,0,0};
                step = new Step(moveStep, moveList, 0);
                break;
            
            case MoveMachine.FlyVerticallyUp:
                y = ParseInput(yInput);
                speed = GetSpeed(y);
                moveList = new List<float> {0f, y, 0f};
                step = new Step(moveStep, moveList, speed);
                break;
            
            case MoveMachine.FlyVerticallyDown: 
                y = -ParseInput(yInput);
                speed = GetSpeed(y);
                moveList = new List<float> {0f, y, 0f};
                step = new Step(moveStep, moveList, speed);
                break;

            case MoveMachine.FlyVerticallyToXPlusOnYPlus:
                x = ParseInput(xInput);
                y = ParseInput(yInput);
                speed = GetSpeed(y);
                moveList = new List<float> {x, y, 0f};
                step = new Step(moveStep, moveList, speed);
                break;
            
            case MoveMachine.FlyVerticallyToXMinusOnYPlus:
                x = -ParseInput(xInput);
                y = ParseInput(yInput);
                speed = GetSpeed(y);
                moveList = new List<float> {x, y, 0f};
                step = new Step(moveStep, moveList, speed);
                break;
            
            case MoveMachine.FlyVerticallyToZPlusOnYPlus:
                z = ParseInput(zInput);
                y = ParseInput(yInput);
                speed = GetSpeed(y);
                moveList = new List<float> {0f, y, z};
                step = new Step(moveStep, moveList, speed);
                break;
            
            case MoveMachine.FlyVerticallyToZMinusOnYPlus:
                z = -ParseInput(zInput);
                y = ParseInput(yInput);
                speed = GetSpeed(y);
                moveList = new List<float> {0f, y, z};
                step = new Step(moveStep, moveList, speed);
                break;
                
            case MoveMachine.FlyVerticallyToXPlusZPlusOnYPlus:
                x = ParseInput(xInput);
                y = ParseInput(yInput);
                z = ParseInput(zInput);
                speed = GetSpeed(y);
                moveList = new List<float> {x, y, z};
                step = new Step(moveStep, moveList, speed);
                break;
                
            case MoveMachine.FlyVerticallyToXPlusZMinusOnYPlus:
                x = ParseInput(xInput);
                y = ParseInput(yInput);
                z = -ParseInput(zInput);
                speed = GetSpeed(y);
                moveList = new List<float> {x, y, z};
                step = new Step(moveStep, moveList, speed);
                break;
            
            case MoveMachine.FlyVerticallyToXMinusZPlusOnYPlus:
                x = -ParseInput(xInput);
                y = ParseInput(yInput);
                z = ParseInput(zInput);
                speed = GetSpeed(y);
                moveList = new List<float> {x, y, z};
                step = new Step(moveStep, moveList, speed);
                break;
            
            case MoveMachine.FlyVerticallyToXMinusZMinusOnYPlus:
                x = -ParseInput(xInput);
                y = ParseInput(yInput);
                z = -ParseInput(zInput);
                speed = GetSpeed(y);
                moveList = new List<float> {x, y, z};
                step = new Step(moveStep, moveList, speed);
                break;
                
            case MoveMachine.FlyVerticallyToXPlusOnYMinus:
                x = ParseInput(xInput);
                y = -ParseInput(yInput);
                speed = GetSpeed(y);
                moveList = new List<float> {x, y, 0f};
                step = new Step(moveStep, moveList, speed);
                break;
            
            case MoveMachine.FlyVerticallyToXMinusOnYMinus:
                x = -ParseInput(xInput);
                y = -ParseInput(yInput);
                speed = GetSpeed(y);
                moveList = new List<float> {x, y, 0f};
                step = new Step(moveStep, moveList, speed);
                break;
            
            case MoveMachine.FlyVerticallyToZPlusOnYMinus:
                z = ParseInput(zInput);
                y = -ParseInput(yInput);
                speed = GetSpeed(y);
                moveList = new List<float> {0f, y, z};
                step = new Step(moveStep, moveList, speed);
                break;
            
            case MoveMachine.FlyVerticallyToZMinusOnYMinus:
                z = -ParseInput(zInput);
                y = -ParseInput(yInput);
                speed = GetSpeed(y);
                moveList = new List<float> {0f, y, z};
                step = new Step(moveStep, moveList, speed);
                break;
                
            case MoveMachine.FlyVerticallyToXPlusZPlusOnYMinus:
                x = ParseInput(xInput);
                y = -ParseInput(yInput);
                z = ParseInput(zInput);
                speed = GetSpeed(y);
                moveList = new List<float> {x, y, z};
                step = new Step(moveStep, moveList, speed);
                break;
            
            case MoveMachine.FlyVerticallyToXPlusZMinusOnYMinus:
                x = ParseInput(xInput);
                y = -ParseInput(yInput);
                z = -ParseInput(zInput);
                speed = GetSpeed(y);
                moveList = new List<float> {x, y, z};
                step = new Step(moveStep, moveList, speed);
                break;
            
            case MoveMachine.FlyVerticallyToXMinusZPlusOnYMinus:
                x = -ParseInput(xInput);
                y = -ParseInput(yInput);
                z = ParseInput(zInput);
                speed = GetSpeed(y);
                moveList = new List<float> {x, y, z};
                step = new Step(moveStep, moveList, speed);
                break;
            
            case MoveMachine.FlyVerticallyToXMinusZMinusOnYMinus:
                x = -ParseInput(xInput);
                y = -ParseInput(yInput);
                z = -ParseInput(zInput);
                speed = GetSpeed(y);
                moveList = new List<float> {x, y, z};
                step = new Step(moveStep, moveList, speed);
                break;
            
            
            
            

            case MoveMachine.FlyHorizontallyToXPlusOnYZero:
                x = ParseInput(xInput);
                speed = GetSpeed(0);
                moveList = new List<float> {x, 0f, 0f};
                step = new Step(moveStep, moveList, speed);
                break;
            
            case MoveMachine.FlyHorizontallyToXMinusOnYZero:
                x = -ParseInput(xInput);
                speed = GetSpeed(0);
                moveList = new List<float> {x, 0f, 0f};
                step = new Step(moveStep, moveList, speed);
                break;
            
            case MoveMachine.FlyHorizontallyToZPlusOnYZero:
                z = ParseInput(zInput);
                speed = GetSpeed(0);
                moveList = new List<float> {0f, 0f, z};
                step = new Step(moveStep, moveList, speed);
                break;
            
            case MoveMachine.FlyHorizontallyToZMinusOnYZero:
                z = -ParseInput(zInput);
                speed = GetSpeed(0);
                moveList = new List<float> {0f, 0f, z};
                step = new Step(moveStep, moveList, speed);
                break;
            
            case MoveMachine.FlyHorizontallyToXPlusZPlusOnYZero:
                x = ParseInput(xInput);
                z = ParseInput(zInput);
                speed = GetSpeed(0);
                moveList = new List<float>{x, 0f, z};
                step = new Step(moveStep, moveList, speed);
                break;
            
            case MoveMachine.FlyHorizontallyToXPlusZMinusOnYZero:
                x = ParseInput(xInput);
                z = -ParseInput(zInput);
                speed = GetSpeed(0);
                moveList = new List<float>{x, 0f, z};
                step = new Step(moveStep, moveList, speed);
                break;
            
            case MoveMachine.FlyHorizontallyToXMinusZPlusOnYZero:
                x = -ParseInput(xInput);
                z = Math.Abs(float.Parse(zInput.text));
                speed = GetSpeed(0);
                moveList = new List<float>{x, 0f, z};
                step = new Step(moveStep, moveList, speed);
                break;
            
            case MoveMachine.FlyHorizontallyToXMinusZMinusOnYZero:
                x = -ParseInput(xInput);
                z = -Math.Abs(float.Parse(zInput.text));
                speed = GetSpeed(0);
                moveList = new List<float>{x, 0f, z};
                step = new Step(moveStep, moveList, speed);
                break;
            
            case MoveMachine.HalfCircleToXPlusOnZMinus:
            case MoveMachine.HalfCircleToXPlusOnZPlus:
            case MoveMachine.HalfCircleToXMinusOnZMinus:
            case MoveMachine.HalfCircleToXMinusOnZPlus:
                radius = ParseInput(extraInputs[0]);
                time = ParseInput(extraInputs[1]);
                x = radius;
                speed = 1f;
                moveList = new List<float>{x * 2, 0, 0};
                
                step = new Step(moveStep, moveList, speed);
                step.SetAdditionalVariable("radius", radius);
                step.SetAdditionalVariable("time", time);
                break;
                
            case MoveMachine.HalfCircleToYPlusOnXMinus:
            case MoveMachine.HalfCircleToYPlusOnxPlus:
            case MoveMachine.HalfCircleToYMinusOnXMinus:
            case MoveMachine.HalfCircleToYMinusOnXPlus:
                radius = ParseInput(extraInputs[0]);
                time = ParseInput(extraInputs[1]);
                y = radius;
                speed = 1f;
                moveList = new List<float>{0, y * 2, 0};
                
                step = new Step(moveStep, moveList, speed);
                step.SetAdditionalVariable("radius", radius);
                step.SetAdditionalVariable("time", time);
                break;
                
            case MoveMachine.HalfCircleToZPlusOnXMinus:
            case MoveMachine.HalfCircleToZPlusOnXPlus:
            case MoveMachine.HalfCircleToZMinusOnXMinus:
            case MoveMachine.HalfCircleToZMinusOnXPlus:
                radius = ParseInput(extraInputs[0]);
                time = ParseInput(extraInputs[1]);
                z = radius;
                speed = 1f;
                moveList = new List<float>{0, 0, z * 2};
                
                step = new Step(moveStep, moveList, speed);
                step.SetAdditionalVariable("radius", radius);
                step.SetAdditionalVariable("time", time);
                break;
            
            
            
            
            default:
                throw new ArgumentOutOfRangeException();
        }

        return step;
    }

    private float ParseInput(InputField inputField) {
        string tmpString = inputField.text;
        if (tmpString.Contains(".")) tmpString = tmpString.Replace(".", ","); 
        
        return Math.Abs(float.Parse(tmpString));
    }

    private float GetSpeed(float y) {
        Choreography choreographyData = DroneGameManager.Instance.ChoreographyManager.ChoreographyData;
        if (y == 0) return choreographyData.HorizontalSpeed; 
        if (y > 0) return choreographyData.VerticalRisingSpeed; 
        if (y < 0) return choreographyData.VerticalFallingSpeed;

        return 1;
    }
    

    public InputField XInput {
        get => xInput;
        set => xInput = value;
    }

    public InputField YInput {
        get => yInput;
        set => yInput = value;
    }

    public InputField ZInput {
        get => zInput;
        set => zInput = value;
    }

    public InputField[] ExtraInputs => extraInputs;
}
