using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateStun : IActionState<BaseCharacter>, IDurationState
{
    private int stateWeight;
    private float duration;
    private int animationNameHash;

    public CharacterStateStun()
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_STUN;
        animationNameHash = Constants.ANIMATION_NAME_HASH_STUN;
    }

    public void Enter(BaseCharacter character)
    {
        character.Animator.Play(animationNameHash);
    }

    public void Update(BaseCharacter character)
    {
        if (duration <= 0f)
            character.State.SetState(ACTION_STATE.PLAYER_IDLE);

        duration -= Time.deltaTime;
    }

    public void Exit(BaseCharacter character)
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
