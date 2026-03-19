using System;
using UnityEngine;

public static class Utilities 
{   
    static LayerMask clickableLayer = LayerMask.GetMask("Clickable");
    static Transform player = GameObject.FindGameObjectWithTag("Player").transform;

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

    public static void MoveTowardsPlayer(Transform transform)
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.transform.position,
            8f * Time.deltaTime
        );
    }

    public static bool IsCloseToPlayer(Transform transform)
    {
        return (player.transform.position - transform.position).sqrMagnitude <= 0.25f;
    }

    public static PlayerManager Player => PlayerManager.Instance;

}
