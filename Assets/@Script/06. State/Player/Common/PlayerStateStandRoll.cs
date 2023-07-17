using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateStandRoll : IActionState
{
    private PlayerCharacter character;
    public int stateWeight;
    private AnimationClipInformation animationClipInfo;
    private Vector3 moveDirection;

    public PlayerStateStandRoll(PlayerCharacter character)
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_STAND_ROLL;
        animationClipInfo = character.AnimationClipTable["Player_Stand_Roll"];
        this.character = character;
    }

    public void Enter()
    {
        // 키보드 입력 방향으로 회피
        Vector3 verticalDirection = new Vector3(character.PlayerCamera.transform.forward.x, 0, character.PlayerCamera.transform.forward.z) * character.GetInput().MoveInput.z;
        Vector3 horizontalDirection = new Vector3(character.PlayerCamera.transform.right.x, 0, character.PlayerCamera.transform.right.z) * character.GetInput().MoveInput.x;
        moveDirection = (verticalDirection + horizontalDirection);
        moveDirection = moveDirection == Vector3.zero ? character.transform.forward : moveDirection;

        character.SetForwardDirection(moveDirection);

        character.HitState = HIT_STATE.Invincible;
        character.Animator.Play(animationClipInfo.nameHash);
    }

    public void Update()
    {
        // -> Idle
        if (character.Animator.IsAnimationFrameUpTo(animationClipInfo, 24))
            character.State.SetState(character.CurrentWeapon.IdleState, STATE_SWITCH_BY.FORCED);
        
        // Movement
        if (character.Animator.IsAnimationFrameDownTo(animationClipInfo, 20))
            character.MoveController.SetMovementAndRotation(moveDirection, Constants.PLAYER_STAND_ROLL_SPEED);
        else
            character.MoveController.SetMovementAndRotation(Vector3.zero, 0f);
    }

    public void Exit()
    {
        character.MoveController.SetMovementAndRotation(Vector3.zero, 0f);
        character.HitState = HIT_STATE.Hittable;
    }

    #region Property
    public int StateWeight { get => stateWeight; }
    #endregion
}
