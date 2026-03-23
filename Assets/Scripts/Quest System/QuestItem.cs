using UnityEngine;

public class QuestItem : MonoBehaviour
{   
    public QuestItemName questItemName;
    PlayerQuests playerQuests;
    bool goToPlayer;

    void Start()
    {
        playerQuests = Utilities.Player.Quests;
    }

    void Update()
    {
        if (!goToPlayer) return;
       
        // Move towards the player
        Utilities.MoveTowardsPlayer(transform);

        if(Utilities.IsCloseToPlayer(transform))
        {
            playerQuests.questItems.Add(this);
            gameObject.SetActive(false);
        }
        
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {  
        
        if(hitInfo.CompareTag("Player"))
        {   
            goToPlayer = true;
        }
    }
}
