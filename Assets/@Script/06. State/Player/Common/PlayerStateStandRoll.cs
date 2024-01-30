using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateStandRoll : IActionState
{
    private PlayerCharacter character;
    public int stateWeight;
    private AnimationClipInfo animationClipInfo;
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
        Vector3 verticalDirection = character.PlayerCamera.GetVerticalDirection() * Managers.InputManager.GetCharacterMoveVector().z;
        Vector3 horizontalDirection = character.PlayerCamera.GetHorizontalDirection() * Managers.InputManager.GetCharacterMoveVector().x;
        moveDirection = (verticalDirection + horizontalDirection);
        moveDirection = moveDirection == Vector3.zero ? character.transform.forward : moveDirection;

        character.SetForwardDirection(moveDirection);

        character.HitState = HIT_STATE.INVINCIBLE;
        character.Animator.Play(animationClipInfo.nameHash);
    }

    public void Update()
    {
        // -> Idle
        if (character.Animator.IsAnimationFrameUpTo(animationClipInfo, 24))
            character.State.SetState(character.CurrentWeapon.IdleState, STATE_SWITCH_BY.FORCED);
    }

    public void Exit()
    {
        character.MoveController.SetMove(Vector3.zero, 0f);
        character.HitState = HIT_STATE.HITTABLE;
    }

    #region Property
    public int StateWeight { get => stateWeight; }
    #endregion
}
