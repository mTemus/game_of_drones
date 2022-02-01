using UnityEngine;

public class ManageChoreography : MonoBehaviour {
    private MenuEvents parent;
    
    public void Load() {
        string choreography = transform.name;
        
        PlayerPrefs.SetString("mode", "load");
        PlayerPrefs.SetString("name", choreography);
        parent.LoadLevel("MainScene");
    }

    public void Delete() {
        DataManager.DataToDelete = transform.name;
        GUIEventsManager.OnDeleteAlertShowInitiated();
    }
    
    public MenuEvents Parent {
        get => parent;
        set => parent = value;
    }
}
