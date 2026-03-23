using UnityEngine;

[CreateAssetMenu(fileName = "New AreaData", menuName = "Area/Area Data")]
public class AreaData : ScriptableObject
{
    public string areaName;              
    public GameObject cameraBoundsPrefab;   
}
