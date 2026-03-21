using UnityEngine;
public class PlayingState : IGameState
{   
    GameController controller;
    StatesData statesData;
    public PlayingState(GameController controller, StatesData statesData)
    {
        this.controller = controller;
        this.statesData = statesData;
    }

    public void Enter()
    {
        Debug.Log("Playing State");
        Time.timeScale = 1f;

        //Clear commands
        controller.InputHandler.ClearCommands();

        //Set a command for Pausing
        controller.InputHandler.SetCommand(
            KeyCode.Escape,
            new PauseCommand(controller, statesData.pausedPanel)
        );
        
        Utilities.EnablePlayerControls();
    }

    public void Exit()
    {
        Debug.Log("Leaving Playing State");
        Utilities.DisablePlayerControls();
    }

    public void Tick()
    {
        controller.InputHandler.HandleInput();

        //Triggers Gameover for testing
        // if (Input.GetKeyDown(KeyCode.K))
        // {
        //     controller.ChangeState(controller.GameOverState);
        // }
    }
}

public class PausedState : IGameState
{   
    GameController controller;
    StatesData statesData;

    public PausedState(GameController controller, StatesData statesData)
    {
        this.controller = controller;
        this.statesData = statesData;
    }

    public void Enter()
    {
        Debug.Log("Game Paused");
        Time.timeScale = 0f;

        controller.InputHandler.ClearCommands();

        //Set a command for resuming
        controller.InputHandler.SetCommand(
            KeyCode.Escape,
            new PauseCommand(controller, statesData.pausedPanel)
        );
    }

    public void Exit()
    {
        Debug.Log("Game Resuming");
    }

    public void Tick()
    {
        controller.InputHandler.HandleInput();
    }
}

public class GameOverState : IGameState
{   
    GameController controller;
    StatesData statesData;

    public GameOverState(GameController controller, StatesData statesData)
    {
        this.controller = controller;
        this.statesData = statesData;
    }

    public void Enter()
    {
        Debug.Log("Game Over");
        Time.timeScale = 0f;

        statesData.gameoverPanel.SetActive(true);
        
        controller.InputHandler.ClearCommands();

        //Set a command for restarting
        controller.InputHandler.SetCommand(
            KeyCode.R,
            new RestartCommand(controller, statesData.gameoverPanel)
        );
    }

    public void Exit()
    {
        Debug.Log("Restarting");
    }

    public void Tick()
    {
        controller.InputHandler.HandleInput();
    }
}