using UnityEngine;
using UnityEngine.Events;

public class KeyListener : MonoBehaviour {
    [SerializeField] private KeyCode key;
    [SerializeField] private UnityEvent myEvent = null;

    void Update() {
        if (Input.GetKeyDown(key)) {
            myEvent?.Invoke();
        }
    }
}
