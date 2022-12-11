using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    [Header("Pool Object")]
    [SerializeField] private ObjectPoolController owner;
    [SerializeField] private string key;
    [SerializeField] private float returnTime;

    public virtual void Initialize(ObjectPoolController owner)
    {
        this.owner = owner;
    }

    public virtual void OnEnable()
    {
        StartCoroutine(AutoReturn(key, returnTime));
    }

    public IEnumerator AutoReturn(string key, float returnTime)
    {
        yield return new WaitForSeconds(returnTime);
        owner.ReturnObject(key, gameObject);
    }
}
