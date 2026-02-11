using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

public class SceneTransitionManager : MonoBehaviour
{
    public FadeScreen fadeScreen;

    [Header("Scene Lists")]
    public List<int> normalScenes = new List<int>(); // Difficulty 0
    public List<int> hardScenes = new List<int>();   // Difficulty > 0

    private FirebaseAuth auth;
    private DatabaseReference db;

    private int userDifficulty = 0; // Default to normal

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseDatabase.DefaultInstance.RootReference;

        FetchUserDifficulty();
    }

    private void FetchUserDifficulty()
    {
        if (auth.CurrentUser == null)
        {
            Debug.LogWarning("No user logged in, defaulting difficulty to 0");
            userDifficulty = 0;
            return;
        }

        string uid = auth.CurrentUser.UserId;

        db.Child("users").Child(uid).Child("userStats").Child("difficulty")
          .GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to fetch difficulty: " + task.Exception);
                userDifficulty = 0;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    if (snapshot.Value is long)
                        userDifficulty = (int)(long)snapshot.Value;
                    else if (snapshot.Value is int)
                        userDifficulty = (int)snapshot.Value;
                    else if (snapshot.Value is string)
                        int.TryParse((string)snapshot.Value, out userDifficulty);

                    Debug.Log("Fetched difficulty from Firebase: " + userDifficulty);
                }
                else
                {
                    Debug.Log("Difficulty not found, defaulting to 0");
                    userDifficulty = 0;
                }
            }
        });
    }
    // Called by your UI button
    public void OnNextSceneButtonClicked()
    {
        StartCoroutine(RunRandomScene());
    }

    private IEnumerator RunRandomScene()
    {
        // Pick a random scene based on fetched difficulty
        int sceneIndex;

        if (userDifficulty > 0 && hardScenes.Count > 0)
        {
            sceneIndex = hardScenes[Random.Range(0, hardScenes.Count)];
        }
        else if (normalScenes.Count > 0)
        {
            sceneIndex = normalScenes[Random.Range(0, normalScenes.Count)];
        }
        else
        {
            Debug.LogWarning("Scene lists are empty!");
            yield break;
        }

        // Fade out before loading
        fadeScreen.gameObject.SetActive(true);
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(fadeScreen.fadeDuration);

        SceneManager.LoadScene(sceneIndex);
    }
}
