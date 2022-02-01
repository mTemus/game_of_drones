using UnityEngine;

public class DeleteAlertButton : MonoBehaviour {
    
    public void Delete() {
        DataManager.DeleteSavedData(DataManager.DataToDelete);
        GUIEventsManager.OnDeleteAlertHideInitiated();
        DataManager.DataToDelete = "";
    }

    public void CancelDeleting() {
        DataManager.DataToDelete = "";
    }
}
