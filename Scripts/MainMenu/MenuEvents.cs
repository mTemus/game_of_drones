using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuEvents : MonoBehaviour {

    public static MenuEvents Instance;
    
    [Header("Menu parts: ")]
    [SerializeField] private GameObject mainMenu = null;
    [SerializeField] private GameObject newChoreography = null;
    [SerializeField] private GameObject loadChoreography = null;
    [SerializeField] private GameObject credits = null;
    [SerializeField] private GameObject options = null;
    [SerializeField] private GameObject loadingBar = null;
    [SerializeField] private GameObject loadingArea = null;

    [Header("UI Alerts: ")]
    [SerializeField] private GameObject deleteAlert = null;
    [SerializeField] private GameObject choreographyExistsAlert = null;
    [SerializeField] private GameObject choreographyNameAlert = null;
    [SerializeField] private GameObject blankPropertiesAlert = null;
    
    [Header("UI items: ")]
    [SerializeField] private Slider loadingSlider = null;
        
    [Header("Inputs: ")]
    [SerializeField] private InputField choreographyNameInput = null;
    [SerializeField] private InputField separationInput = null;
    [SerializeField] private InputField horizontalSpeedInput = null;
    [SerializeField] private InputField verticalRisingSpeedInput = null;
    [SerializeField] private InputField verticalFallingSpeedInput = null;
    [SerializeField] private InputField droneSizeInput = null;
    
    [Header("Click sound: ")]
    [SerializeField] private AudioClip sound = null;
    
    private AudioSource source => GetComponent<AudioSource>();
    private Choreography defaultChoreography;
    private Dictionary<string, GameObject> saves;
    
    private void Awake() {
        if (Instance == null) { Instance = this; }
        SetDelegates();
        saves = new Dictionary<string, GameObject>();
        defaultChoreography = LoadDefaultValues();
        
    }

    private void Start() {
        gameObject.AddComponent<AudioSource>();
        source.clip = sound;
        source.playOnAwake = false;
    }
    
    private void PlayOnClick() {
        source.PlayOneShot(sound);
    }

    private void SetDelegates() {
        GUIEventsManager.DeleteAlertShowInitiated += ShowDeleteAlert;
        GUIEventsManager.DeleteAlertHideInitiated += HideDeleteAlert;
    }

    private void SetNewChoreography() {
        defaultChoreography.Name = choreographyNameInput.text;
    }

    public void LoadLevel(string level) {
        StartCoroutine(LoadAsynchronously(level));
    }

    private IEnumerator LoadAsynchronously(string level) {
        newChoreography.SetActive(false);
        loadingBar.SetActive(true);
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(level);

        while (!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingSlider.value = progress;
            yield return null;
        }
    }

    public void LoadSaves() {
        if (saves.Keys.Count != 0) {
            foreach (var key in saves.Keys) {
                Destroy(saves[key]);
            }
        }
        
        int spacing = 20;
        int padding = 20;

        string[] directories = Directory.GetDirectories(Application.persistentDataPath + "/saves/");

        foreach (var directory in directories) {
            DirectoryInfo choreography = new DirectoryInfo(directory);
            GameObject saveGO = (GameObject) Instantiate(Resources.Load("Prefabs/MainMenu/LoadDataButton"), loadingArea.transform, true);
            saveGO.name = choreography.Name;
            saveGO.transform.GetChild(0).GetComponent<Text>().text = choreography.Name;
            saveGO.transform.GetChild(1).GetComponent<Text>().text = "Utworzono: " + choreography.CreationTime;
            saveGO.GetComponent<ManageChoreography>().Parent = this;
            saves[directory] = saveGO;
        }
        
        int transformHeight = (padding * 2) + (saves.Keys.Count * 70) + ((saves.Keys.Count - 1) * spacing);
        loadingArea.GetComponent<RectTransform>().sizeDelta = new Vector2(230, transformHeight);
    }
    
    private void SavePrefs() {
        PlayerPrefs.SetString("mode", "new");
        PlayerPrefs.SetString("name", defaultChoreography.Name);
        PlayerPrefs.SetFloat("separation", defaultChoreography.Separation);
        PlayerPrefs.SetFloat("horSpeed", defaultChoreography.HorizontalSpeed);
        PlayerPrefs.SetFloat("verRisSpeed", defaultChoreography.VerticalRisingSpeed);
        PlayerPrefs.SetFloat("verFalSpeed", defaultChoreography.VerticalFallingSpeed);
        PlayerPrefs.SetFloat("droneSize", defaultChoreography.DroneSize);
    }

    private void SetDefaultOptions() {
        separationInput.text = defaultChoreography.Separation.ToString();
        droneSizeInput.text = defaultChoreography.DroneSize.ToString();
        horizontalSpeedInput.text = defaultChoreography.HorizontalSpeed.ToString();
        verticalRisingSpeedInput.text = defaultChoreography.VerticalRisingSpeed.ToString();
        verticalFallingSpeedInput.text = defaultChoreography.VerticalFallingSpeed.ToString();
    }

    private void UnsetDefaultOptions() {
        separationInput.text = "0";
        droneSizeInput.text = "0";
        horizontalSpeedInput.text = "0";
        verticalRisingSpeedInput.text = "0";
        verticalFallingSpeedInput.text = "0";
    }

    private bool ChangeChoreographyValues() {
        string separation = separationInput.text;
        string droneSize = droneSizeInput.text;
        string horizontalSpeed = horizontalSpeedInput.text;
        string verticalRisingSpeed = verticalRisingSpeedInput.text;
        string verticalFallingSpeed = verticalFallingSpeedInput.text;

        if (separation.Equals("") || droneSize.Equals("") || horizontalSpeed.Equals("") ||
            verticalRisingSpeed.Equals("") || verticalFallingSpeed.Equals("")) {
            blankPropertiesAlert.SetActive(true);
            return false;
        }

        if (separation.Contains(".")) { separation = separation.Replace(".", ","); }
        if (droneSize.Contains(".")) { droneSize = droneSize.Replace(".", ","); }
        if (horizontalSpeed.Contains(".")) { horizontalSpeed = horizontalSpeed.Replace(".", ","); }
        if (verticalRisingSpeed.Contains(".")) { verticalRisingSpeed = verticalRisingSpeed.Replace(".", ","); }
        if (verticalFallingSpeed.Contains(".")) { verticalFallingSpeed = verticalFallingSpeed.Replace(".", ","); }
        
        defaultChoreography.Separation = float.Parse(separation);
        defaultChoreography.DroneSize = float.Parse(droneSize);
        defaultChoreography.HorizontalSpeed = float.Parse(horizontalSpeed);
        defaultChoreography.VerticalRisingSpeed = float.Parse(verticalRisingSpeed);
        defaultChoreography.VerticalFallingSpeed = float.Parse(verticalFallingSpeed);
        
        DataManager.SaveConfig(defaultChoreography, "choreography configuration");
        return true;
    }

    private Choreography LoadDefaultValues() {
        return  DataManager.ConfigDataFileExists("choreography configuration") 
            ? DataManager.LoadConfig<Choreography>("choreography configuration") : new Choreography();
    }
    
    public void AcceptOptions() {
        if (ChangeChoreographyValues()) {
            GoBackToMainMenu();
        }
    }

    public void CreateNewChoreography() {
        PlayOnClick();
        mainMenu.SetActive(false);
        newChoreography.SetActive(true);
    }

    public void LoadChoreographies() {
        PlayOnClick();
        mainMenu.SetActive(false);
        loadChoreography.SetActive(true);
        LoadSaves();
    }

    public void ViewCredits() {
        PlayOnClick();
        mainMenu.SetActive(false);
        credits.SetActive(true);
    }

    public void ChangeOptions() {
        PlayOnClick();
        mainMenu.SetActive(false);
        options.SetActive(true);
        SetDefaultOptions();
    }
    
    public void GoBackToMainMenu() {
        PlayOnClick();
        
        if (newChoreography.activeSelf) {
            newChoreography.SetActive(false);
        } else if (loadChoreography.activeSelf) {
            loadChoreography.SetActive(false);
        } else if (credits.activeSelf) {
            credits.SetActive(false);
        } else if (options.activeSelf) {
            UnsetDefaultOptions();
            options.SetActive(false);
        }
        
        mainMenu.SetActive(true);
    }
    
    public void Quit(){
        PlayOnClick();
        Application.Quit();
    }

    public void StartNewChoreography() {
        SetNewChoreography();
        if (!DataManager.DataExists(defaultChoreography.Name)) {
            SavePrefs();
            LoadLevel("MainScene");
        }
        else if(defaultChoreography.Name.Equals("")){
            choreographyNameAlert.SetActive(true);
        }
        else {
            choreographyExistsAlert.SetActive(true);
        }
    }

    public void ShowDeleteAlert() {
        deleteAlert.SetActive(true);
        loadChoreography.SetActive(false);
    }

    public void HideDeleteAlert() {
        deleteAlert.SetActive(false);
        loadChoreography.SetActive(true);
    }
}
