using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager
{
    // UI Inputs
    private bool[] uiKeys;
    private bool escDown;
    private bool optionDown;
    private bool inventoryDown;
    private bool skillDown;
    private bool statusDown;
    private bool questDown;

    // Character Inputs
    private Vector3 moveInput;
    private bool[] characterKeys;
    private bool leftMouseDown;
    private bool rightMouseDown;
    private bool rightMouseHold;
    private bool runHold;
    private bool rollDown;
    private bool counterDown;
    private bool swapDown;
    private bool resonanceWaterDown;
    private bool interactionDown;

    public void Initialize()
    {
        uiKeys = new bool[] { escDown, optionDown, inventoryDown, skillDown, statusDown, questDown};
        CancelKeys(uiKeys);

        characterKeys = new bool[] { leftMouseDown, rightMouseDown, rightMouseHold, runHold, rollDown, counterDown, swapDown, interactionDown };
        CancelKeys(characterKeys);
    }

    public void CancelKeys(bool[] keys)
    {
        for (int i = 0; i < keys.Length; ++i)
            keys[i] = false;
    }

    public void CancelCharacterInputs()
    {
        moveInput = Vector3.zero;
        CancelKeys(characterKeys);
    }

    public void UpdateCharacterInputs()
    {
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        leftMouseDown = Input.GetMouseButtonDown(0);
        rightMouseDown = Input.GetMouseButtonDown(1);
        rightMouseHold = Input.GetMouseButton(1);

        runHold = Input.GetKey(KeyCode.LeftShift);
        rollDown = Input.GetKeyDown(KeyCode.Space);
        counterDown = Input.GetKeyDown(KeyCode.R);
        swapDown = Input.GetKeyDown(KeyCode.Tab);
        resonanceWaterDown = Input.GetKeyDown(KeyCode.V);
        interactionDown = Input.GetKeyDown(KeyCode.F);
    }

    public void UpdateUIInputs()
    {
        escDown = Input.GetKeyDown(KeyCode.Escape);
        optionDown = Input.GetKeyDown(KeyCode.O);
        inventoryDown = Input.GetKeyDown(KeyCode.I);
        skillDown = Input.GetKeyDown(KeyCode.K);
        statusDown = Input.GetKeyDown(KeyCode.T);
        questDown = Input.GetKeyDown(KeyCode.Q);
    }

    #region Property
    // UI
    public bool EscDown { get { return escDown; } }
    public bool OptionDown { get { return optionDown; } }
    public bool SkillDown { get { return skillDown; } }
    public bool InventoryDown { get { return inventoryDown; } }
    public bool StatusDown { get { return statusDown; } }
    public bool QuestDown { get { return questDown; } }

    // Character
    public Vector3 MoveInput { get { return moveInput; } }
    public bool LeftMouseDown { get { return leftMouseDown; } }
    public bool RightMouseDown { get { return rightMouseDown; } }
    public bool RightMouseHold { get { return rightMouseHold; } }
    public bool RunHold { get { return runHold; } }
    public bool RollDown { get { return rollDown; } }
    public bool CounterDown { get { return counterDown; } }
    public bool SwapDown { get { return swapDown; } }
    public bool ResonanceWaterDown { get { return resonanceWaterDown; } }
    public bool InteractionDown { get { return interactionDown; } }
    #endregion
}
