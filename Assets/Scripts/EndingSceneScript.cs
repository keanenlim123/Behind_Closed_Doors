/// Author : Keanen Lim
/// Date Created : 03/02/2026
/// Description : Handles the ending scene logic, including user verification and updating user statistics in Firebase.
/// 

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Database;

public class EndingSceneScript : MonoBehaviour
{
    public Button firstVerificationButton;
    public Button secondVerificationButton;
    public string trueEndingScene = "TrueEnding";
    public string badEndingScene = "BadEnding";

    public bool isTrueEnding = true;
    private bool firstVerified = false;
    private bool statsUpdated = false;


    FirebaseAuth auth;
    DatabaseReference db;

    void Start() /// Initialize Firebase and set up button listeners
    {
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseDatabase.DefaultInstance.RootReference;

        firstVerificationButton.onClick.AddListener(OnFirstVerification);
        secondVerificationButton.onClick.AddListener(OnSecondVerification);
    }

    void OnFirstVerification() /// Handle the first verification step
    {
        firstVerified = true;
        Debug.Log("First verification complete.");
    }

    void OnSecondVerification() /// Handle the second verification step and update stats
    {
        if (!firstVerified || statsUpdated)
            return;

        statsUpdated = true;

        UpdateUserStats();

        if (isTrueEnding)
            SceneManager.LoadScene(trueEndingScene);
        else
            SceneManager.LoadScene(badEndingScene);
    }
    void UpdateUserStats() /// Update the user's statistics in Firebase
    {
        if (auth.CurrentUser == null)
            return;

        string uid = auth.CurrentUser.UserId;

        DatabaseReference statsRef = db
            .Child("users")
            .Child(uid)
            .Child("userStats");

        // Read once
        statsRef.GetValueAsync().ContinueWith(task =>
        {
            if (!task.IsCompleted || task.Result == null)
                return;

            int timesPlayed = 0;
            int difficulty = 0;

            DataSnapshot snapshot = task.Result;

            if (snapshot.Child("timesPlayed").Exists) /// Get current times played
                timesPlayed = int.Parse(snapshot.Child("timesPlayed").Value.ToString());

            if (snapshot.Child("difficulty").Exists) /// Get current difficulty
                difficulty = int.Parse(snapshot.Child("difficulty").Value.ToString());

            // +1 logic
            timesPlayed += 1;

            if (isTrueEnding)
                difficulty += 1;

            // Write back
            statsRef.Child("timesPlayed").SetValueAsync(timesPlayed);
            statsRef.Child("difficulty").SetValueAsync(difficulty);
        });
    }
}
