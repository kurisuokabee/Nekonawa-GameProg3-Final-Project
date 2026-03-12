using UnityEngine;

[System.Serializable]
public class QuestGoal 
{
    public string requiredQuestItem;
    public string currentQuestItem;

    public bool IsReached()
    {
        return requiredQuestItem == currentQuestItem;
    }

    public void GiveQuestItem(string item)
    {
        currentQuestItem = item;
    }

}
