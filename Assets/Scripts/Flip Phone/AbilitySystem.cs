using System.Collections.Generic;
using UnityEngine;
public enum AbilityName
{
    Heal,
    Speed,
    Shield
}

public class AbilitySystem : MonoBehaviour
{
    public static AbilitySystem Instance;

    [Header("Unlocked abilities")]
    public List<PlayerAbility> abilities = new();

    [Header("Equipped (ONLY 2 usable)")]
    public PlayerAbility slot1;
    public PlayerAbility slot2;

    PlayerManager playerManager;

    void Awake()
    {
        Instance = this;

        playerManager = PlayerManager.Instance;
    }

    void Update()
    {
        // SLOT 1 -> Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            slot1?.Use(gameObject);
        }

        // SLOT 2 -> E
        if (Input.GetKeyDown(KeyCode.E))
        {
            slot2?.Use(gameObject);
        }
    }

    public void EquipAbility(PlayerAbility ability, int slotIndex)
    {
        if (!ability.isUnlocked) return;

        // prevent duplicate
        if (slot1 == ability || slot2 == ability)
            return;

        if (slotIndex == 0)
            slot1 = ability;
        else if (slotIndex == 1)
            slot2 = ability;
    }
}