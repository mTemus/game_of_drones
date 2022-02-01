using System;
using UnityEngine;
using UnityEngine.UI;

public class MoveByCoordsController : MonoBehaviour {
    
    [Header("Inputs")] 
    [SerializeField] private InputField xInput = null;
    [SerializeField] private InputField yInput = null;
    [SerializeField] private InputField zInput = null;

    public void MoveDroneToCoordinates() {
        string xTmp = xInput.text;
        string yTmp = yInput.text;
        string zTmp = zInput.text;

        if (xTmp.Contains(".")) xTmp = xTmp.Replace(".", ",");
        if (yTmp.Contains(".")) yTmp = yTmp.Replace(".", ",");    
        if (zTmp.Contains(".")) zTmp = zTmp.Replace(".", ",");
        
        float x = float.Parse(xTmp);
        float y = float.Parse(yTmp);
        float z = float.Parse(zTmp);
        Vector3 newPosition = new Vector3(x, y, z);
        
        DroneGameManager.Instance.MoveDroneToCoordinates(newPosition);
        DroneGameManager.Instance.ShowUpdatedDroneCoordinates();
    }

    private void OnEnable() {
        GameObject drone = DroneGameManager.Instance.DronesManager.SelectedDrone;
        Vector3 dronePosition = drone.transform.localPosition;
        xInput.text = dronePosition.x.ToString();
        yInput.text = dronePosition.y.ToString();
        zInput.text = dronePosition.z.ToString();
    }
}
