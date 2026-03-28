using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public PlayerHealth Health { get; private set; }
    public PlayerQuests Quests { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerShield Shield { get; private set; }
    public AbilitySystem Abilities { get; private set; }
    public GameObject PlayerGO { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        PlayerGO = GameObject.FindGameObjectWithTag("Player");

        Health = PlayerGO.GetComponent<PlayerHealth>();
        Quests = PlayerGO.GetComponent<PlayerQuests>();
        Abilities = PlayerGO.GetComponent<AbilitySystem>();
        Movement = PlayerGO.GetComponent<PlayerMovement>();
        Shield = PlayerGO.GetComponent<PlayerShield>();

        
    }
}