using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateRoll : ICharacterState
{
    private int stateWeight;
    private Vector3 moveInput;
    private Vector3 verticalDirection;
    private Vector3 horizontalDirection;
    private Vector3 moveDirection;

    public CharacterStateRoll()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Roll;
    }

    public void Enter(BaseCharacter character)
    {
        character.StatusData.CurrentSP -= Constants.CHARACTER_STAMINA_CONSUMPTION_ROLL;

        // 키보드 입력 방향으로 회피
        moveInput = Managers.InputManager.MoveInput;
        verticalDirection = new Vector3(character.PlayerCamera.transform.forward.x, 0, character.PlayerCamera.transform.forward.z);
        horizontalDirection = new Vector3(character.PlayerCamera.transform.right.x, 0, character.PlayerCamera.transform.right.z);
        moveDirection = (verticalDirection * moveInput.z + horizontalDirection * moveInput.x).normalized;

        character.transform.forward = (moveDirection == Vector3.zero ? character.transform.forward : moveDirection);

        // Set Roll State
        character.IsInvincible = true;
        character.Animator.SetTrigger(Constants.ANIMATOR_PARAMETERS_TRIGGER_ROLL);
    }

    public void Update(BaseCharacter character)
    {
        if (character.Animator.GetNextAnimatorStateInfo(0).IsName(Constants.ANIMATOR_STATE_NAME_MOVE_BLEND_TREE))
            character.SwitchState(CHARACTER_STATE.Move);
    }

    public void Exit(BaseCharacter character)
    {
        character.IsInvincible = false;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
