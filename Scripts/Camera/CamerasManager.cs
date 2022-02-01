using System.Collections.Generic;
using UnityEngine;

public class CamerasManager : MonoBehaviour {
    private Camera mainCamera;


    private void Awake() {
        mainCamera = Camera.main;
        SetDelegates();
    }

    private void SetDelegates() {
        GameEventsManager.SaveInitiated += SaveMainCameraData;
        GameEventsManager.LoadInitiated += LoadMainCameraData;
    }

    private void SaveMainCameraData() {
        string choreography = DroneGameManager.Instance.ChoreographyManager.ChoreographyData.Name;
        
        List<float> cameraPosition = new List<float>();
        var position = mainCamera.transform.position;
        cameraPosition.Add(position.x);
        cameraPosition.Add(position.y);
        cameraPosition.Add(position.z);

        List<float> cameraRotation = new List<float>();
        var rotation = mainCamera.transform.rotation;
        cameraRotation.Add(rotation.w);
        cameraRotation.Add(rotation.x);
        cameraRotation.Add(rotation.y);
        cameraRotation.Add(rotation.z);

        DataManager.Save(cameraPosition, choreography, "mainCamPos");
        DataManager.Save(cameraRotation, choreography, "mainCamRot");
    }

    private void LoadMainCameraData(Choreography choreography) {
        string chorName = choreography.Name;
        List<float> camPos = DataManager.Load<List<float>>(chorName, "mainCamPos");
        List<float> camRot = DataManager.Load<List<float>>(chorName, "mainCamRot");
        
        Vector3 newPosition = new Vector3(camPos[0], camPos[1], camPos[2]);
        Quaternion newRotation = new Quaternion(camRot[1], camRot[2], camRot[3], camRot[0]);
        
        mainCamera.transform.SetPositionAndRotation(newPosition, newRotation);
    }
    
}
