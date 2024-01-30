using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBaseCanvas : UIBase
{
    protected Canvas canvas;

    protected virtual void Awake()
    {
        TryGetComponent(out canvas);
    }
}
