using System;
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
            SaveGame(transform, "Limbo");
        }
    }

    public void ChangeState(IGameState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void SaveGame(Transform spawnPos, string c_currentAreaName)
    {
        var player = Utilities.Player;

        SaveData data = new()
        {
            playerPosX = spawnPos.position.x,
            playerPosY = spawnPos.position.y,
            playerKeys = player.Quests.keysCollected,
            playerCurrentHealth = player.Health.currentHealth,
            playerAbilities = player.Abilities.abilities,
            slot1 = player.Abilities.slot1,
            slot2 = player.Abilities.slot2,
            currentAreaName = c_currentAreaName
        };


        SaveSystem.SaveGame(data);
        Debug.Log("Game saved at area: " + c_currentAreaName);
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

        // Restore player state
        player.PlayerGO.transform.position = new Vector2(data.playerPosX, data.playerPosY);
        player.Health.UpdateHealth(data.playerCurrentHealth);
        player.Quests.keysCollected = data.playerKeys;
        player.Abilities.abilities = data.playerAbilities;

        data.slot1.onCooldown = false;
        data.slot2.onCooldown = false;
        player.Abilities.EquipAbility(data.slot1, 0);
        player.Abilities.EquipAbility(data.slot2, 1);
        AbilityUIManager.Instance.UpdateSlotUI();
        
        // Respawn if dead
        if (player.Health.isDead) player.Health.Respawn();

        Debug.Log($"Loaded game in area: {savedAreaName}");
    }

    public void ResetSave()
    {
        SaveData defaultData = new(); // sets default values
        SaveSystem.SaveGame(defaultData);
        Debug.Log("Save reset to default values.");
    }
}
