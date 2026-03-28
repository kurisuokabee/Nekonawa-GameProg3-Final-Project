using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Abilities/Player Ability")]
public class PlayerAbility : ScriptableObject
{
    public AbilityName abilityName;    
    public float cooldown;
    public bool isUnlocked = false;

    public bool onCooldown = false;

    public void Use(GameObject user)
    {
        if (!isUnlocked || onCooldown) return;

        Debug.Log($"Used {abilityName}!");

        //Activate ability
        Activate();

        //Start ability cooldown
        user.GetComponent<MonoBehaviour>().StartCoroutine(StartCooldown());
    }

    void Activate()
    {   
        var player = Utilities.Player;

        int healAmount = 20;

        float speedMultiplier = 1.5f;
        float speedBoostDuration = 3f;
        
        //float shieldDuration = 3f;
        switch (abilityName)
        {
            case AbilityName.Heal:
                player.Health.Heal(healAmount);
                break;
            case AbilityName.Speed:
                player.Movement.BoostSpeed(speedMultiplier, speedBoostDuration);
                break;
            case AbilityName.Shield:
                player.Shield.ActivateShield();
                break;
        }
    }
    IEnumerator StartCooldown()
    {
        onCooldown = true;
        yield return new WaitForSecondsRealtime(cooldown);
        onCooldown = false;
    }

}