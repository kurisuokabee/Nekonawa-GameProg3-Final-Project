using UnityEngine;

public class AimAtMouse : MonoBehaviour
{   
    public static AimAtMouse Instance;
    void Awake()
    {
        Instance = this;
    }
    
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector3 direction = mousePos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}