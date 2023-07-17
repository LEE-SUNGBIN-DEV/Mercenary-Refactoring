using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateFall : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private AnimationClipInformation animationClipInformation;

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
        switch (character.MoveController.GroundState)
        {
            case ACTOR_GROUND_STATE.GROUNDING:
                character.State.SetState(ACTION_STATE.PLAYER_LANDING, STATE_SWITCH_BY.WEIGHT);
                break;

            case ACTOR_GROUND_STATE.FLOATING:
                break;

            case ACTOR_GROUND_STATE.SLIDING:
                character.MoveController.SlideTime += Time.deltaTime;
                break;
            case ACTOR_GROUND_STATE.FALLING:
                character.MoveController.FallTime += Time.deltaTime;
                break;
        }
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
