using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuestGiverNPC : MonoBehaviour
{
    public Quest quest;

    public PlayerQuests playerQuests;
    [SerializeField] GameObject questWindow;
    [SerializeField] GameObject giveItemWindow;
    [SerializeField] TextMeshProUGUI questTitleText;
    [SerializeField] Button acceptButton;
    [SerializeField] Button giveItemButton;

    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private GameObject talkIcon;
    bool isTalking = false;
    private static QuestGiverNPC activeNPC;
    void Start()
    {
        playerQuests = PlayerQuests.Instance;
    }

    void Update()
    {
        if (playerQuests != null && !isTalking)
        {
            UpdateTalkableVisual();
        }
        else if (isTalking)
        {
            talkIcon.SetActive(false); // hide while talking
        }

    }

    public void OpenQuestWindow()
    {   
        questWindow.SetActive(true);

        //Set Quest Title
        Transform titleTransform = questWindow.transform.Find("Quest Title");
        questTitleText = titleTransform.GetComponent<TextMeshProUGUI>();
        questTitleText.text = quest.questTitle;

        //Add a Listener to button
        acceptButton = questWindow.GetComponentInChildren<Button>();
        acceptButton.onClick.RemoveAllListeners();
        acceptButton.onClick.AddListener(AcceptQuest);

        questWindow.transform.position = transform.position + new Vector3(0, 3, 0);
    }

    public void AcceptQuest()
    {
        Debug.Log("Quest Accepted!");

        questWindow.SetActive(false);

        quest.isActive = true;
        playerQuests.quests.Add(quest);

        EndTalk();
    }

    public void OpenGivingItemWindow(string item)
    {   
        giveItemWindow.SetActive(true);

        //Set Quest Title
        Transform titleTransform = giveItemWindow.transform.Find("Quest Title");
        questTitleText = titleTransform.GetComponent<TextMeshProUGUI>();
        questTitleText.text = quest.questTitle;

        //Add a Listener to button
        giveItemButton = giveItemWindow.GetComponentInChildren<Button>();
        giveItemButton.onClick.RemoveAllListeners();
        giveItemButton.onClick.AddListener(() => TryGiveItem(item));

        giveItemWindow.transform.position = transform.position + new Vector3(0, 3, 0);
    }

    public void TryGiveItem(string item)
    {
        if(quest.isActive)
        {
            quest.questGoal.GiveQuestItem(item);

            if(quest.questGoal.IsReached())
            {
                playerQuests.AddKey();
                quest.Complete();
                giveItemWindow.SetActive(false);
                EndTalk();
            }
            else
            {
                Debug.Log("Quest Item not Found!");
            }
        }
    }

    void UpdateTalkableVisual()
    {
        float sqrDistance = (playerQuests.transform.position - transform.position).sqrMagnitude;
        talkIcon.SetActive(sqrDistance <= interactionDistance * interactionDistance);
    }

    public void TryInteractNPC(List<QuestItem> questItems)
    {   
        float sqrDistance = (playerQuests.transform.position - transform.position).sqrMagnitude;
        if (sqrDistance <= interactionDistance * interactionDistance)
        {   
            // End previous NPC conversation if there is one
            if (activeNPC != null && activeNPC != this)
            {
                activeNPC.EndTalk();
            }

            isTalking = true;
            activeNPC = this;
            talkIcon.SetActive(false);

            if (!quest.isActive)
            {
                OpenQuestWindow();
            }
            else //Quest is Active and will Open the window for giving the item
            {   
                QuestItem foundQuestItem = questItems.Find(item => item.questItemName == quest.questGoal.requiredQuestItem);
                string questItemName = "";

                if (foundQuestItem != null)
                {
                    Debug.Log("Found item: " + foundQuestItem.questItemName);
                    questItemName = foundQuestItem.questItemName;
                }
                else
                {
                    Debug.Log("Item not found.");
                }

                OpenGivingItemWindow(questItemName);
            }
        }
        else
        {
            Debug.Log("NPC is not yet Interactable.");
        }
    }

    public void EndTalk()
    {
        isTalking = false;
        activeNPC = null;
    }

}
