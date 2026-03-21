using TMPro;
using UnityEngine;

public class QuestDoor : MonoBehaviour
{
    PlayerQuests playerQuests;
    [SerializeField] BoxCollider2D doorCollider;
    [SerializeField] TextMeshProUGUI doorText;
    [SerializeField] int keysRequired = 3; 
    
    void Start()
    {
        playerQuests = Utilities.Player.Quests;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (Utilities.HitTarget())
            {
                TryOpenDoor();
            }
        }
    }

    void TryOpenDoor()
    {
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

    void OpenDoor()
    {
        Debug.Log("Door is now open!");
        doorCollider.enabled = true;
        doorText.text = "DOOR IS \n OPENED";
    }
}
