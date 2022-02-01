using UnityEngine;

public class StepsInstanceCreator : MonoBehaviour {
    [SerializeField] private GameObject droneStepsWindow = null;
    [SerializeField] private GameObject instantiatingButton = null;
    [SerializeField] private GameObject deletingButton = null;
    
    public void ChoseStep() {
        if (droneStepsWindow.transform.childCount > 0) {
            bool hasStart = false;
            foreach (Transform child in droneStepsWindow.transform) {
                if (child.name.Equals("Stop")) { return; }
                if (child.name.Equals("Start") && transform.name.Equals("Start")) { return; }
                if (child.name.Equals("Start") || transform.name.Equals("Start")) { hasStart = true; }
            }
            if (!hasStart) { return; }
        }
        else {
            if (!transform.name.Equals("Start")) { return; }
        }
        
        RectTransform rt = droneStepsWindow.GetComponent<RectTransform>();
        Vector2 mySizeDelta = rt.sizeDelta;
        Transform myTransform = transform;
        float height = mySizeDelta.y + myTransform.GetComponent<RectTransform>().sizeDelta.y + 20;
        float width = mySizeDelta.x;
        
        Vector2 newSize = new Vector2(width, height);
        rt.sizeDelta = newSize;

        GameObject stepInstance = Instantiate(myTransform.gameObject, droneStepsWindow.transform, true);
        stepInstance.name = myTransform.name;
        Destroy(stepInstance.GetComponent<StepsInstanceCreator>());
    }
    
    private void OnDestroy() {
        deletingButton.SetActive(true);
        Destroy(instantiatingButton);
    }
}
