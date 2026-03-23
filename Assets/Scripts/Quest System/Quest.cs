using UnityEngine;

[System.Serializable]
public class Quest 
{
    public bool isActive;
    public bool isFinished;
    public QuestTitle questTitle;
    public QuestGoal questGoal;

    public Quest Init()
    {
        return new Quest
        {
            
            isActive = this.isActive,
            isFinished = this.isFinished,
            questTitle = this.questTitle,
            questGoal = this.questGoal, 
            
        };
    }

    public void Complete()
    {
        isActive = false;
        isFinished = true;
        Debug.Log(questTitle + " is Done!");
    }

}

[System.Serializable]
public class QuestSaveData
{
    public string questID;
    public bool isActive;
    public bool isFinished;
}

public enum QuestTitle
{
    QuestOne,
    QuestTwo,
    QuestThree,
    Default
    
}

