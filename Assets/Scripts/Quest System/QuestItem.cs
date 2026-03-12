using UnityEngine;

public class QuestItem : MonoBehaviour
{   
    public string questItemName;
    PlayerQuests playerQuests;
    bool goToPlayer;

    void Start()
    {
        playerQuests = PlayerQuests.Instance;
    }

    void Update()
    {
        if (goToPlayer)
        {
            // Calculate direction towards the player
            Vector2 direction = (playerQuests.transform.position - transform.position).normalized;

            // Move towards the player
            transform.Translate(direction * 8f * Time.deltaTime);

            float sqrDistance = (playerQuests.transform.position - transform.position).sqrMagnitude;
            if(sqrDistance <= 0.5f * 0.5f)
            {
                playerQuests.questItems.Add(this);
                gameObject.SetActive(false);
            }
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
