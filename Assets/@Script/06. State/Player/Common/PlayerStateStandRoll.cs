using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateStandRoll : IActionState
{
    private PlayerCharacter character;
    public int stateWeight;
    private AnimationClipInformation animationClipInformation;
    private Vector3 moveInput;
    private Vector3 verticalDirection;
    private Vector3 horizontalDirection;
    private Vector3 moveDirection;

    public PlayerStateStandRoll(PlayerCharacter character)
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_STAND_ROLL;
        animationClipInformation = character.AnimationClipTable["Player_Stand_Roll"];
        this.character = character;
    }

    public void Enter()
    {
        // 키보드 입력 방향으로 회피
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        verticalDirection = new Vector3(character.PlayerCamera.transform.forward.x, 0, character.PlayerCamera.transform.forward.z);
        horizontalDirection = new Vector3(character.PlayerCamera.transform.right.x, 0, character.PlayerCamera.transform.right.z);
        moveDirection = (verticalDirection * moveInput.z + horizontalDirection * moveInput.x).normalized;
        character.transform.forward = (moveDirection == Vector3.zero ? character.transform.forward : moveDirection);

        character.HitState = HIT_STATE.Invincible;
        character.Animator.Play(animationClipInformation.nameHash);
    }

    public void Update()
    {
        // !! When animation is over
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, character.CurrentWeapon.IdleState, 0.9f))
            return;
    }

    public void Exit()
    {
        character.HitState = HIT_STATE.Hittable;
    }

    #region Property
    public int StateWeight { get => stateWeight; }
    #endregion
}
