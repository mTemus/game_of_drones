using UnityEngine;
using RenderSettings = UnityEngine.RenderSettings;

public class EnvironmentManager : MonoBehaviour
{
    [Header("SkyBox materials")] 
    [SerializeField] private Material[] skyboxes = null;

    [Header("Main light")]
    [SerializeField] private Light mainLight = null;
    
    
    private int skyboxIDX;
    private FogData lightFog;
    private FogData darkFog;
    
    
    private void Awake() {
        skyboxIDX = 0;
        SetDelegates();
        
        lightFog = new FogData(RenderSettings.fogColor, RenderSettings.fogStartDistance, RenderSettings.fogEndDistance);
        darkFog = new FogData(Color.black, 30, 300);
    }

    private void SetDelegates() {
        GameEventsManager.SaveInitiated += SaveCurrentSkybox;
        GameEventsManager.LoadInitiated += LoadCurrentSkybox;
    }

    private void SaveCurrentSkybox() {
        int currentSkybox;

        if (skyboxIDX == 0) { currentSkybox = 0; }
        else { currentSkybox = skyboxIDX - 1; }
        
        if (currentSkybox < 0) { currentSkybox = skyboxes.Length - 1; }
        
        DataManager.Save(currentSkybox, DroneGameManager.Instance.ChoreographyManager.ChoreographyData.Name, "skybox");
    }

    private void LoadCurrentSkybox(Choreography choreography) {
        skyboxIDX = DataManager.Load<int>(choreography.Name,
            "skybox");
        ToggleSkybox();
    }
    
    public void ToggleSkybox() {

        if (skyboxIDX == 3 || skyboxIDX == 4) {
            RenderSettings.fogColor = darkFog.Color;
            RenderSettings.fogStartDistance = darkFog.StartDistance;
            RenderSettings.fogEndDistance = darkFog.EndDistance;
            mainLight.gameObject.SetActive(false);
        }
        else {
            RenderSettings.fogColor = lightFog.Color;
            RenderSettings.fogStartDistance = lightFog.StartDistance;
            RenderSettings.fogEndDistance = lightFog.EndDistance;
            mainLight.gameObject.SetActive(true);
        }
        
        RenderSettings.skybox = skyboxes[skyboxIDX];
        skyboxIDX++;
        if (skyboxIDX >= skyboxes.Length) { skyboxIDX = 0; }
    }
}
