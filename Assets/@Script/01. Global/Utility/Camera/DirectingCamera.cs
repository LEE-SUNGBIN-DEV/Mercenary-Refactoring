using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectingCamera : BaseCamera
{
    protected override void Awake()
    {
        base.Awake();
        Managers.GameManager.DirectingCamera = this;
        gameObject.SetActive(false);
    }
}
