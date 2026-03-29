using TMPro;
using UnityEngine;

public class QuestDoor : MonoBehaviour
{
    PlayerQuests playerQuests;

    [SerializeField] BoxCollider2D doorCollider;

    [SerializeField] int keysRequired = 3;

    [Header("Door Sprites")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] doorStages; 
    
    void Start()
    {
        playerQuests = Utilities.Player.Quests;
        UpdateDoorSprite(); // initialize sprite
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Utilities.HitTarget<QuestDoor>())
            {
                TryOpenDoor();
            }
        }
    }

    void TryOpenDoor()
    {
        UpdateDoorSprite(); // update every click

        if (playerQuests.keysCollected >= keysRequired)
        {
            OpenDoor();
        }
        else
        {
            int missing = keysRequired - playerQuests.keysCollected;
            Debug.Log("You need " + missing + " more key(s) to open the door!");
        }
    }

    void UpdateDoorSprite()
    {
        int currentKeys = Mathf.Clamp(playerQuests.keysCollected, 0, keysRequired);

        // Pick sprite based on keys collected
        spriteRenderer.sprite = doorStages[currentKeys];
    }

    void OpenDoor()
    {
        Debug.Log("Door is now open!");
        doorCollider.enabled = false; 
    }
}