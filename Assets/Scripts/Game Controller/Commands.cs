using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseCommand : ICommand
{
    GameController controller;
    GameObject pausedPanel;

    public PauseCommand(GameController controller, GameObject pausedPanel)
    {
        this.controller = controller;
        this.pausedPanel = pausedPanel;
    }

    public void Execute()
    {   
        if (Time.timeScale > 0)
        {
            controller.ChangeState(controller.PausedState);
            pausedPanel.SetActive(true);
        }
        else
        {
            controller.ChangeState(controller.PlayingState);
            pausedPanel.SetActive(false);
        }
            
    }
}

public class RestartCommand : ICommand
{   
    GameController controller;
    GameObject gameoverPanel;

    public RestartCommand(GameController controller, GameObject gameoverPanel)
    {
        this.controller = controller;
        this.gameoverPanel = gameoverPanel;
    }

    public void Execute()
    {
        Time.timeScale = 1f;
        gameoverPanel.SetActive(false);
        controller.LoadGame();
        controller.ChangeState(controller.PlayingState);
    }
}




