using UnityEngine;

public enum ObjectType
{
    Bullet,
    Effects
}

public class ObjectFactory : MonoBehaviour
{
    [SerializeField] PoolManager poolManager;

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
