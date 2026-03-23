using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public static class Utilities 
{   
    static LayerMask clickableLayer;
    public static PlayerManager Player => PlayerManager.Instance;
    public static ObjectFactory Factory => ObjectFactory.Instance;
    public static DialogueUIManager DialogueUIManager => DialogueUIManager.Instance;
    public static StoryManager StoryManager => StoryManager.Instance;
    public static CinemachineCamera virtualCamera;
    public static List<QuestGiverNPC> AllNPCs = new();

    public static void InitUtilities()
    {
        clickableLayer = LayerMask.GetMask("Clickable");
        virtualCamera = GameObject.Find("Player CAM").GetComponent<CinemachineCamera>();
    }
    
    //Getting the gameobject of the clicked object
    public static GameObject GetClickedObject()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 0f, clickableLayer);

        if (hit.collider != null)
            return hit.collider.gameObject;

        return null;
    }

    //Returns true if the player clicked an object that corresponds to a specific component
    public static bool HitTarget<T>() where T : Component
    {
        GameObject obj = GetClickedObject();
        if (obj == null) return false;

        
        if (obj.GetComponent<T>() != null)
            return true;

        return obj.GetComponentInParent<T>() != null;
    }

    public static void DisablePlayerControls()
    {
        FlipPhone.Instance.enabled = false;
        AimAtMouse.Instance.enabled = false;
        AbilitySystem.Instance.enabled = false;
        Player.Movement.DisableMovement();
    }

    public static void EnablePlayerControls()
    {
        FlipPhone.Instance.enabled = true;
        AimAtMouse.Instance.enabled = true;
        AbilitySystem.Instance.enabled = true;
        Player.Movement.EnableMovement();
    }

    public static void MoveTowardsPlayer(Transform transform)
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            Player.transform.position,
            8f * Time.deltaTime
        );
    }

    public static bool IsCloseToPlayer(Transform transform)
    {
        return (Player.transform.position - transform.position).sqrMagnitude <= 1f;
    }

    public static float NPCDistanceToPlayer(Transform transform)
    {
        return (Player.transform.position - transform.position).sqrMagnitude;
    }

    

    

}
