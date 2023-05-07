using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBaseScene : UIBase
{
    protected bool isInitialized;
    protected Canvas canvas;

    public virtual void Initialize()
    {
        if (isInitialized == true)
        {
            Debug.Log($"{this}: Already Initialized.");
            return;
        }
        isInitialized = true;
        TryGetComponent(out canvas);
    }
}
