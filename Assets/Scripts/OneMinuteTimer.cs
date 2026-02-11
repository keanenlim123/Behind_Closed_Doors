/// Author : Waine Low
/// Date Created : 27/01/2026
/// Description : Implements a one-minute countdown timer that transitions to the next scene upon completion.
/// 

using UnityEngine;
using UnityEngine.SceneManagement;

public class OneMinuteTimer : MonoBehaviour
{
    public float timeLeft = 60f; // 60 seconds
    private bool timerRunning = false;
    public string nextSceneName;

    void Start() /// Initialize and start the timer
    {
        timerRunning = true;
    }

    void Update() /// Update the timer each frame
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

    void TimerFinished() /// Handle timer completion and scene transition
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}