using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateHeavyHitLoop : IActionState<BaseCharacter>
{
    private int stateWeight;
    private int animationNameHash;
    private float duration;
    private float time;

    public CharacterStateHeavyHitLoop()
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_HIT_HEAVY_LOOP;
        animationNameHash = Constants.ANIMATION_NAME_HASH_HEAVY_HIT_Loop;
        duration = Constants.TIME_CHARACTER_STAND_UP;
        time = 0f;
    }

    public void Enter(BaseCharacter character)
    {
        character.Animator.Play(animationNameHash);
        time = 0f;
    }

    public void Update(BaseCharacter character)
    {
        if(time < duration)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                character.State.SetState(ACTION_STATE.PLAYER_STAND_ROLL);
                return;
            }
        }

        else
        {
            character.State.SetState(ACTION_STATE.PLAYER_STAND_UP);
            return;
        }

        time += Time.deltaTime;
    }

    public void Exit(BaseCharacter character)
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
