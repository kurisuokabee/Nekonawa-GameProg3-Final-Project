using UnityEngine;
using Unity.Cinemachine;

public class AreaManager : MonoBehaviour
{
    public static AreaManager Instance;

    private CinemachineCamera virtualCamera;

    private CinemachineConfiner2D confiner;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        virtualCamera = GameObject.Find("Player CAM").GetComponent<CinemachineCamera>();
        confiner = virtualCamera.GetComponent<CinemachineConfiner2D>();
    }

    public void SetCurrentArea(AreaData newArea)
    {
        Collider2D collider = newArea.cameraBoundsPrefab.GetComponent<Collider2D>();
        if (confiner != null && collider != null)
        {
            confiner.BoundingShape2D = collider;
            confiner.InvalidateBoundingShapeCache();
        }
    }
}
