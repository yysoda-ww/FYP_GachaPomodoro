using System;

[Serializable]
public class PlayerProfile
{
    public string username;
    public int coins;
    public int level;
    public int exp;
    public int studyTimeSeconds;
    public int tasksCompleted;

    public PlayerProfile(string username)
    {
        this.username = username;
        coins = 0;
        level = 1;
        exp = 0;
        studyTimeSeconds = 0;
        tasksCompleted = 0;
    }
}