using System;
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

        if (Input.GetKeyDown(KeyCode.L))
        {
            ResetSave();
        }
    }

    public void ChangeState(IGameState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void SaveGame(Transform spawnPos, string c_currentAreaName, int keys, int health)
    {
        SaveData data = new()
        {
            playerPosX = spawnPos.position.x,
            playerPosY = spawnPos.position.y,
            playerKeys = keys,
            playerCurrentHealth = health,
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

        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        PlayerHealth player = playerGO.GetComponent<PlayerHealth>();
        PlayerQuests playerQuests = playerGO.GetComponent<PlayerQuests>();
        if(playerGO == null)
        {
            Debug.LogError("Player not found in scene!");
            return;
        }

        string savedAreaName = data.currentAreaName;

        // Find the AreaData asset with this name
        AreaData[] allAreas = Resources.LoadAll<AreaData>("AreaData"); // load all assets in Resources/AreaData folder
        AreaData areaToLoad = Array.Find(allAreas, a => a.areaName == savedAreaName);

        if(areaToLoad != null)
        {
            AreaManager.Instance.SetCurrentArea(areaToLoad);
            playerGO.transform.position = new Vector2(data.playerPosX, data.playerPosY);
            player.currentHealth = data.playerCurrentHealth;
            playerQuests.keysCollected = data.playerKeys;

            if(player.isDead)
            {
                player.Respawn();
            }
            

            Debug.Log("Loaded game in area: " + savedAreaName);
        }
        else
        {
            Debug.LogWarning("AreaData not found for saved area: " + savedAreaName);
        }
    }

    public void ResetSave()
    {
        SaveData defaultData = new(); // sets default values
        SaveSystem.SaveGame(defaultData);
        Debug.Log("Save reset to default values.");
    }
}
