using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class AreaTransition : MonoBehaviour
{   
    public static AreaTransition Instance;
    [SerializeField] private Transform targetSpawnPoint;            
    [SerializeField] private AreaData areaData;
    private CinemachineCamera virtualCamera; 

    void Awake()
    {   
        Instance = this;
        virtualCamera = Utilities.virtualCamera;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {   
            StartCoroutine(TeleportPlayer(other.transform));
        }
    }

    public IEnumerator TeleportPlayer(Transform other)
    {
        Debug.Log("Area Switched");

        PlayerMovement.Instance.DisableMovement();
        // Show loading screen
        LoadingScreen.Instance.ShowLoading();

        // Wait for fade duration
        yield return new WaitForSecondsRealtime(1.5f);

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
        AreaManager.Instance.SetAreaVariables(areaData, targetSpawnPoint.position);

        // --- Save Current Area ---
        GameController.Instance.SaveGame();
        Debug.Log("Current Area Updated: " + areaData.areaName);

        PlayerMovement.Instance.EnableMovement();

        if(areaData.areaName == "Boss")
        {   
            StoryManager.Instance.EnterState(StoryState.BossArea);
        }
    }
}
