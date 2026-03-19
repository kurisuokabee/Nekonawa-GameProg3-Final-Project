using UnityEngine;

public static class Utilities 
{   
    static LayerMask clickableLayer = LayerMask.GetMask("Clickable");

    //Getting the gameobject of the clicked object
    public static GameObject GetClickedObject()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 0f, clickableLayer);

        if (hit.collider != null)
            return hit.collider.gameObject;

        return null;
    }

    //Returns true if player clicked an object
    public static bool HitTarget()
    {
        return GetClickedObject() != null;
    }

    public static void DisablePlayerControls()
    {
        FlipPhone.Instance.enabled = false;
        AimAtMouse.Instance.enabled = false;
        AbilitySystem.Instance.enabled = false;
    }

    public static void EnablePlayerControls()
    {
        FlipPhone.Instance.enabled = true;
        AimAtMouse.Instance.enabled = true;
        AbilitySystem.Instance.enabled = true;
    }
}
