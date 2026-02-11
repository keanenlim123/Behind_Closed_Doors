using UnityEngine;
using Firebase.Database;

public class CaptionsBehaviour : MonoBehaviour
{
    private DatabaseReference db;

    void Start()
    {
        db = FirebaseDatabase.DefaultInstance.RootReference;

        AddEatingDisorder();
        AddDepression();
        AddSchizophrenia();
    }

    void AddEatingDisorder()
    {
        EatingDisorderCaptions eating = new EatingDisorderCaptions();
        string json = JsonUtility.ToJson(eating);

        db.Child("captions").Child("eatingDisorder")
          .SetRawJsonValueAsync(json);
    }

    void AddDepression()
    {
        DepressionCaptions depression = new DepressionCaptions();
        string json = JsonUtility.ToJson(depression);

        db.Child("captions").Child("depression")
          .SetRawJsonValueAsync(json);
    }

    void AddSchizophrenia()
    {
        SchizophreniaCaptions schizophrenia = new SchizophreniaCaptions();
        string json = JsonUtility.ToJson(schizophrenia);

        db.Child("captions").Child("schizophrenia")
          .SetRawJsonValueAsync(json);
    }
}
