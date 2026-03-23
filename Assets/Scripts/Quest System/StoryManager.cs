using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum StoryState
{
    Intro,
    FirstEnemyKilled,
    BossArea,
    Ending
}

public class StoryManager : MonoBehaviour
{   
    public static StoryManager Instance { get; private set; }
    DialogueUIManager dialogueUIManager;
    [SerializeField]StoryDialogueData storyDialogueData;
    [SerializeField] Enemy firstEnemy;
    [SerializeField] Enemy bossEnemy;
    [SerializeField] GameObject endCreditsPanel;
 
    GameObject currentPanel;
    StoryState currentState;
    bool isFirstEnemyKilled = false;
    private static WaitForSecondsRealtime _waitForSecondsRealtime0_5 = new WaitForSecondsRealtime(0.5f);
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    void Start()
    {
        dialogueUIManager = Utilities.DialogueUIManager;
      
        currentState = StoryState.Intro; // start story at Intro

        EnterState(currentState);
    }

    void Update()
    {
        if(!firstEnemy.gameObject.activeInHierarchy && !isFirstEnemyKilled)
        {
            EnterState(StoryState.FirstEnemyKilled);
        }
    }
    public void PlayDialogueBlock(StoryState blockName, Action onFinish)
    {
        DialogueBlock block = storyDialogueData.GetBlock(blockName);
        if (block == null)
        {
            Debug.LogWarning("Block not found: " + blockName);
            return;
        }

        DialoguePanel panel = dialogueUIManager.CreateStoryDialoguePanel(transform);
        currentPanel = panel.gameObject;
        
        StartCoroutine(PlayBlockCoroutine(panel, block, onFinish));
    }

    IEnumerator PlayBlockCoroutine(DialoguePanel panel, DialogueBlock block, Action onFinish)
    {   
        //Loop through all lines from the dialogue block
        for (int i = 0; i < block.lines.Count; i++)
        {
            var line = block.lines[i];

            // Change speaker
            panel.npcNameText.text = line.speakerName.ToString();

            // Show dialogue line
            panel.SetDialogue(new List<string> { line.line });

            //Delay before skipping dialogue to avoid skipping dialogue from double clicks
            yield return _waitForSecondsRealtime0_5;

            // Only wait for left click if it's NOT the last line
            if (i < block.lines.Count - 1)
            {
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            }
        }

        //Dialogue block is done
        dialogueUIManager.InitInteractButton(this, panel, onFinish, "Continue");
    }

    void OnDialogueFinish(StoryState state)
    {
        Utilities.Factory.Release(ObjectType.Dialogue, currentPanel);

        switch(state)
        {
            case StoryState.Intro:
                Utilities.EnablePlayerControls();
                firstEnemy.gameObject.SetActive(true);
                firstEnemy.isChasing = true;
                break;
            
            case StoryState.BossArea:
                Utilities.EnablePlayerControls();
                bossEnemy.isChasing = true;
            break;

            case StoryState.Ending:
                endCreditsPanel.SetActive(true);
                endCreditsPanel.GetComponentInChildren<Button>().onClick.AddListener(GoToMainMenu);
            break;
        }
    }

    public void EnterState(StoryState state)
    {
        switch(state)
        {
            case StoryState.Intro:
                Utilities.DisablePlayerControls();
                PlayDialogueBlock(StoryState.Intro,() => OnDialogueFinish(state));
                break;

            case StoryState.FirstEnemyKilled:
                isFirstEnemyKilled = true;
                PlayDialogueBlock(StoryState.FirstEnemyKilled,() => OnDialogueFinish(state));
            break;    

            case StoryState.BossArea:
                Utilities.DisablePlayerControls();
                bossEnemy.gameObject.SetActive(true);
                PlayDialogueBlock(StoryState.BossArea,() => OnDialogueFinish(state));
            break;

            case StoryState.Ending:
                Utilities.DisablePlayerControls();
                PlayDialogueBlock(StoryState.Ending,() => OnDialogueFinish(state));
                break;
        }
    }

    void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu Scene");
        GameController.Instance.ResetSave();
    }
}

