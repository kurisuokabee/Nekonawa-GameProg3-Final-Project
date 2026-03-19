using UnityEngine;

public class AbilityUnlockerItem : MonoBehaviour
{
    [SerializeField] PlayerAbility abilityToUnlock;

    AbilitySystem abilitySystem;
    bool goToPlayer;

    void Start()
    {
        abilitySystem = PlayerManager.Instance.Abilities;
    }

    void Update()
    {
        if (!goToPlayer) return;

        // Move towards the player
        Utilities.MoveTowardsPlayer(transform);

        if(Utilities.IsCloseToPlayer(transform))
        {   
            abilityToUnlock.isUnlocked = true;
            abilitySystem.abilities.Add(abilityToUnlock);

            //Auto Equip Abilities on unlock for the 1st time
            if (abilitySystem.slot1 == null)
            {
                abilitySystem.EquipAbility(abilityToUnlock, 0);
                AbilityUIManager.Instance.UpdateSlotUI();
            }     
            else if (abilitySystem.slot2 == null)
            {
                abilitySystem.EquipAbility(abilityToUnlock, 1);
                AbilityUIManager.Instance.UpdateSlotUI();
            }
    
            Debug.Log(abilityToUnlock.abilityName + " UNLOCKED!");
            
            
            gameObject.SetActive(false);
        }
    
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            goToPlayer = true;
        }
    }
}
