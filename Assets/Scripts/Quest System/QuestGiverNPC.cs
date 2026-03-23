using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class QuestGiverNPC : MonoBehaviour
{
    [SerializeField] Quest quest;
    public PlayerQuests playerQuests;

    [SerializeField] GameObject talkIcon;
    [SerializeField] GameObject npcSprite;
    [SerializeField] NPCData npc;

    private bool isTalking = false;
    private GameObject currentPanel;
    DialogueUIManager dialogueUIManager;

    [SerializeField] float interactionDistance = 9f;

    void Awake()
    {
        Utilities.AllNPCs.Add(this);
    }

    void Start()
    {
        playerQuests = Utilities.Player.Quests;
        dialogueUIManager = Utilities.DialogueUIManager;
        quest = npc.quest.Init();
    }

    void Update()
    {
        // Only show talk icon if not talking
        talkIcon.SetActive(!isTalking && Utilities.NPCDistanceToPlayer(transform) <= interactionDistance);
    }

    public void StartConversation(List<QuestItem> questItems)
    {
        // Check distance
        if (Utilities.NPCDistanceToPlayer(transform) > interactionDistance)
        {
            Debug.Log("NPC is too far away to interact.");
            return;
        }

        // Start Talking if player close to npc
        isTalking = true;
        talkIcon.SetActive(false);

        StartDialogue(questItems);
    }

    void StartDialogue(List<QuestItem> questItems)
    {   
        if (currentPanel != null)
        return;

        DialoguePanel panel = dialogueUIManager.CreateNPCDialoguePanel(transform, npc, EndTalk);
        currentPanel = panel.gameObject;

        if (!quest.isActive && !quest.isFinished)
            StartingQuestDialogue(panel);
        else if(quest.isActive && !quest.isFinished)
        {   
            // Find the quest item from player
            QuestItem foundItem = questItems.Find(item => item.questItemName == quest.questGoal.requiredQuestItem);

            // Init the quest item if foundItem is not null, else make quest item default
            QuestItemName questItemName = foundItem != null ? foundItem.questItemName : QuestItemName.Default;

            FinishingQuestDialogue(questItemName, panel);
        }
    }

    void StartingQuestDialogue(DialoguePanel panel)
    {
        panel.SetDialogue(npc.StartingDialogue);
        dialogueUIManager.InitInteractButton(this, panel, AcceptQuest, "Accept");
    }

    void FinishingQuestDialogue(QuestItemName item, DialoguePanel panel)
    {
        panel.SetDialogue(npc.FinishingDialogue);
        dialogueUIManager.InitInteractButton(this, panel, () => TryGiveItem(item), "Give");
    }

    void FinishedQuestDialogue()
    {   
        EndTalk();

        DialoguePanel newPanel = dialogueUIManager.CreateNPCDialoguePanel(transform, npc, EndTalk);
        currentPanel = newPanel.gameObject;

        List<string>  dialogue = new List<string> { "Thanks Again! and GoodLuck!" };
        newPanel.SetDialogue(dialogue);
        dialogueUIManager.InitInteractButton(this, newPanel, FreeNPC, "Continue");
    }

    void AcceptQuest()
    {
        quest.isActive = true;
        playerQuests.quests.Add(quest);
        EndTalk();
    }

    void TryGiveItem(QuestItemName item)
    {
        if (!quest.isActive) return;

        quest.questGoal.GiveQuestItem(item);

        // Remove old panel
        EndTalk();

        DialoguePanel newPanel = dialogueUIManager.CreateNPCDialoguePanel(transform, npc, EndTalk);
        currentPanel = newPanel.gameObject;

        List<string> dialogue;

        if (quest.questGoal.IsReached())
        {
            dialogue = new List<string> { "Thank you! Here's your Reward!" };
            newPanel.SetDialogue(dialogue);
            dialogueUIManager.InitInteractButton(this, newPanel, GiveReward, "Receive");
        }
        else
        {
            dialogue = new List<string> { "Sorry, you don't have the item yet." };
            newPanel.SetDialogue(dialogue);
            dialogueUIManager.InitInteractButton(this, newPanel, EndTalk , "Back");
        }
    }

    void GiveReward()
    {
        playerQuests.AddKey();
        quest.Complete();
        
        FinishedQuestDialogue();
    }

    void EndTalk()
    {
        isTalking = false;
        Utilities.Factory.Release(ObjectType.Dialogue, currentPanel);
        currentPanel = null;
    }

    void FreeNPC()
    {
        isTalking = false;
        Utilities.Factory.Release(ObjectType.Dialogue, currentPanel);
        currentPanel = null;

        GameController.Instance.SaveGame();
        npcSprite.SetActive(false);
    }

    public QuestSaveData GetSaveData()
    {
        return new QuestSaveData
        {
            questID = quest.questTitle.ToString(),
            isActive = quest.isActive,
            isFinished = quest.isFinished,
        };
    }

    public void LoadFromSave(QuestSaveData data)
    {
        if (data.questID != quest.questTitle.ToString()) return;

        quest.isActive = data.isActive; 
        quest.isFinished = data.isFinished;
        
        npcSprite.SetActive(!quest.isFinished);
    }
}