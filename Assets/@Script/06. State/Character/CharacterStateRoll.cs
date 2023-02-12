using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateRoll : ICharacterState
{
    private int stateWeight;
    private Animator animator;
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
        animator = character.Animator;

        // 키보드 입력 방향으로 회피
        moveInput = Managers.InputManager.MoveInput;
        verticalDirection = new Vector3(character.PlayerCamera.transform.forward.x, 0, character.PlayerCamera.transform.forward.z);
        horizontalDirection = new Vector3(character.PlayerCamera.transform.right.x, 0, character.PlayerCamera.transform.right.z);
        moveDirection = (verticalDirection * moveInput.z + horizontalDirection * moveInput.x).normalized;
        character.transform.forward = (moveDirection == Vector3.zero ? character.transform.forward : moveDirection);

        character.IsInvincible = true;
        character.StatusData.CurrentSP -= Constants.CHARACTER_STAMINA_CONSUMPTION_ROLL;
        character.Animator.CrossFade(Constants.ANIMATION_NAME_ROLL, 0.1f);
    }

    public void Update(BaseCharacter character)
    {
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == Constants.ANIMATION_NAME_ROLL
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f
            && !animator.IsInTransition(0))
        {
            character.SetState(CHARACTER_STATE.Idle);
        }
    }

    public void Exit(BaseCharacter character)
    {
        character.IsInvincible = false;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
