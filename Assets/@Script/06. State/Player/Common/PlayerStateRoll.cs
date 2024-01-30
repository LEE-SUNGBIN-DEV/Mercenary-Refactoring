using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateRoll : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private AnimationClipInfo animationClipInfo;
    private Vector3 moveDirection;

    public PlayerStateRoll(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_ROLL;
        animationClipInfo = character.AnimationClipTable["Player_Roll"];
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
        character.StatusData.ConsumeStamina(Constants.PLAYER_STAMINA_CONSUMPTION_ROLL);
        character.Animator.Play(animationClipInfo.nameHash);
    }

    public void Update()
    {
        // -> Idle
        if (character.Animator.IsAnimationFrameUpTo(animationClipInfo, 20))
            character.State.SetState(character.CurrentWeapon.IdleState, STATE_SWITCH_BY.FORCED);
    }

    public void Exit()
    {
        character.HitState = HIT_STATE.HITTABLE;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
