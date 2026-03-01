using UnityEngine;

public static class Utilities 
{   
    static LayerMask clickableLayer = LayerMask.GetMask("Clickable");

    public static GameObject GetClickedObject()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 0f, clickableLayer);

        if (hit.collider != null)
            return hit.collider.gameObject;

        return null;
    }

    public static bool HitTarget()
    {
        return GetClickedObject() != null;
    }
}
