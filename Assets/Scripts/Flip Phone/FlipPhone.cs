using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public enum FlipPhoneMode
{
    Sword,
    Gun,
    FlipPhone
}

public class FlipPhone : MonoBehaviour
{
    public static FlipPhone Instance;
    [SerializeField] private FlipPhoneMode currentMode;

    [SerializeField] private GameObject swordObj;
    [SerializeField] private GameObject gunObj;
    [SerializeField] private GameObject phoneObj;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] private GameObject[] blockAttackUI;
    [SerializeField] AudioClip audioClip;
    [SerializeField] Animator animator;
    private IPhone currentWeapon;

    [Header("Mode Switching Cooldown")]
    [SerializeField] private float switchCooldown;
    private float lastSwitchTime = -Mathf.Infinity;       

    void Awake()
    {
        Instance = this;
        
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

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SetFlipPhoneMode(FlipPhoneMode.FlipPhone);    
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
        phoneObj.SetActive(false);

        GameObject selected = null;

        switch (mode)
        {
            case FlipPhoneMode.Sword:
                selected = swordObj;
                text.text = "Sword Mode";
                animator.SetInteger("WeaponType", 1);
                break;
            case FlipPhoneMode.Gun:
                selected = gunObj;
                text.text = "Gun Mode";
                animator.SetInteger("WeaponType", 2);
                break;  
            case FlipPhoneMode.FlipPhone:
                selected = phoneObj;
                text.text = "Flip Phone";
                animator.SetInteger("WeaponType", 0);   
                break;  
        }

        selected.SetActive(true);
        currentWeapon = selected.GetComponent<IPhone>();

        // Enter new weapon
        currentWeapon?.Enter(); 
        //SoundFXManager.Instance.PlaySound(audioClip, transform, 1f);

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