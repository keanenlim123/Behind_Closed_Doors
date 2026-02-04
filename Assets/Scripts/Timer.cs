using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Database;
using Firebase.Auth;

public class Timer : MonoBehaviour
{
    private float elapsedTime = 0f;
    private bool timerRunning = false;

    void Start()
    {
        Debug.Log("Timer started.");
        elapsedTime = 0f;
        timerRunning = true;
        // Listen for scene changes
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void Update()
    {
        if (timerRunning)
        {
            elapsedTime += Time.deltaTime;
        }
    }

    // Call this when the game ends
    public void EndGame()
    {
        if (timerRunning)
        {
            timerRunning = false;
            UploadTimeToFirebase();
            // Optionally, remove the sceneUnloaded event if game ends
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
    }

    private void OnSceneUnloaded(Scene current)
    {
        if (timerRunning)
        {
            timerRunning = false;
            UploadTimeToFirebase();
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
    }

    private void UploadTimeToFirebase()
    {
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
        if (user == null)
        {
            Debug.LogWarning("No user logged in, cannot upload time.");
            return;
        }

        string uid = user.UserId;
        DatabaseReference db = FirebaseDatabase.DefaultInstance.RootReference;
        db.Child("users").Child(uid).Child("bestRecordedTime").SetValueAsync(elapsedTime)
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Failed to upload session time: " + task.Exception);
                }
                else
                {
                    Debug.Log("Session time uploaded: " + elapsedTime + " seconds");
                }
            });
    }

    void OnApplicationQuit()
    {
        if (timerRunning)
        {
            timerRunning = false;
            UploadTimeToFirebase();
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
    }


    void OnDestroy()
    {
        // Clean up event subscription
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}