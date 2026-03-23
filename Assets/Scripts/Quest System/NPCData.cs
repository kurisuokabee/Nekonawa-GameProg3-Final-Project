using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewNPCData", menuName = "NPC/NPC Data")]
public class NPCData : ScriptableObject
{   
    [Header("Quest Info")]
    public Quest quest;
    
    [Header("Basic Info")]
    public string NPCName;
    public Sprite NPCImage;

    [Header("Dialogue")]
    public List<string> StartingDialogue = new();
    public List<string> FinishingDialogue = new();
}
