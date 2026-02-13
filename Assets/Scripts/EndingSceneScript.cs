using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Database;

public class EndingSceneScript : MonoBehaviour
{
    public string trueEndingScene = "TrueEnding";
    public string badEndingScene = "BadEnding";

    [Header("Ending Options")]
    public GameObject correctOption;
    public GameObject wrongOption1;
    public GameObject wrongOption2;

    private bool statsUpdated = false;
    private bool isTrueEnding = false;

    FirebaseAuth auth;
    DatabaseReference db;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void OnSubmit()
    {
        if (statsUpdated)
            return;

        // Check which object is active
        if (correctOption.activeSelf)
        {
            isTrueEnding = true;
            Debug.Log("Correct option is active.");
        }
        else if (wrongOption1.activeSelf || wrongOption2.activeSelf)
        {
            isTrueEnding = false;
            Debug.Log("Wrong option is active.");
        }
        else
        {
            Debug.Log("No option is active.");
            return;
        }

        statsUpdated = true;
        UpdateUserStats();

        if (isTrueEnding)
            SceneManager.LoadScene(trueEndingScene);
        else
            SceneManager.LoadScene(badEndingScene);
    }

    void UpdateUserStats()
    {
        if (auth.CurrentUser == null)
            return;

        string uid = auth.CurrentUser.UserId;

        DatabaseReference statsRef = db
            .Child("users")
            .Child(uid)
            .Child("userStats");

        statsRef.GetValueAsync().ContinueWith(task =>
        {
            if (!task.IsCompleted || task.Result == null)
                return;

            int timesPlayed = 0;
            int difficulty = 0;

            DataSnapshot snapshot = task.Result;

            if (snapshot.Child("timesPlayed").Exists)
                timesPlayed = int.Parse(snapshot.Child("timesPlayed").Value.ToString());

            if (snapshot.Child("difficulty").Exists)
                difficulty = int.Parse(snapshot.Child("difficulty").Value.ToString());

            timesPlayed += 1;

            if (isTrueEnding)
                difficulty += 1;

            statsRef.Child("timesPlayed").SetValueAsync(timesPlayed);
            statsRef.Child("difficulty").SetValueAsync(difficulty);
        });
    }
}
 