using UnityEngine;

[System.Serializable]
public class Quest 
{
    public bool isActive;

    public string questTitle;
    public QuestGoal questGoal;

    public void Complete()
    {
        isActive = false;
        Debug.Log(questTitle + " is Done!");
    }

}
