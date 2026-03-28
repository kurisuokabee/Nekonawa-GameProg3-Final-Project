using UnityEngine;

public enum ObjectType
{
    Bullet,
    Dialogue
}

public class ObjectFactory : MonoBehaviour
{   
    public static ObjectFactory Instance { get; private set; }
    [SerializeField] PoolManager poolManager;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    public GameObject SpawnObject(ObjectType type, Vector3 position, Quaternion rotation)
    {   
        //Creates an object from the pool
        GameObject obj = poolManager.Get(type);
        Debug.Log("Spawning " + type.ToString());

        // Set position and rotation
        obj.transform.SetPositionAndRotation(position, rotation);
        
        return obj;
    }

    public void Release(ObjectType type, GameObject obj)
    {
        poolManager.Release(type, obj);
    }
}
