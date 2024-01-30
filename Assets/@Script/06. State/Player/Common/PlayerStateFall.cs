using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerStateFall : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private AnimationClipInfo animationClipInformation;

    private Vector3 moveDirection;

    public PlayerStateFall(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_FALL;
        animationClipInformation = character.AnimationClipTable["Player_Fall"];
    }

    public void Enter()
    {
        character.Animator.Play(animationClipInformation.nameHash);
    }

    public void Update()
    {
        Vector3 verticalDirection = character.PlayerCamera.GetVerticalDirection() * Managers.InputManager.GetCharacterMoveVector().z;
        Vector3 horizontalDirection = character.PlayerCamera.GetHorizontalDirection() * Managers.InputManager.GetCharacterMoveVector().x;
        moveDirection = (verticalDirection + horizontalDirection);

        if (moveDirection.sqrMagnitude > 0f)
        {
            character.MoveController.SetMove(moveDirection, character.StatusData.StatDict[STAT_TYPE.STAT_MOVE_SPEED].GetFinalValue() * 0.5f);
        }
        else
        {
            character.MoveController.SetMove(Vector3.zero, 0f);
        }

        switch (character.MoveController.MoveState)
        {
            case MOVE_STATE.GROUNDING:
            case MOVE_STATE.FLOATING:
                character.State.SetState(ACTION_STATE.PLAYER_LANDING, STATE_SWITCH_BY.WEIGHT);
                break;
            case MOVE_STATE.FALLING:
                character.MoveController.FallTime += Time.deltaTime;
                break;

            default:
                break;
        }
    }

    public void Exit()
    {
        character.MoveController.SetMove(Vector3.zero, 0f);
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
