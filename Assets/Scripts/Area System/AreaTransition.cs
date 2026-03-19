using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class AreaTransition : MonoBehaviour
{
    [SerializeField] private Transform targetSpawnPoint;
    [SerializeField] private AreaData areaData;
    PlayerHealth player;
    PlayerQuests playerQuests;
    private CinemachineCamera virtualCamera; 

    void Awake()
    {
        virtualCamera = GameObject.Find("Player CAM").GetComponent<CinemachineCamera>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {   
            StartCoroutine(TeleportPlayer(other));
        }
    }

    private IEnumerator TeleportPlayer(Collider2D other)
    {
        Debug.Log("Area Switched");

        // Show loading screen
        LoadingScreen.Instance.ShowLoading();

        // Wait for fade duration
        yield return new WaitForSecondsRealtime(0.8f);

        // Teleport player and reset velocity
        Vector3 oldPosition = other.transform.position;
        other.transform.position = targetSpawnPoint.position;
        other.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        // Prevents Camera Lag
        if (virtualCamera != null)
        {
            virtualCamera.OnTargetObjectWarped(
                other.transform,
                targetSpawnPoint.position - oldPosition
            );

        }

        // --- Update Current Area ---
        AreaManager.Instance.SetCurrentArea(areaData);

        player = other.GetComponent<PlayerHealth>();
        playerQuests = other.GetComponent<PlayerQuests>();
        // --- Save Current Area ---
        GameController.Instance.SaveGame(targetSpawnPoint, areaData.areaName, playerQuests.keysCollected, player.currentHealth);
        Debug.Log("Current Area Updated: " + areaData.areaName);
    }
}
