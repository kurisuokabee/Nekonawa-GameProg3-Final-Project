using System.Collections.Generic;
using UnityEngine;

public enum SpeakerName
{
    Gerald,
    Fern,
    Unknown,
    FlipPhone,
    Lucifer
}
[System.Serializable]
public class DialogueBlock
{
    public StoryState blockName;          
    public List<DialogueLine> lines;
}

[System.Serializable]
public class DialogueLine
{
    public SpeakerName speakerName;
    
    [TextArea(3, 10)]
    public string line;
}

[CreateAssetMenu(fileName = "NewStoryDialogue", menuName = "Dialogue/Dialogue Data")]
public class StoryDialogueData : ScriptableObject
{
    public List<DialogueBlock> blocks;

    // Helper to get block by name
    public DialogueBlock GetBlock(StoryState name)
    {
        return blocks.Find(b => b.blockName == name);
    }
}
