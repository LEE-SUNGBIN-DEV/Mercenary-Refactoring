using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateRoll : ICharacterState
{
    private int stateWeight;
    private int animationNameHash;
    private Vector3 moveInput;
    private Vector3 verticalDirection;
    private Vector3 horizontalDirection;
    private Vector3 moveDirection;

    public CharacterStateRoll()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Roll;
        animationNameHash = Constants.ANIMATION_NAME_ROLL;
    }

    public void Enter(BaseCharacter character)
    {
        // Ű���� �Է� �������� ȸ��
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        verticalDirection = new Vector3(character.PlayerCamera.transform.forward.x, 0, character.PlayerCamera.transform.forward.z);
        horizontalDirection = new Vector3(character.PlayerCamera.transform.right.x, 0, character.PlayerCamera.transform.right.z);
        moveDirection = (verticalDirection * moveInput.z + horizontalDirection * moveInput.x).normalized;
        character.transform.forward = (moveDirection == Vector3.zero ? character.transform.forward : moveDirection);

        character.IsInvincible = true;
        character.StatusData.CurrentSP -= Constants.CHARACTER_STAMINA_CONSUMPTION_ROLL;
        character.Animator.CrossFade(animationNameHash, 0.1f);
    }

    public void Update(BaseCharacter character)
    {

        // !! When Animation is over
        if (character.State.SetStateByUpperAnimationTime(animationNameHash, CHARACTER_STATE.Idle, 0.9f))
            return;
    }

    public void Exit(BaseCharacter character)
    {
        character.IsInvincible = false;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
