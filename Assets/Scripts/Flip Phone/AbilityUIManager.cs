using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUIManager : MonoBehaviour
{   
    public static AbilityUIManager Instance;
    [SerializeField] GameObject panel;
    [SerializeField] TextMeshProUGUI slot1Text;
    [SerializeField] TextMeshProUGUI slot2Text;
    [SerializeField] GameObject[] abilityButtons;

    int currentSlotIndex; // 0 = slot1, 1 = slot2
    AbilitySystem abilitySystem;

    void Awake()
    {
        Instance = this;
        
    }

    void Start()
    {
        abilitySystem = PlayerManager.Instance.Abilities;
    }

    // Called when clicking Slot1 or Slot2 button
    public void OpenPanel(int slotIndex)
    {
        currentSlotIndex = slotIndex;
        panel.SetActive(true);

        PopulateAbilitiesToPanel();
    }

    void PopulateAbilitiesToPanel()
    {
        int index = 0;

        foreach (var ability in abilitySystem.abilities)
        {   
            if (!ability.isUnlocked) continue; // skip the locked abilities

            if (index >= abilityButtons.Length)
                break;

            GameObject buttonGO = abilityButtons[index];
            buttonGO.SetActive(true);

            // Setup the ability data and manager to the button
            var ui = buttonGO.GetComponent<AbilityButtonUI>();
            ui.Setup(ability, this);
            
            // Disable if already equipped
            ui.button.interactable =
            ability != abilitySystem.slot1 && ability != abilitySystem.slot2;

            index++;
        }

        // Disable unused buttons
        for (int i = index; i < abilityButtons.Length; i++)
        {
            abilityButtons[i].SetActive(false);
        }
    }

    public void UpdateSlotUI()
    {   
        //Ability Slot 1 Text
        slot1Text.text = "Q: " + (
            abilitySystem.slot1 != null 
            ? abilitySystem.slot1.abilityName.ToString() 
            : "Empty"
        );

        //Ability Slot 2 Text
        slot2Text.text = "E: " + (
            abilitySystem.slot2 != null 
            ? abilitySystem.slot2.abilityName.ToString() 
            : "Empty"
        );
    }

    public void SelectAbility(PlayerAbility ability)
    {   
        abilitySystem.EquipAbility(ability, currentSlotIndex);

        UpdateSlotUI();

        panel.SetActive(false);
    }

    public void CloseAbilitySwitchingPanel()
    {
        panel.SetActive(false);
    }

}