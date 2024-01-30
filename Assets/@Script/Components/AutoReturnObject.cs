using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoReturnObject : MonoBehaviour, IPoolObject
{
    [Header("Auto Return")]
    [SerializeField] protected bool isAutoReturnEnable;
    [SerializeField] protected float duration;

    protected Coroutine autoReturnCoroutine;

    public bool TryStartAutoReturn()
    {
        if(isAutoReturnEnable)
        {
            if (autoReturnCoroutine != null)
                StopCoroutine(autoReturnCoroutine);

            autoReturnCoroutine = StartCoroutine(CoAutoReturn());
        }

        return isAutoReturnEnable;
    }

    public IEnumerator CoAutoReturn()
    {
        yield return new WaitForSeconds(duration);
        ReturnOrDestoryObject();
    }

    #region IPoolObject Interface Fucntion
    public virtual void ActionAfterRequest(ObjectPooler owner)
    {
        ObjectPooler = owner;
        TryStartAutoReturn();
    }

    public virtual void ActionBeforeReturn()
    {
        if (autoReturnCoroutine != null)
            StopCoroutine(autoReturnCoroutine);
    }

    public virtual void ReturnOrDestoryObject()
    {
        if (ObjectPooler == null)
            Destroy(gameObject);

        ObjectPooler.ReturnObject(name, gameObject);
    }

    public ObjectPooler ObjectPooler { get; set; }
    #endregion
}
