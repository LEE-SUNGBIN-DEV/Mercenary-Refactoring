using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectingCamera : BaseCamera
{
    public override void Initialize()
    {
        base.Initialize();
        gameObject.SetActive(false);
    }
}
