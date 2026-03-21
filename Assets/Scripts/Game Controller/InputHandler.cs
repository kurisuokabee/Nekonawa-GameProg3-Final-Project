using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private Dictionary<KeyCode, ICommand> keyCommands = new();

    public void SetCommand(KeyCode key, ICommand command)
    {
        keyCommands[key] = command;
    }

    public void ClearCommands()
    {
        keyCommands.Clear();
    }

    public void HandleInput()
    {
        var keys = new List<KeyCode>(keyCommands.Keys);

        foreach (var key in keys)
        {
            if (Input.GetKeyDown(key))
            {
                keyCommands[key].Execute();
            }
        }
    }
}
