using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public enum FlipPhoneMode
{
    Sword,
    Gun,
    Default
}

public class FlipPhone : MonoBehaviour
{
    public static FlipPhone Instance;
    [SerializeField] private FlipPhoneMode currentMode;

    [SerializeField] private GameObject swordObj;
    [SerializeField] private GameObject gunObj;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] private GameObject[] blockAttackUI;
    private IPhone currentWeapon;

    [Header("Mode Switching Cooldown")]
    [SerializeField] private float switchCooldown;
    private float lastSwitchTime = -Mathf.Infinity;       

    void Awake()
    {
        Instance = this;
        
    }

    void Start()
    {
        SetFlipPhoneMode(FlipPhoneMode.Sword);
    }

    void Update()
    {
        HandleSwitch();

        if (IsCursorOverSpecificUI(blockAttackUI))
        return; // Don't attack

        if (Input.GetKey(KeyCode.Mouse0))
        {
            currentWeapon?.Use();
        }
    }

    void HandleSwitch()
    {
        // Weapon Switch Cooldown
        if (Time.time - lastSwitchTime < switchCooldown)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetFlipPhoneMode(FlipPhoneMode.Sword);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetFlipPhoneMode(FlipPhoneMode.Gun);
    }

    void SetFlipPhoneMode(FlipPhoneMode mode)
    {
        if (currentMode == mode)
        return;

        // Exit previous weapon
        currentWeapon?.Exit();

        currentMode = mode;

        swordObj.SetActive(false);
        gunObj.SetActive(false);

        GameObject selected = null;

        switch (mode)
        {
            case FlipPhoneMode.Sword:
                selected = swordObj;
                text.text = "Sword Mode";
                break;
            case FlipPhoneMode.Gun:
                selected = gunObj;
                text.text = "Gun Mode";
                break;
        }

        selected.SetActive(true);
        currentWeapon = selected.GetComponent<IPhone>();

        // Enter new weapon
        currentWeapon?.Enter();

        lastSwitchTime = Time.time;
    }

    bool IsCursorOverSpecificUI(GameObject[] uiObjects)
    {
        PointerEventData pointerData = new(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult r in results)
        {
            foreach (var uiObj in uiObjects)
            {
                if (r.gameObject == uiObj)
                    return true; // Mouse is over a specific UI element
            }
        }
        return false;
    }
}