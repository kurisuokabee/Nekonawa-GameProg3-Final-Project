using UnityEngine;

public enum PhoneMode
{
    Sword,
    Gun,
    Default
}

public class FlipPhone : MonoBehaviour
{
    public static FlipPhone Instance;
    public PhoneMode currentMode;

    public GameObject swordObj;
    public GameObject gunObj;

    private IPhone currentWeapon;

    [Header("Mode Switching Cooldown")]
    [SerializeField] private float switchCooldown;
    private float lastSwitchTime = -Mathf.Infinity;       // so player can switch immediately at start

    void Awake()
    {
        Instance = this;
        
    }

    void Start()
    {
        SetMode(PhoneMode.Sword);
    }

    void Update()
    {
        HandleSwitch();

        if (Input.GetKey(KeyCode.Mouse0))
        {
            currentWeapon?.Use();
        }
    }

    void HandleSwitch()
    {
        // Only allow switching if enough time has passed
        if (Time.time - lastSwitchTime < switchCooldown)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetMode(PhoneMode.Sword);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetMode(PhoneMode.Gun);
    }

    void SetMode(PhoneMode mode)
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
            case PhoneMode.Sword:
                selected = swordObj;
                break;
            case PhoneMode.Gun:
                selected = gunObj;
                break;
        }

        selected.SetActive(true);
        currentWeapon = selected.GetComponent<IPhone>();

        // Enter new weapon
        currentWeapon?.Enter();

        lastSwitchTime = Time.time;
    }
}