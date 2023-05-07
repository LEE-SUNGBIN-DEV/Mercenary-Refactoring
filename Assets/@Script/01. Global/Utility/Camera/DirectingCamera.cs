using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectingCamera : BaseCamera
{
    public override void Initialize()
    {
        base.Initialize();
        Managers.GameManager.DirectingCamera = this;
        gameObject.SetActive(false);
    }
}
