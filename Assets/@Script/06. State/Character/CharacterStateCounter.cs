using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateCounter : ICharacterState
{
    private int stateWeight;
    private Animator animator;
    private Vector3 moveInput;
    private Vector3 verticalDirection;
    private Vector3 horizontalDirection;
    private Vector3 moveDirection;

    public CharacterStateCounter()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Counter;
    }

    public void Enter(BaseCharacter character)
    {
        animator = character.Animator;

        // 키보드 입력 방향으로 공격
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        verticalDirection = new Vector3(character.PlayerCamera.transform.forward.x, 0, character.PlayerCamera.transform.forward.z);
        horizontalDirection = new Vector3(character.PlayerCamera.transform.right.x, 0, character.PlayerCamera.transform.right.z);
        moveDirection = (verticalDirection * moveInput.z + horizontalDirection * moveInput.x).normalized;
        character.transform.forward = (moveDirection == Vector3.zero ? character.transform.forward : moveDirection);

        character.StatusData.CurrentSP -= Constants.CHARACTER_STAMINA_CONSUMPTION_COUNTER;
        character.Animator.CrossFade(Constants.ANIMATION_NAME_SKILL_COUNTER, 0.1f);
    }

    public void Update(BaseCharacter character)
    {
        if (Input.GetKeyDown(KeyCode.Space) && character.StatusData.CheckStamina(Constants.CHARACTER_STAMINA_CONSUMPTION_ROLL))
        {
            character.TrySwitchState(CHARACTER_STATE.Roll);
            return;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == Constants.ANIMATION_NAME_SKILL_COUNTER
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            character.SetState(CHARACTER_STATE.Idle);
        }
    }

    public void Exit(BaseCharacter character)
    {
    }

    #region Property
    public int StateWeight { get => stateWeight; }
    #endregion
}
