using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateHeavyHitLoop : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private AnimationClipInformation animationClipInformation;
    private float duration;
    private float time;

    public PlayerStateHeavyHitLoop(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_HIT_HEAVY_LOOP;
        animationClipInformation = character.AnimationClipTable["Player_Heavy_Hit_Loop"];
        duration = Constants.TIME_CHARACTER_STAND_UP;
        time = 0f;
    }

    public void Enter()
    {
        character.Animator.Play(animationClipInformation.nameHash);
        time = 0f;
    }

    public void Update()
    {
        if(time < duration)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                character.State.SetState(ACTION_STATE.PLAYER_STAND_ROLL, STATE_SWITCH_BY.WEIGHT);
                return;
            }
        }

        else
        {
            character.State.SetState(ACTION_STATE.PLAYER_STAND_UP, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        time += Time.deltaTime;
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
