using System.Collections.Generic;
using UnityEngine;

public class PlayerQuests : MonoBehaviour
{
    public static PlayerQuests Instance;

    public List<Quest> quests;
    public List<QuestItem> questItems;
    public int keysCollected;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {   
        if (Input.GetMouseButtonDown(0))
        {
            TalkToNPC();
        }
    }

    void TalkToNPC()
    {
        if (Utilities.HitTarget<QuestGiverNPC>())
        {
            if (Utilities.GetClickedObject().TryGetComponent<QuestGiverNPC>(out var npc))
            {
                npc.StartConversation(questItems);
            }    
        }
    }

    public void AddKey()
    {
        keysCollected++;
        Debug.Log("Key collected! Total keys: " + keysCollected);
    }

}
