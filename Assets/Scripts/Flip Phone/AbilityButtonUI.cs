using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButtonUI : MonoBehaviour
{
    public Button button;
    [SerializeField] TextMeshProUGUI text;

    public void Setup(PlayerAbility ability, AbilityUIManager manager)
    {
        text.text = ability.abilityName.ToString();    

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => manager.SelectAbility(ability));
    }

}