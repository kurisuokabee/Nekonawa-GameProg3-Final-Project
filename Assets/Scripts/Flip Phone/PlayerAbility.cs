using UnityEngine;
using System.Collections;

public enum AbilityName
{
    Heal,
    Speed,
    Shield
}


[System.Serializable]
public class PlayerAbility
{
    public AbilityName utilityName;
    public KeyCode key;        
    public float cooldown = 5f;

    private bool onCooldown = false;

    // The actual effect
    public void Use(GameObject user)
    {
        if (onCooldown)
        {   
            Debug.Log($"{utilityName} is on cooldown!");
            return;
        } 

        Debug.Log($"Used {utilityName}!");

        
        switch (utilityName)
        {
            case AbilityName.Heal:
                user.GetComponent<PlayerHealth>().Heal(20);
                break;
            case AbilityName.Speed:
                user.GetComponent<PlayerMovement>().BoostSpeed(2f, 3f);
                break;
            case AbilityName.Shield:
                // user.GetComponent<PlayerShield>().Activate(3f);
                break;
        }

        // Start cooldown
        user.GetComponent<MonoBehaviour>().StartCoroutine(StartCooldown());
    }

    private IEnumerator StartCooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }

    public bool IsReady() => !onCooldown;
}