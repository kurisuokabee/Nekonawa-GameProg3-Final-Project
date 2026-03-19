using UnityEngine;

public class AbilitySystem : MonoBehaviour
{   
    public static AbilitySystem Instance;
    public PlayerAbility[] abilities;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        foreach (var ability in abilities)
        {
            if (Input.GetKeyDown(ability.key))
            {
                ability.Use(gameObject);
            }
        }
    }
}