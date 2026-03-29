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
    DoorArea,
    GluttonyArea,
    AngerArea,
    Ending,
    Default
}

public class StoryManager : MonoBehaviour
{   
    public static StoryManager Instance { get; private set; }
    DialogueUIManager dialogueUIManager;
    [SerializeField]StoryDialogueData storyDialogueData;
    [SerializeField] GameObject firstEnemy;
    [SerializeField] GameObject bossEnemy;
    [SerializeField] GameObject bossHealthBar;
    [SerializeField] GameObject endCreditsPanel;
    public bool doorAreaTutorialDone;
    public bool gluttonyAreaTutorialDone;
    public bool angerAreaTutorialDone;
    public bool introDone;
 
    GameObject currentPanel;
    [SerializeField]StoryState currentState;
    Coroutine currentCoroutine;
    static List<string> tempList = new List<string>(1);
    
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

    public void PlayDialogueBlock(StoryState blockName, Action onFinish)
    {
        DialogueBlock block = storyDialogueData.GetBlock(blockName);
        if (block == null)
        {
            Debug.LogWarning("Block not found: " + blockName);
            return;
        }

        ClearCurrentPanel();

        DialoguePanel panel = dialogueUIManager.CreateStoryDialoguePanel(transform);
        currentPanel = panel.gameObject;

        currentCoroutine = StartCoroutine(PlayBlockCoroutine(panel, block, onFinish));
    }

    IEnumerator PlayBlockCoroutine(DialoguePanel panel, DialogueBlock block, Action onFinish)
    {
        for (int i = 0; i < block.lines.Count; i++)
        {
            var line = block.lines[i];

            panel.npcNameText.text = line.speakerName.ToString();

            tempList.Clear();
            tempList.Add(line.line);
            panel.SetDialogue(tempList);

            yield return _waitForSecondsRealtime0_5;

            if (i < block.lines.Count - 1)
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        }

        dialogueUIManager.InitInteractButton(this, panel, onFinish, "Continue");
    }

    void OnDialogueFinish(StoryState state)
    {   
        ClearCurrentPanel();

        GameController.Instance.SaveGame();
        currentState = state;
        switch (state)
        {
            case StoryState.Intro:
                Utilities.EnablePlayerControls();
                firstEnemy.SetActive(true);
                firstEnemy.GetComponent<Enemy>().isChasing = true;
                introDone = true;
                break;

            case StoryState.BossArea:
                Utilities.EnablePlayerControls();
                bossHealthBar.SetActive(true);
                bossEnemy.GetComponent<Lucifer_SM>().ChangeState<Lucifer_DecideState>();
                break;

            case StoryState.Ending:
                endCreditsPanel.SetActive(true);
                endCreditsPanel.GetComponentInChildren<Button>().onClick.AddListener(GoToMainMenu);
                break;
            case StoryState.FirstEnemyKilled:
                break;
            case StoryState.DoorArea:
                doorAreaTutorialDone = true;
                break;
            case StoryState.GluttonyArea:
                gluttonyAreaTutorialDone = true;
                break;
            case StoryState.AngerArea:
                angerAreaTutorialDone = true;
                break;
            case StoryState.Default:
                break;
        }

        EnterState(StoryState.Default);
    }

    public void EnterState(StoryState state)
    {   
        currentState = state;
        switch (state)
        {
            case StoryState.Intro:
                if(introDone) return;
                Utilities.DisablePlayerControls();
                PlayStateDialogue(state);
                break;

            case StoryState.FirstEnemyKilled:
                PlayStateDialogue(state);
                break;

            case StoryState.BossArea:
                Utilities.DisablePlayerControls();
                bossEnemy.SetActive(true);
                PlayStateDialogue(state);
                break;

            case StoryState.Ending:
                Utilities.Player.Health.Heal(100);
                Utilities.DisablePlayerControls(); 
                bossEnemy.GetComponent<Lucifer_SM>().enabled = false;
                bossEnemy.GetComponent<Lucifer_DecideState>().enabled = false; 
                bossHealthBar.SetActive(false);
                PlayStateDialogue(state);
                break;
            case StoryState.DoorArea:
                if (doorAreaTutorialDone) return;
                PlayStateDialogue(state);
                break;
            case StoryState.GluttonyArea:
                if (gluttonyAreaTutorialDone) return;
                PlayStateDialogue(state);
                break;
            case StoryState.AngerArea:
                if (angerAreaTutorialDone) return;
                PlayStateDialogue(state);
                break;
            case StoryState.Default:
                break;
        }
    }

    void PlayStateDialogue(StoryState state)
    {
        PlayDialogueBlock(state, () => OnDialogueFinish(state));
    }

    void ClearCurrentPanel()
    {
        if (currentPanel)
        {
            Utilities.Factory.Release(ObjectType.Dialogue, currentPanel);
            currentPanel = null;
        }

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
    }

    void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu Scene");
        GameController.Instance.ResetSave();
    }
}

