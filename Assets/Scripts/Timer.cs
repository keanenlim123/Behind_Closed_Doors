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
    DatabaseReference bestTimeRef = db.Child("users").Child(uid).Child("userStats").Child("bestRecordedTime");

    bestTimeRef.GetValueAsync().ContinueWith(task =>
    {
        if (task.IsFaulted)
        {
            Debug.LogError("Failed to retrieve best time: " + task.Exception);
            return;
        }

        float previousBest = float.MaxValue;
        if (task.Result.Exists && float.TryParse(task.Result.Value.ToString(), out float fetchedBest))
        {
            previousBest = fetchedBest;
        }

        if (elapsedTime < previousBest)
        {
            bestTimeRef.SetValueAsync(elapsedTime).ContinueWith(setTask =>
            {
                if (setTask.IsFaulted)
                {
                    Debug.LogError("Failed to upload session time: " + setTask.Exception);
                }
                else
                {
                    Debug.Log("Session time uploaded: " + elapsedTime + " seconds");
                }
            });
        }
        else
        {
            Debug.Log("New time (" + elapsedTime + "s) is not better than previous best (" + previousBest + "s). Not updating.");
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