/// Author : Jaasper Lee
/// Date Created : 05/02/2026
/// Description : Represents an achievement in the game.
/// Each achievement has an ID, name, description, unlock status, progress towards unlocking, and a target value for unlocking.

using System;

[Serializable]
public class Achievement
{
    public string id; /// Achievement id for Firebase to reference
    public string name; /// Achievement name to display
    public string description; /// Achievement description to display
    public bool unlocked; /// Whether the achievement is unlocked
    public int progress; /// Current progress towards unlocking the achievement
    public int target; /// Target value to unlock the achievement

    public Achievement(string id, string name, string description, int target) /// Constructor to initialize an achievement
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.target = target;
        this.unlocked = false;
        this.progress = 0;
    }

    public bool CheckUnlock() /// Method to check if the achievement is unlocked
    {
        if (!unlocked && progress >= target)
        {
            unlocked = true;
            return true;
        }
        return false;
    }
}