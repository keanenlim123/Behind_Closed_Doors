/// Author : Jaasper Lee
/// Date Created : 22/01/2026
/// Description : Manages the splash screen video playback and transitions to the next scene.
/// 

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SplashScreenManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName; /// Name of the next scene to load

    void Start()
    {
        /// Subscribe to the video end event
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.Play(); /// Ensure the video plays
    }

    void OnVideoEnd(VideoPlayer vp) /// Called when the video finishes playing
    {
        /// Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }

    void Update() /// Allow skipping the splash screen with spacebar or mouse click
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
