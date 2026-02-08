using System;

[Serializable]
public class Achievement
{
    public string id;
    public string name;
    public string description;
    public bool unlocked;
    public int progress;
    public int target;

    public Achievement(string id, string name, string description, int target)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.target = target;
        this.unlocked = false;
        this.progress = 0;
    }

    public bool CheckUnlock()
    {
        if (!unlocked && progress >= target)
        {
            unlocked = true;
            return true;
        }
        return false;
    }
}