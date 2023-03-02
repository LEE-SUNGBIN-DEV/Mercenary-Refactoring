using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateStandRoll : IActionState<BaseCharacter>
{
    public int stateWeight;
    private int animationNameHash;
    private Vector3 moveInput;
    private Vector3 verticalDirection;
    private Vector3 horizontalDirection;
    private Vector3 moveDirection;

    public CharacterStateStandRoll()
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_STAND_ROLL;
        animationNameHash = Constants.ANIMATION_NAME_HASH_STAND_ROLL;
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
        character.Animator.Play(animationNameHash);
    }

    public void Update(BaseCharacter character)
    {
        // !! When animation is over
        if (character.State.SetStateByUpperAnimationTime(animationNameHash, ACTION_STATE.PLAYER_IDLE, 0.9f))
            return;
    }

    public void Exit(BaseCharacter character)
    {
        character.IsInvincible = false;
    }

    #region Property
    public int StateWeight { get => stateWeight; }
    #endregion
}
