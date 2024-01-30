using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerStateSlide : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private AnimationClipInfo animationClipInformation;

    public PlayerStateSlide(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_SLIDE;
        animationClipInformation = character.AnimationClipTable["Player_Slide"];
    }

    public void Enter()
    {
        character.Animator.Play(animationClipInformation.nameHash);
    }

    public void Update()
    {
        switch (character.MoveController.MoveState)
        {
            case MOVE_STATE.GROUNDING:
            case MOVE_STATE.FLOATING:
                character.State.SetState(ACTION_STATE.PLAYER_LANDING, STATE_SWITCH_BY.WEIGHT);
                break;

            case MOVE_STATE.SLIDING:
                character.MoveController.SlideTime += Time.deltaTime;
                break;

            case MOVE_STATE.FALLING:
                character.State.SetState(ACTION_STATE.PLAYER_FALL, STATE_SWITCH_BY.WEIGHT);
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
