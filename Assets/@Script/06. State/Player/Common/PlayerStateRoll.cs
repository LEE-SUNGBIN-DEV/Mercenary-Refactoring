using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateRoll : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private int animationNameHash;
    private Vector3 moveInput;
    private Vector3 verticalDirection;
    private Vector3 horizontalDirection;
    private Vector3 moveDirection;

    public PlayerStateRoll(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_ROLL;
        animationNameHash = Constants.ANIMATION_NAME_HASH_ROLL;
    }

    public void Enter()
    {
        // Ű���� �Է� �������� ȸ��
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        verticalDirection = new Vector3(character.PlayerCamera.transform.forward.x, 0, character.PlayerCamera.transform.forward.z);
        horizontalDirection = new Vector3(character.PlayerCamera.transform.right.x, 0, character.PlayerCamera.transform.right.z);
        moveDirection = (verticalDirection * moveInput.z + horizontalDirection * moveInput.x).normalized;
        character.transform.forward = (moveDirection == Vector3.zero ? character.transform.forward : moveDirection);

        character.IsInvincible = true;
        character.Status.CurrentSP -= Constants.PLAYER_STAMINA_CONSUMPTION_ROLL;
        character.Animator.CrossFadeInFixedTime(animationNameHash, 0.1f);
    }

    public void Update()
    {

        // !! When Animation is over
        if (character.State.SetStateByAnimationTimeUpTo(animationNameHash, character.CurrentWeapon.BasicStateInformation.idleState, 0.9f))
            return;
    }

    public void Exit()
    {
        character.IsInvincible = false;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
