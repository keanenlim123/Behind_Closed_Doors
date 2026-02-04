using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SplashScreenManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName; // Name of the next scene to load

    void Start()
    {
        // Subscribe to the video end event
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.Play(); // Ensure the video plays
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }

    // Optional: Allow skipping the video
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
