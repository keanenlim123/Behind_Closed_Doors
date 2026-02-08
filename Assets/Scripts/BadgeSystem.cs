using UnityEngine;
using System.Collections.Generic;
using Firebase.Database;
using Firebase.Auth;

public class BadgeSystem : MonoBehaviour
{
    public List<Achievement> achievements = new List<Achievement>();

    private DatabaseReference db;
    private string userId;

    void Start()
    {
        db = FirebaseDatabase.DefaultInstance.RootReference;
        userId = FirebaseAuth.DefaultInstance.CurrentUser?.UserId;

        achievements.Add(new Achievement("dumbells", "Do you even lift?", "Pickup Dumbells 10 times", 10));
        achievements.Add(new Achievement("laundry", "Separate clean from dirty", "Pickup Laundry 10 times", 10));
        achievements.Add(new Achievement("penknife", "Arts & Crafts", "Pickup Pen Knife 10 times", 10));
        achievements.Add(new Achievement("pills", "Medicine Cabinet", "Pickup Pills 10 times", 10));
        achievements.Add(new Achievement("played_twice", "You've Been Here Before...", "Complete the game more than once", 1));

        if (userId != null)
            CheckTimesPlayed();
    }

    private void CheckTimesPlayed()
    {
        db.Child("users").Child(userId).Child("userStats").Child("timesPlayed").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
            {
                int timesPlayed = 0;
                int.TryParse(task.Result.Value.ToString(), out timesPlayed);

                if (timesPlayed > 1)
                {
                    var ach = achievements.Find(a => a.id == "played_twice");
                    if (ach != null && !ach.unlocked)
                    {
                        ach.progress = 1;
                        ach.unlocked = true;
                        SaveBadge(ach);
                    }
                }
            }
        });
    }

    public void RegisterPickup(string item)
    {
        foreach (var ach in achievements)
        {
            if (ach.id == item)
            {
                ach.progress++;
                if (ach.CheckUnlock())
                {
                    SaveBadge(ach);
                }
            }
        }
    }

    private void SaveBadge(Achievement achievement)
    {
        if (userId == null) return;
        string badgePath = $"users/{userId}/userStats/badges/{achievement.id}";
        db.Child(badgePath).SetRawJsonValueAsync(JsonUtility.ToJson(achievement));
        Debug.Log($"Badge unlocked: {achievement.name}");
    }

    // For future achievements, just add new Achievement instances in Start()
}