using UnityEngine;

[CreateAssetMenu(fileName = "New AreaData", menuName = "Game/Area Data")]
public class AreaData : ScriptableObject
{
    public string areaName;              
    public GameObject cameraBoundsPrefab;   
}
