using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoReturnObject : MonoBehaviour, IPoolObject
{
    [Header("Auto Return")]
    [SerializeField] private float duration;
    private IEnumerator autoReturnCoroutine;
    private ObjectPooler objectPooler;

    public IEnumerator CoAutoReturn()
    {
        yield return new WaitForSeconds(duration);
        ReturnOrDestoryObject(objectPooler);
    }

    #region IPoolObject Interface Fucntion
    public void ActionAfterRequest(ObjectPooler owner)
    {
        objectPooler = owner;
        autoReturnCoroutine = CoAutoReturn();

        if (autoReturnCoroutine != null)
            StartCoroutine(autoReturnCoroutine);
    }

    public void ActionBeforeReturn()
    {
        if (autoReturnCoroutine != null)
            StopCoroutine(autoReturnCoroutine);

        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        for(int i=0; i<particles.Length; ++i)
        {
            if (!particles[i].isStopped)
                particles[i].Stop();
        }
    }

    public void ReturnOrDestoryObject(ObjectPooler owner)
    {
        if (owner == null)
        {
            Destroy(gameObject);
            return;
        }
        owner.ReturnObject(name, gameObject);
    }
    public ObjectPooler ObjectPooler { get { return objectPooler; } }
    #endregion
}
