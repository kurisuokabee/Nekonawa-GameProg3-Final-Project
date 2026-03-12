[System.Serializable]
public class SaveData 
{
    public float playerPosX;
    public float playerPosY;
    public int playerKeys;
    public int playerCurrentHealth;
    public string currentAreaName;

    public SaveData()
    {
        playerPosX = 0f;               // starting X position
        playerPosY = 0f;               // starting Y position
        playerKeys = 0;                // player starts with 0 keys
        playerCurrentHealth = 100;     // default health
        currentAreaName = "Limbo"; // default area
    }
}
