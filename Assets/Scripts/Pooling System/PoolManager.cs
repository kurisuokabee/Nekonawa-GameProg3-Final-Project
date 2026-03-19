using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    public class PoolData
    {
        public ObjectType objectType;
        public GameObject prefab;
        public Transform objectParent;
    }

    public List<PoolData> pools;
    
    Dictionary<ObjectType, ObjectPool<GameObject>> poolDictionary;

    void Awake()
    {   
        poolDictionary = new Dictionary<ObjectType, ObjectPool<GameObject>>();

        //Create a pool for each pool datas with the four core callbacks.
        foreach (PoolData poolData in pools)
        {
            ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
                () => CreateItem(poolData.prefab, poolData.objectParent),
                obj => OnGet(obj),
                obj => OnRelease(obj),
                obj => OnDestroyItem(obj),
                true,
                defaultCapacity: 10, 
                maxSize: 50
            );

            poolDictionary.Add(poolData.objectType, pool);
        }
    }

    // Creates a new pooled GameObject the first time (and whenever the pool needs more).
    private GameObject CreateItem(GameObject prefab, Transform parent)
    {
        GameObject gameObject = Instantiate(prefab, parent);
    
        gameObject.SetActive(false);
        return gameObject;
    }

    // Called when an item is taken from the pool.
    private void OnGet(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    // Called when an item is returned to the pool.
    private void OnRelease(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    // Called when the pool decides to destroy an item (e.g., above max size).
    private void OnDestroyItem(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    public GameObject Get(ObjectType type) 
    { 
        return poolDictionary[type].Get();
    }

    public void Release(ObjectType type, GameObject obj)
    {
        poolDictionary[type].Release(obj);
    }
}
