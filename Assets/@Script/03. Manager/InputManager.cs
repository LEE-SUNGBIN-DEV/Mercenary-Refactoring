using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager
{
    private Vector3 moveInput;

    private bool mouseLeftDown;
    private bool mouseRightDown;
    private bool mouseLeftPress;
    private bool mouseRightPress;

    private bool isLeftShiftKeyDown;
    private bool isSpaceKeyDown;
    private bool isEscapeKeyDown;

    private bool isRKeyDown;
    private bool isOKeyDown;
    private bool isIKeyDown;
    private bool isTKeyDown;
    private bool isQKeyDown;
    private bool isHKeyDown;

    public void Initialize()
    {
        moveInput = Vector3.zero;
        mouseLeftDown = false;
        mouseRightDown = false;
        mouseLeftPress = false;
        mouseRightPress = false;

        isLeftShiftKeyDown = false;
        isSpaceKeyDown = false;
        isEscapeKeyDown = false;

        isRKeyDown = false;
        isOKeyDown = false;
        isIKeyDown = false;
        isTKeyDown = false;
        isQKeyDown = false;
        isHKeyDown = false;
    }

    public void UpdateMoveInput()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = 0;
        moveInput.z = Input.GetAxisRaw("Vertical");
        isLeftShiftKeyDown = Input.GetKey(KeyCode.LeftShift);
    }

    public void UpdateCombatInput()
    {
        mouseLeftDown = Input.GetMouseButtonDown(0);
        mouseRightDown = Input.GetMouseButtonDown(1);
        mouseLeftPress = Input.GetMouseButton(0);
        mouseRightPress = Input.GetMouseButton(1);

        isSpaceKeyDown = Input.GetKeyDown(KeyCode.Space);
        isEscapeKeyDown = Input.GetKeyDown(KeyCode.Escape);

        isRKeyDown = Input.GetKey(KeyCode.R);
    }

    public void UpdateUIInput()
    {
        isOKeyDown = Input.GetKeyDown(KeyCode.O);
        isIKeyDown = Input.GetKeyDown(KeyCode.I);
        isTKeyDown = Input.GetKeyDown(KeyCode.T);
        isQKeyDown = Input.GetKeyDown(KeyCode.Q);
        isHKeyDown = Input.GetKeyDown(KeyCode.H);
    }

    #region Property
    public Vector3 MoveInput { get { return moveInput; } }
    public bool MouseLeftDown { get { return mouseLeftDown; } }
    public bool MouseRightDown { get { return mouseRightDown; } }
    public bool MouseLeftPress { get { return mouseLeftPress; } }
    public bool MouseRightPress { get { return mouseRightPress; } }
    public bool IsLeftShiftKeyDown { get => isLeftShiftKeyDown; }
    public bool IsSpaceKeyDown { get => isSpaceKeyDown; }
    public bool IsEscapeKeyDown { get => isEscapeKeyDown; }
    public bool IsRKeyDown { get => isRKeyDown; }
    public bool IsOKeyDown { get => isOKeyDown; }
    public bool IsIKeyDown { get => isIKeyDown; }
    public bool IsTKeyDown { get => isTKeyDown; }
    public bool IsQKeyDown { get => isQKeyDown; }
    public bool IsHKeyDown { get => isHKeyDown; }
    #endregion
}
