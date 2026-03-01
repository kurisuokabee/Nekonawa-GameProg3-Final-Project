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
        //LoadGame();
    }

    void Update()
    {
        currentState?.Tick();
    }

    public void ChangeState(IGameState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void SaveGame(Transform spawnPos, string c_currentAreaName)
    {
        SaveData data = new()
        {
            playerPosX = spawnPos.position.x,
            playerPosY = spawnPos.position.y,
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

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player == null)
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
            player.transform.position = new Vector2(data.playerPosX, data.playerPosY);

            Debug.Log("Loaded game in area: " + savedAreaName);
        }
        else
        {
            Debug.LogWarning("AreaData not found for saved area: " + savedAreaName);
        }
    }
}
