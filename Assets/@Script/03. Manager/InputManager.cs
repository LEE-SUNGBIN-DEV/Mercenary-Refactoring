using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager
{
    private bool isEscapeKeyDown;
    private bool isOKeyDown;
    private bool isIKeyDown;
    private bool isTKeyDown;
    private bool isQKeyDown;
    private bool isHKeyDown;

    public void Initialize()
    {
        isEscapeKeyDown = false;

        isOKeyDown = false;
        isIKeyDown = false;
        isTKeyDown = false;
        isQKeyDown = false;
        isHKeyDown = false;
    }

    public void UpdateUIInput()
    {
        isOKeyDown = Input.GetKeyDown(KeyCode.O);
        isIKeyDown = Input.GetKeyDown(KeyCode.I);
        isTKeyDown = Input.GetKeyDown(KeyCode.T);
        isQKeyDown = Input.GetKeyDown(KeyCode.Q);
        isHKeyDown = Input.GetKeyDown(KeyCode.H);
        isEscapeKeyDown = Input.GetKeyDown(KeyCode.Escape);
    }

    #region Property
    public bool IsEscapeKeyDown { get => isEscapeKeyDown; }
    public bool IsOKeyDown { get => isOKeyDown; }
    public bool IsIKeyDown { get => isIKeyDown; }
    public bool IsTKeyDown { get => isTKeyDown; }
    public bool IsQKeyDown { get => isQKeyDown; }
    public bool IsHKeyDown { get => isHKeyDown; }
    #endregion
}
