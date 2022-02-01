using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void ShowDeleteAlert() {
        GUIEventsManager.OnDeleteAlertShowInitiated();
    }
    public void HideDeleteAlert() {
        GUIEventsManager.OnDeleteAlertHideInitiated();
    }
}
