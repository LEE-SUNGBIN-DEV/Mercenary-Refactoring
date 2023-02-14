using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateHeavyHitLoop : ICharacterState
{
    private int stateWeight;
    private int animationNameHash;
    private float duration;
    private float time;

    public CharacterStateHeavyHitLoop()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.HeavyHitLoop;
        animationNameHash = Constants.ANIMATION_NAME_HEAVY_HIT_Loop;
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
                character.State.SetState(CHARACTER_STATE.Stand_Roll);
                return;
            }
        }

        else
        {
            character.State.SetState(CHARACTER_STATE.Stand_Up);
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
