using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class StatesData
{
    public GameObject pausedPanel;
    public GameObject gameoverPanel;
}

public class GameController : MonoBehaviour
{   
    public static GameController Instance;

    private IGameState currentState;

    public InputHandler InputHandler { get; private set; }

    public PlayingState PlayingState { get; private set; }
    public PausedState PausedState { get; private set; }
    public GameOverState GameOverState { get; private set; }

    public StatesData statesData;
    
    void Awake()
    {   
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        InputHandler = gameObject.AddComponent<InputHandler>();

        PlayingState = new PlayingState(this, statesData);
        PausedState = new PausedState(this, statesData);
        GameOverState = new GameOverState(this, statesData);
    }

    void Start()
    {   
        //Change state to playing on start
        ChangeState(PlayingState);

        //Load a checkpoint
        LoadGame();

        //Load Utilities
        Utilities.InitUtilities();
    }

    void Update()
    {
        currentState?.Tick();


        //For Testing
        if (Input.GetKeyDown(KeyCode.L))
        {
            ResetSave();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveGame();
        }
    }

    public void ChangeState(IGameState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void SaveGame()
    {
        var player = Utilities.Player;

        Vector2 spawnPos = AreaManager.Instance.LastSpawnPos;
        string currentAreaName = AreaManager.Instance.CurrentArea.areaName;

        SaveData data = new()
        {
            playerPosX = spawnPos.x,
            playerPosY = spawnPos.y,
            playerKeys = player.Quests.keysCollected,
            playerCurrentHealth = player.Health.currentHealth,
            playerAbilities = player.Abilities.abilities,
            slot1 = player.Abilities.slot1,
            slot2 = player.Abilities.slot2,
            currentAreaName = currentAreaName,
            quests = new List<QuestSaveData>()

        };

        foreach (var npc in Utilities.AllNPCs)
        {
            data.quests.Add(npc.GetSaveData());
        }
        

        SaveSystem.SaveGame(data);
        Debug.Log("Game saved at area: " + currentAreaName);
    }

    public void LoadGame()
    {   
        SaveData data = SaveSystem.LoadGame();
        if (data == null)
            return;

        var player = Utilities.Player;

        if(player.PlayerGO == null)
        {
            Debug.LogError("Player not found in scene!");
            return;
        }

        string savedAreaName = data.currentAreaName;
        
        AreaData areaToLoad = Resources.LoadAll<AreaData>("AreaData")
                                    .FirstOrDefault(a => a.areaName == savedAreaName);

        if (areaToLoad == null)
        {
            Debug.LogWarning($"AreaData not found for saved area: {savedAreaName}");
            return;
        }

        // Set the current area
        AreaManager.Instance.SetCurrentArea(areaToLoad);

        Vector2 spawnPos = new(data.playerPosX, data.playerPosY);
        AreaManager.Instance.SetAreaVariables(areaToLoad, spawnPos);

        // Restore player state
        player.PlayerGO.transform.position = spawnPos;
        player.Health.UpdateHealth(data.playerCurrentHealth);
        player.Quests.keysCollected = data.playerKeys;
        player.Abilities.abilities = data.playerAbilities;

        if(data.slot1 != null && data.slot2 != null)
        {
            data.slot1.onCooldown = false;
            data.slot2.onCooldown = false;

            player.Abilities.EquipAbility(data.slot1, 0);
            player.Abilities.EquipAbility(data.slot2, 1);
            AbilityUIManager.Instance.UpdateSlotUI();
        }
        
        // Respawn if dead
        if (player.Health.isDead) player.Health.Respawn();

        StartCoroutine(LoadAfterSceneReady(data));

        Debug.Log($"Loaded game in area: {savedAreaName}");
    }

    IEnumerator LoadAfterSceneReady(SaveData data)
    {
        yield return null; // wait 1 frame 

        foreach (var npc in Utilities.AllNPCs)
        {
            foreach (var savedQuest in data.quests)
            {
                npc.LoadFromSave(savedQuest);
            }
        }
    }

    public void ResetSave()
    {
        SaveData defaultData = new(); // sets default values
        SaveSystem.SaveGame(defaultData);
        Debug.Log("Save reset to default values.");
    }
}
