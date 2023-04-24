using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateRoll : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private AnimationClipInformation animationClipInformation;
    private Vector3 moveDirection;

    public PlayerStateRoll(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_ROLL;
        animationClipInformation = character.AnimationClipTable["Player_Roll"];
    }

    public void Enter()
    {
        // 키보드 입력 방향으로 회피
        Vector3 verticalDirection = new Vector3(character.PlayerCamera.transform.forward.x, 0, character.PlayerCamera.transform.forward.z) * Input.GetAxisRaw("Vertical");
        Vector3 horizontalDirection = new Vector3(character.PlayerCamera.transform.right.x, 0, character.PlayerCamera.transform.right.z) * Input.GetAxisRaw("Horizontal");
        moveDirection = (verticalDirection + horizontalDirection).normalized;

        character.transform.forward = (moveDirection == Vector3.zero ? character.transform.forward : moveDirection);

        character.IsInvincible = true;
        character.Status.CurrentSP -= Constants.PLAYER_STAMINA_CONSUMPTION_ROLL;
        character.Animator.CrossFadeInFixedTime(animationClipInformation.nameHash, 0.1f);
    }

    public void Update()
    {
        // !! When Animation is over
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, character.CurrentWeapon.IdleState, 0.9f))
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
