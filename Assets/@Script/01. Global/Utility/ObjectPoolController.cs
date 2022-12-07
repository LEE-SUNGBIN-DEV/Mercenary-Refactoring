using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool
{
    public string key;
    public int amount;
    public GameObject value;
    public Queue<GameObject> queue = new Queue<GameObject>();

    public void Initialize(GameObject root)
    {
        for (int i = 0; i < amount; ++i)
        {
            GameObject poolObject = Managers.ResourceManager.InstantiatePrefabSync(key, root.transform);
            poolObject.SetActive(false);
            queue.Enqueue(poolObject);
        }
    }
}
[System.Serializable]
public class ObjectPoolController
{
    private Dictionary<string, ObjectPool> objectPoolDictionary = new Dictionary<string, ObjectPool>();
    [SerializeField] private ObjectPool[] objectPools;

    public void Initialize(GameObject root)
    {
        for (int i = 0; i < objectPools.Length; ++i)
        {
            objectPools[i].Initialize(root);
            objectPoolDictionary.Add(objectPools[i].key, objectPools[i]);
        }
    }
    public GameObject RequestObject(string key)
    {
        GameObject requestObject = objectPoolDictionary[key].queue.Dequeue();
        requestObject.SetActive(true);
        return requestObject;
    }
    public void ReturnObject(string key, GameObject returnObject)
    {
        returnObject.SetActive(false);
        objectPoolDictionary[key].queue.Enqueue(returnObject);
    }
}
