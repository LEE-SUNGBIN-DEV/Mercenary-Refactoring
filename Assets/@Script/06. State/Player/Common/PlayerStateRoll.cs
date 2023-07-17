using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateRoll : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private AnimationClipInformation animationClipInfo;
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
        Vector3 verticalDirection = new Vector3(character.PlayerCamera.transform.forward.x, 0, character.PlayerCamera.transform.forward.z) * character.GetInput().MoveInput.z;
        Vector3 horizontalDirection = new Vector3(character.PlayerCamera.transform.right.x, 0, character.PlayerCamera.transform.right.z) * character.GetInput().MoveInput.x;
        moveDirection = (verticalDirection + horizontalDirection);
        moveDirection = moveDirection == Vector3.zero ? character.transform.forward : moveDirection;

        character.SetForwardDirection(moveDirection);

        character.HitState = HIT_STATE.Invincible;
        character.Status.ConsumeStamina(Constants.PLAYER_STAMINA_CONSUMPTION_ROLL);
        character.Animator.Play(animationClipInfo.nameHash);
    }

    public void Update()
    {
        // -> Idle
        if (character.Animator.IsAnimationFrameUpTo(animationClipInfo, 20))
            character.State.SetState(character.CurrentWeapon.IdleState, STATE_SWITCH_BY.FORCED);

        // Movement
        if (character.Animator.IsAnimationFrameBetweenTo(animationClipInfo, 0, 16))
            character.MoveController.SetMovementAndRotation(moveDirection, Constants.PLAYER_ROLL_SPEED);
        else
            character.MoveController.SetMovementAndRotation(Vector3.zero, 0f);
    }

    public void Exit()
    {
        character.MoveController.SetMovementAndRotation(Vector3.zero, 0f);
        character.HitState = HIT_STATE.Hittable;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
