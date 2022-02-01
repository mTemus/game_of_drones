using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

    [Header("Cameras")] 
    [SerializeField] private Camera mainCamera = null;

    [Header("GUI Modes")]
    [SerializeField] private GameObject projectingMode = null;
    [SerializeField] private GameObject playingMode = null;
    
    [Header("GUI Right Panel")]
    [SerializeField] private GameObject pauseButton = null;
    [SerializeField] private GameObject dronePointButton = null;

    [Header("GUI Texts to set")]
    [SerializeField] private Text choreographyName = null;
    [SerializeField] private Text projectingModeText = null;
    
    [Header("GUI Icons to set")]
    [SerializeField] private Sprite pauseIcon = null;
    [SerializeField] private Sprite startIcon = null;
    [SerializeField] private Sprite dronePointCheck = null;
    [SerializeField] private Sprite dronePointUncheck = null;
    
    [Header("Other GUI parts")] 
    [SerializeField] private GameObject optionsWindow = null;
    [SerializeField] private GameObject keysWindow = null;
    [SerializeField] private GameObject choreographyGui = null;
    [SerializeField] private GameObject choreographyHeader = null;
    [SerializeField] private GameObject choreographyTimer = null;
    
    [Header("Alerts")] 
    [SerializeField] private GameObject savedAlert = null;
    [SerializeField] private GameObject saveBeforeExitAlert = null;
    [SerializeField] private GameObject separationErrorAlert = null;
    [SerializeField] private GameObject deleteReferencePointAlert = null;
    [SerializeField] private GameObject noDroneReferencePointAlert = null;
    [SerializeField] private GameObject separationColliderErrorAlert = null;
    [SerializeField] private GameObject emptyDroneQueueAlert = null;

    [Header("Projecting - Drone GUI parts")]
    [SerializeField] private Text droneName = null;
    [SerializeField] private GameObject droneStepsArea = null;
    [SerializeField] private GameObject droneStepsWindows = null;
    [SerializeField] private Text coordX = null;
    [SerializeField] private Text coordY = null;
    [SerializeField] private Text coordZ = null;

    [Header("Options - Choreography")] 
    [SerializeField] private GameObject optionsPanel = null;
    [SerializeField] private InputField separationText = null;
    [SerializeField] private InputField droneSizeText = null;
    [SerializeField] private InputField horizontalSpeedText = null;
    [SerializeField] private InputField verticalRisingSpeedText = null;
    [SerializeField] private InputField verticalFallingSpeedText = null;

    [Header("Other inputs")] 
    [SerializeField] private InputField timerInput = null;

    [Header("Other things")]
    [SerializeField] private Text currentQuality = null;
    [SerializeField] private Text separationErrorText = null;
    [SerializeField] private Text emptyDroneQueueText = null;
    
    [Header("Email panel")] 
    [SerializeField] private Input nameInput;

    private bool playingIsActive;
    private bool projectingWasActive;
    private bool playingWasActive;
    

    private void Awake() {
        DisableGUI();
        SetDelegates();
        UpdateModeText();
    }

    private void SetDelegates() {
        GUIEventsManager.ChoreographyNameInitiated += SetChoreographyName;
        GUIEventsManager.ModeChangeInitiated += SwitchMode;
        GUIEventsManager.SelectDroneInitiated += ShowDroneStepsWindows;
        GUIEventsManager.SelectDroneInitiated += ShowDroneCoordinates;
        GUIEventsManager.DeselectDroneInitiated += HideDroneStepsWindows;
        GUIEventsManager.DroneStepsUpdateInitiated += UpdateStepsWithVisuals;
        GUIEventsManager.ShowDroneStepsInitiated += ShowStepsVisualBlocks;
        GUIEventsManager.DeleteDroneStepsInitiated += DeleteStepsVisuals;
        GUIEventsManager.DeleteDroneReferencePointInitiated += ShowDeleteDroneReferencePointAlert;
        GUIEventsManager.NoDroneReferencePointInitiated += ShowNoDroneReferencePointAlert;
        GUIEventsManager.DroneCoordinatesUpdateInitiated += ShowDroneCoordinates;
        GUIEventsManager.DronePointSettingInitiated += ChangeDronePointButtonSprite;
    }

    private void UpdateModeText() {
        projectingModeText.text = projectingMode.activeSelf ? "Projektowanie" : "Odtwarzanie";
    }

    private void DisableGUI() {
        optionsWindow.SetActive(false);
        keysWindow.SetActive(false);
        playingMode.SetActive(false);
        savedAlert.SetActive(false);
        saveBeforeExitAlert.SetActive(false);
        droneStepsWindows.SetActive(false);
        separationErrorAlert.SetActive(false);
    }

    private void SwitchMode() {
        if (projectingMode.activeSelf) {
            if (!DroneGameManager.Instance.CheckDronesSeparationError()) {
                projectingMode.SetActive(false);
                playingMode.SetActive(true);
            
                if (DroneGameManager.Instance.DronesManager.IsSeparationActive) {
                    DroneGameManager.Instance.ToggleSeparation();
                }
            } else {
                separationErrorAlert.SetActive(true);
            }
        } else if (playingMode.activeSelf) {
            playingMode.SetActive(false);
            projectingMode.SetActive(true);
            
            if (!DroneGameManager.Instance.DronesManager.IsSeparationActive) {
                DroneGameManager.Instance.ToggleSeparation();
            }
        }

        playingIsActive = playingMode.activeSelf;
        UpdateModeText();
    }

    public void SeparationCollisionOccured(GameObject declarant, GameObject collider) {
        string warning = "Wystąpił problem z separacją. " + declarant.name + " zderzył się z " + collider.name + ". " +
                         "Po zamknięciu okna nastąpi powrót do trybu projektowania.";
        separationErrorText.text = warning;
        separationColliderErrorAlert.SetActive(true);
    }
    
    public void ToggleHUD() {
        if (playingMode.activeSelf || projectingMode.activeSelf) {
            if (playingMode.activeSelf) {
                playingWasActive = true;
                choreographyGui.SetActive(false);
                playingMode.SetActive(false);
            }
            else if (projectingMode.activeSelf) {
                projectingWasActive = true; 
                choreographyGui.SetActive(false);
                projectingMode.SetActive(false);
            }
        } else if (playingWasActive || projectingWasActive) {
            if (playingWasActive) {
                playingMode.SetActive(true);
                playingWasActive = false;
                if (!choreographyGui.activeSelf) { choreographyGui.SetActive(true); }
            } else if (projectingWasActive) {
                projectingMode.SetActive(true);
                projectingWasActive = false;
                if (!choreographyGui.activeSelf) { choreographyGui.SetActive(true); }
            }
        }
    }

    private void SetChoreographyName(string name) {
        choreographyName.text = name;
    }

    private void UpdateStepsWithVisuals() {
        Queue<Step> newDroneSteps = new Queue<Step>();
        int children = droneStepsArea.transform.childCount;

        if (children > 0) {
            for (int i = 0; i < children; i++) {
                Step s = droneStepsArea.transform.GetChild(i).GetComponent<StepsCreator>().CreateStep();
                newDroneSteps.Enqueue(s);
            }
        }

        DroneGameManager.Instance.SelectionManager.SelectedDrone.GetComponent<FlyingController>().ChoreographySteps =
            newDroneSteps;
    }

    private void ShowStepsVisualBlocks(GameObject drone) {
        Queue<Step> droneSteps = drone.GetComponent<FlyingController>().ChoreographySteps;
        List<Step> steps = new List<Step>(droneSteps);
        
        foreach (Step step in steps) {
            string name = step.State.ToString();
            GameObject stepInstance = Instantiate((GameObject)Resources.Load("Prefabs/Steps/" + name), droneStepsArea.transform, true);
            
            StepsCreator st = stepInstance.GetComponent<StepsCreator>();
            if (st.XInput != null) {
                float x = step.Distance.x;
                if (x < 0) { x = -x; }
                st.XInput.text = x.ToString();
            }

            if (st.YInput != null) {
                float y = step.Distance.y;
                if (y < 0) { y = -y; }
                st.YInput.text = y.ToString();
            }

            if (st.ZInput != null) {
                float z = step.Distance.z;
                if (z < 0) { z = -z; }
                st.ZInput.text = z.ToString();
            }

            SetExtraStepInputs(step, st);
            
            stepInstance.name = name;
            Destroy(stepInstance.GetComponent<StepsInstanceCreator>());
            
            RectTransform rt = droneStepsArea.GetComponent<RectTransform>();
            Vector2 sizeDelta = rt.sizeDelta;
            float height = sizeDelta.y + stepInstance.GetComponent<RectTransform>().sizeDelta.y + 20;
            float width = sizeDelta.x;
        
            Vector2 newSize = new Vector2(width, height);
            rt.sizeDelta = newSize;
        }
    }

    private void SetExtraStepInputs(Step step, StepsCreator st) {
        switch (step.State) {
            case MoveMachine.HalfCircleToXPlusOnZMinus:
            case MoveMachine.HalfCircleToXPlusOnZPlus:
            case MoveMachine.HalfCircleToXMinusOnZMinus:
            case MoveMachine.HalfCircleToXMinusOnZPlus:
            case MoveMachine.HalfCircleToYPlusOnXMinus:
            case MoveMachine.HalfCircleToYPlusOnxPlus:
            case MoveMachine.HalfCircleToYMinusOnXMinus:
            case MoveMachine.HalfCircleToYMinusOnXPlus:
            case MoveMachine.HalfCircleToZPlusOnXMinus:
            case MoveMachine.HalfCircleToZPlusOnXPlus:
            case MoveMachine.HalfCircleToZMinusOnXMinus:
            case MoveMachine.HalfCircleToZMinusOnXPlus:
                st.ExtraInputs[0].text = step.ExtraVariables["radius"].ToString();
                st.ExtraInputs[1].text = step.ExtraVariables["time"].ToString();
                break;
        }
    }

    private void DeleteStepsVisuals() {
        foreach (Transform child in droneStepsArea.transform) {
            Destroy(child.gameObject);
        }

        Vector2 sd = droneStepsArea.GetComponent<RectTransform>().sizeDelta;
        droneStepsArea.GetComponent<RectTransform>().sizeDelta = new Vector2(sd.x, 10);
    }

    private void ChangeDronePointButtonSprite() {
        bool pointChecked = DroneGameManager.Instance.DronesManager.DronePointChecked;

        dronePointButton.GetComponent<Image>().sprite = pointChecked ? dronePointCheck : dronePointUncheck;
    }

    private void SetQualityLevelText() {
        switch (QualitySettings.GetQualityLevel()) {
            case 0:
                currentQuality.text = "Bardzo niskie";
                break;
            case 1: 
                currentQuality.text = "Niskie";
                break;
            case 2: 
                currentQuality.text = "Średnie";
                break;
            case 3: 
                currentQuality.text = "Wysokie";
                break;
            case 4: 
                currentQuality.text = "Bardzo wysokie";
                break;
            case 5: 
                currentQuality.text = "Ultra";
                break;
        }
    }

    public void WarnEmptyDroneQueue(List<GameObject> emptyDrones) {
        string warning = "Wykryto, że drony: ";

        foreach (GameObject drone in emptyDrones) { warning += drone.name + ", "; }

        warning += "nie posiadają choreografii. Usuń drony albo stwórz dla nich choreografię i spróbuj ponownie.";
        emptyDroneQueueText.text = warning;
        emptyDroneQueueAlert.SetActive(true);
    }

    public void SetCoordinatesTimerDivider() {
        string timeString = timerInput.text;
        if (timeString.Contains(".")) { timeString = timeString.Replace(".", ","); }

        float time = Math.Abs(float.Parse(timeString));
        DroneGameManager.Instance.UpdateTimerDivider(time);
    }
    
    public void UpdateChoreography() {
        string separationString = separationText.text;
        string droneSizeString = droneSizeText.text;
        string horizontalSpeedString = horizontalSpeedText.text;
        string verticalRisingSpeedString = verticalRisingSpeedText.text;
        string verticalFallingSpeedString = verticalFallingSpeedText.text;

        if (separationString.Contains(".")) { separationString = separationString.Replace(".", ","); }
        if (droneSizeString.Contains(".")) { droneSizeString = droneSizeString.Replace(".", ","); }
        if (horizontalSpeedString.Contains(".")) { horizontalSpeedString = horizontalSpeedString.Replace(".", ","); }
        if (verticalRisingSpeedString.Contains(".")) { verticalRisingSpeedString = verticalRisingSpeedString.Replace(".", ","); }
        if (verticalFallingSpeedString.Contains(".")) { verticalFallingSpeedString = verticalFallingSpeedString.Replace(".", ","); }
        
        float separation = float.Parse(separationString);
        float droneSize = float.Parse(droneSizeString);
        float horizontalSpeed = float.Parse(horizontalSpeedString);
        float verticalRisingSpeed = float.Parse(verticalRisingSpeedString);
        float verticalFallingSpeed = float.Parse(verticalFallingSpeedString);
        
        Choreography chor = new Choreography("tmp", horizontalSpeed, verticalRisingSpeed, 
            verticalFallingSpeed, separation, droneSize);
        GameEventsManager.OnChoreographyUpdateInitiated(chor);
    }

    public void OpenOptions() {
        Choreography currentChoreography = DroneGameManager.Instance.ChoreographyManager.ChoreographyData;
        
        separationText.text = currentChoreography.Separation.ToString();
        droneSizeText.text = currentChoreography.DroneSize.ToString();
        horizontalSpeedText.text = currentChoreography.HorizontalSpeed.ToString();
        verticalRisingSpeedText.text = currentChoreography.VerticalRisingSpeed.ToString();
        verticalFallingSpeedText.text = currentChoreography.VerticalFallingSpeed.ToString();
        SetQualityLevelText();
        optionsPanel.SetActive(true);
    }
    
    public void ToggleHideName() {
        choreographyHeader.SetActive(!choreographyHeader.activeSelf);
    }

    public void ToggleHideTimer() {
        choreographyTimer.SetActive(!choreographyTimer.activeSelf);
    }

    public void ToggleMenuWindow() {
        optionsWindow.SetActive(optionsWindow.activeSelf == false);
        mainCamera.GetComponent<ExtendedFlycam>().MenuIsActive = optionsWindow.activeSelf;
    }

    public void ChangeTimeButtonIcon() {
        pauseButton.transform.GetChild(0).GetComponent<Image>().sprite = 
            DroneGameManager.Instance.ChoreographyManager.IsRunning ? pauseIcon : startIcon;
    }

    public void ShowSavedAlert() {
        savedAlert.SetActive(true);
    }

    public void ShowSaveBeforeExitAlert() {
        saveBeforeExitAlert.SetActive(true);
    }

    public void GoBackToMainMenu() {
        SceneManager.LoadScene(0);
    }

    public void ShowDroneStepsWindows(string name) {
        droneName.text = name;
        if (droneStepsArea.transform.childCount > 0) {
            foreach (Transform child in droneStepsArea.transform) {
                Destroy(child.gameObject);
            }
            
            droneStepsArea.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 10);
        }
        droneStepsWindows.SetActive(true);
    }

    public void ShowDroneCoordinates(string name) {
        GameObject drone = DroneGameManager.Instance.DronesManager.Drones[name];
        Vector3 dronePosition = drone.transform.localPosition;
        
        coordX.text = "X: " + dronePosition.x;
        coordY.text = "Y: " + dronePosition.y;
        coordZ.text = "Z: " + dronePosition.z;
    }
    
    public void SetQualityLevel(int index) {
        QualitySettings.SetQualityLevel(index);
        SetQualityLevelText();
    }
    
    
    

    public void HideDroneStepsWindows() {
        droneStepsWindows.SetActive(false);
    }

    public void ShowDeleteDroneReferencePointAlert() {
        deleteReferencePointAlert.SetActive(true);
    }

    public void ShowNoDroneReferencePointAlert() {
        noDroneReferencePointAlert.SetActive(true);
    }

    public bool PlayingIsActive => playingIsActive;

    public GameObject PlayingMode => playingMode;
}
