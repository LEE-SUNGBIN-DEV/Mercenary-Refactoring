using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolObject
{
    ObjectPooler ObjectPooler { get; }

    public void ActionAfterRequest(ObjectPooler owner);
    public void ActionBeforeReturn();
    public void ReturnOrDestoryObject();
}
