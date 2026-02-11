/// Author : Keanen Lim
/// Date Created : 09/02/2026
/// Description : Handles the addition of caption data to the Firebase database.

using UnityEngine;
using Firebase.Database;

public class CaptionsBehaviour : MonoBehaviour
{
    private DatabaseReference db;

    void Start() /// Initialize the database reference and add caption data
    {
        db = FirebaseDatabase.DefaultInstance.RootReference;

        AddEatingDisorder();
        AddDepression();
        AddSchizophrenia();
    }

    void AddEatingDisorder() /// Add eating disorder captions to the database
    {
        EatingDisorderCaptions eating = new EatingDisorderCaptions();
        string json = JsonUtility.ToJson(eating);

        db.Child("captions").Child("eatingDisorder")
          .SetRawJsonValueAsync(json);
    }

    void AddDepression() /// Add depression captions to the database
    {
        DepressionCaptions depression = new DepressionCaptions();
        string json = JsonUtility.ToJson(depression);

        db.Child("captions").Child("depression")
          .SetRawJsonValueAsync(json);
    }

    void AddSchizophrenia() /// Add schizophrenia captions to the database
    {
        SchizophreniaCaptions schizophrenia = new SchizophreniaCaptions();
        string json = JsonUtility.ToJson(schizophrenia);

        db.Child("captions").Child("schizophrenia")
          .SetRawJsonValueAsync(json);
    }
}
