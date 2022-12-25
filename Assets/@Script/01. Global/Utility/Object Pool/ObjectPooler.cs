using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool
{
    public string key;
    public int amount;
    public GameObject sampleObject;
    public Queue<GameObject> queue = new Queue<GameObject>();

    public void Initialize(Transform rootTransform)
    {
        sampleObject = Managers.ResourceManager.LoadResourceSync<GameObject>(key);
        for (int i = 0; i < amount; ++i)
            CreateObject(rootTransform);
    }
    public void Initialize(Transform rootTransform, string key, int amount)
    {
        this.key = key;
        this.amount = amount;
        Initialize(rootTransform);
    }

    public GameObject CreateObject(Transform rootTransform)
    {
        GameObject poolObject = Managers.ResourceManager.InstantiatePrefabSync(key, rootTransform);
        poolObject.SetActive(false);
        Enqueue(poolObject);

        return poolObject;
    }

    public void Enqueue(GameObject enqueueObject)
    {
        enqueueObject.transform.SetLocalPositionAndRotation(sampleObject.transform.position, sampleObject.transform.rotation);
        enqueueObject.SetActive(false);
        queue.Enqueue(enqueueObject);
    }
}
[System.Serializable]
public class ObjectPooler
{
    private Transform rootTransform;
    private Dictionary<string, ObjectPool> objectPoolDictionary = new Dictionary<string, ObjectPool>();
    [SerializeField] private List<ObjectPool> objectPoolList;

    public void Initialize(Transform rootTransform)
    {
        this.rootTransform = rootTransform;
        for (int i = 0; i < objectPoolList.Count; ++i)
        {
            objectPoolList[i].Initialize(this.rootTransform);
            objectPoolDictionary.Add(objectPoolList[i].key, objectPoolList[i]);
        }
    }

    public void RegisterObject(string key, int amount)
    {
        if (objectPoolDictionary.TryGetValue(key, out ObjectPool objectPool))
        {
            for (int i = 0; i < amount; ++i)
                objectPool.CreateObject(rootTransform);
        }
        else
        {
            ObjectPool newObjectPool = new ObjectPool();
            newObjectPool.Initialize(rootTransform, key, amount);
            objectPoolList.Add(newObjectPool);
            objectPoolDictionary.Add(key, newObjectPool);
        }
    }

    public GameObject RequestObject(string key)
    {
        if (objectPoolDictionary.ContainsKey(key) == false)
            RegisterObject(key, 1);

        if (objectPoolDictionary[key].queue.TryDequeue(out GameObject requestObject) == false)
            requestObject = objectPoolDictionary[key].CreateObject(rootTransform);

        requestObject.transform.SetParent(null, false);
        requestObject.SetActive(true);

        if (requestObject.TryGetComponent(out IPoolObject poolObject))
            poolObject.ActionAfterRequest(this);

        return requestObject;
    }

    public void ReturnObject(string key, GameObject returnObject)
    {
        if(returnObject.TryGetComponent(out IPoolObject poolObject))
            poolObject.ActionBeforeReturn();

        returnObject.transform.SetParent(rootTransform, false);
        objectPoolDictionary[key].Enqueue(returnObject);
    }
}
