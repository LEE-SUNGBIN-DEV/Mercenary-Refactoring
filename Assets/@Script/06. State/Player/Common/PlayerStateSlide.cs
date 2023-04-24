using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateSlide : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private AnimationClipInformation animationClipInformation;

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
        switch (character.MoveController.GetGroundState())
        {
            case ACTOR_GROUND_STATE.GROUND:
                character.Status.CurrentHP -= character.Status.MaxHP * character.MoveController.GetFallDamage();
                character.State.SetState(character.CurrentWeapon.IdleState, STATE_SWITCH_BY.FORCED);
                break;

            case ACTOR_GROUND_STATE.SLOPE:
                character.MoveController.SlideTime += Time.deltaTime;
                break;

            case ACTOR_GROUND_STATE.AIR:
                character.State.SetState(ACTION_STATE.PLAYER_FALL, STATE_SWITCH_BY.WEIGHT);
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