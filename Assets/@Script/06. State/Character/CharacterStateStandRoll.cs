using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateStandRoll : ICharacterState
{
    public int stateWeight;
    private Vector3 moveInput;
    private Vector3 verticalDirection;
    private Vector3 horizontalDirection;
    private Vector3 moveDirection;

    public CharacterStateStandRoll()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.StandRoll;
    }

    public void Enter(BaseCharacter character)
    {
        // 키보드 입력 방향으로 회피
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        verticalDirection = new Vector3(character.PlayerCamera.transform.forward.x, 0, character.PlayerCamera.transform.forward.z);
        horizontalDirection = new Vector3(character.PlayerCamera.transform.right.x, 0, character.PlayerCamera.transform.right.z);
        moveDirection = (verticalDirection * moveInput.z + horizontalDirection * moveInput.x).normalized;

        character.transform.forward = (moveDirection == Vector3.zero ? character.transform.forward : moveDirection);

        character.IsInvincible = true;
        character.Animator.SetTrigger(Constants.ANIMATOR_PARAMETERS_TRIGGER_STAND_ROLL);
    }

    public void Update(BaseCharacter character)
    {
        if (character.Animator.GetNextAnimatorStateInfo(0).IsName(Constants.ANIMATOR_STATE_NAME_MOVE_BLEND_TREE))
            character.SwitchCharacterState(CHARACTER_STATE.Move);
    }

    public void Exit(BaseCharacter character)
    {
        character.IsInvincible = false;
    }

    #region Property
    public int StateWeight { get => stateWeight; }
    #endregion
}
