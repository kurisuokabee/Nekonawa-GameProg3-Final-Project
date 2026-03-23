using UnityEngine;
using Unity.Cinemachine;

public class AreaManager : MonoBehaviour
{
    public static AreaManager Instance;

    private CinemachineCamera virtualCamera;

    private CinemachineConfiner2D confiner;

    public AreaData CurrentArea { get; private set; }
    public Vector2 LastSpawnPos { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

    }

    void Start()
    {
        virtualCamera = Utilities.virtualCamera;
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

    public void SetAreaVariables(AreaData newArea, Vector2 spawnPos)
    {
        CurrentArea = newArea;
        LastSpawnPos = spawnPos;
    }
}
