using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {
    [SerializeField] private Text timerText = null;

    private bool isRunning = false;

    private float startTime;
    private float t = 0;

    private void Start() {
        SetDelegates();
    }

    private void Update()
    {
        if (!isRunning) return;
        
        t += Time.deltaTime;
        string minutes = ((int) t / 60).ToString();
        string seconds = (t % 60).ToString("f2");

        timerText.text = minutes + ":" + seconds;
    }

    private void SetDelegates() {
        GameEventsManager.TimeToggleInitiated += ToggleTimer;
        GameEventsManager.RewindChoreographyInitiated += ResetTimer;
    }

    public void ToggleTimer() {
        isRunning = !isRunning;
    }
    
    public void ResetTimer() {
        if (isRunning) { ToggleTimer(); }
        t = 0;
        timerText.text = "0:00.00";
    }

    
}
