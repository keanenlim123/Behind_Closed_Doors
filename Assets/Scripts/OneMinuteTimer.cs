using UnityEngine;
using UnityEngine.SceneManagement;

public class OneMinuteTimer : MonoBehaviour
{
    public float timeLeft = 60f; // 60 seconds
    private bool timerRunning = false;
    public string nextSceneName;

    void Start()
    {
        timerRunning = true;
    }

    void Update()
    {
        if (timerRunning)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
            }
            else
            {
                timeLeft = 0;
                timerRunning = false;
                TimerFinished();
            }
        }
    }

    void TimerFinished()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}