using UnityEngine;

[System.Serializable]
public class QuestGoal 
{
    public QuestItemName requiredQuestItem;
    public QuestItemName currentQuestItem;

    public bool IsReached()
    {
        return requiredQuestItem == currentQuestItem;
    }

    public void GiveQuestItem(QuestItemName item)
    {
        currentQuestItem = item;
    }
}

public enum QuestItemName
{
    QuestOneItem,
    QuestTwoItem,
    QuestThreeItem,
    Default
    
}
