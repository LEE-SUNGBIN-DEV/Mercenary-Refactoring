using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateStun : IActionState, IDurationState
{
    private PlayerCharacter character;
    private int stateWeight;
    private float duration;
    private AnimationClipInformation animationClipInformation;

    public PlayerStateStun(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_STUN;
        animationClipInformation = character.AnimationClipTable["Player_Stun"];
    }

    public void Enter()
    {
        character.Animator.Play(animationClipInformation.nameHash);
    }

    public void Update()
    {
        if (duration <= 0f)
            character.State.SetState(ACTION_STATE.PLAYER_HALBERD_IDLE, STATE_SWITCH_BY.FORCED);

        duration -= Time.deltaTime;
    }

    public void Exit()
    {
        duration = 0f;
    }

    public void SetDuration(float duration = 0)
    {
        this.duration = duration;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    public float Duration { get { return duration; }}
    #endregion
}
