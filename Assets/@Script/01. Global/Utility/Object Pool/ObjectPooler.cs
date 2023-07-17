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

    public void Initialize(Transform rootTransform, string key = null, int amount = 0)
    {
        this.key = key;
        this.amount = amount;
        sampleObject = Managers.ResourceManager.LoadResourceSync<GameObject>(key);
        for (int i = 0; i < amount; ++i)
            RegistObject(rootTransform);
    }

    public GameObject RegistObject(Transform rootTransform)
    {
        GameObject poolObject = Managers.ResourceManager.InstantiatePrefabSync(key, rootTransform);
        if(poolObject != null)
        {
            ++amount;
            ReturnObject(poolObject);
        }
        else
        {
            Debug.Log("Fail to Regist Object");
        }

        return poolObject;
    }

    public void ReturnObject(GameObject enqueueObject)
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
    [SerializeField] private LinkedList<ObjectPool> objectPoolLinkedList = new LinkedList<ObjectPool>();

    public void Initialize(Transform rootTransform)
    {
        this.rootTransform = rootTransform;
        foreach (ObjectPool objectPool in objectPoolLinkedList)
        {
            objectPool.Initialize(this.rootTransform);
            objectPoolDictionary.Add(objectPool.key, objectPool);
        }
    }

    public void RegistObject(string key, int amount)
    {
        if (objectPoolDictionary.TryGetValue(key, out ObjectPool objectPool))
        {
            for (int i = 0; i < amount; ++i)
                objectPool.RegistObject(rootTransform);
        }
        else
        {
            ObjectPool newObjectPool = new ObjectPool();
            newObjectPool.Initialize(rootTransform, key, amount);
            objectPoolLinkedList.AddLast(newObjectPool);
            objectPoolDictionary.Add(key, newObjectPool);
        }
    }

    public GameObject RequestObject(string key)
    {
        // Has Not Object Pool
        if (!objectPoolDictionary.ContainsKey(key))
        {
            RegistObject(key, 1);
        }

        // Object Pool is Empty
        if (objectPoolDictionary[key].queue.TryDequeue(out GameObject requestObject))
        {
            requestObject.transform.SetParent(null, false);
            requestObject.SetActive(true);

            if (requestObject.TryGetComponent(out IPoolObject poolObject))
                poolObject.ActionAfterRequest(this);

            return requestObject;
        }
        else
        {
            objectPoolDictionary[key].RegistObject(rootTransform);
            return RequestObject(key);
        }
    }

    public void ReturnObject(string key, GameObject returnObject)
    {
        if(returnObject.TryGetComponent(out IPoolObject poolObject))
            poolObject.ActionBeforeReturn();

        returnObject.transform.SetParent(rootTransform, false);
        objectPoolDictionary[key].ReturnObject(returnObject);
    }
}
