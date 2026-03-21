using System.Collections.Generic;

[System.Serializable]
public class SaveData 
{
    public float playerPosX;
    public float playerPosY;
    public int playerKeys;
    public int playerCurrentHealth;
    public List<PlayerAbility> playerAbilities;
    public PlayerAbility slot1;
    public PlayerAbility slot2;
    public string currentAreaName;

    public SaveData()
    {
        playerPosX = 0f;               // starting X position
        playerPosY = 0f;               // starting Y position
        playerKeys = 0;                // player starts with 0 keys
        playerCurrentHealth = 100;     // default health
        playerAbilities = null;       // empty list
        slot1 = null;
        slot2 = null;
        currentAreaName = "Limbo"; // default area
    }
}
