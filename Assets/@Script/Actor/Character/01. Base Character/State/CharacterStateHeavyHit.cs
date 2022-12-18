using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateHeavyHit : ICharacterState
{
    private int stateWeight;
    private float duration;
    private float time;

    public CharacterStateHeavyHit()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.HeavyHit;
        duration = Constants.TIME_CHARACTER_STAND_UP;
        time = 0f;
    }

    public void Enter(BaseCharacter character)
    {
        character.Animator.SetTrigger(Constants.ANIMATOR_PARAMETERS_TRIGGER_HEAVY_HIT);
        character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_DOWN, true);
        time = 0f;
    }

    public void Update(BaseCharacter character)
    {
        if(time >= duration)
        {
            character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_DOWN, false);
        }
        else if (time > 1.133f && time < duration)
        {
            if(character.PlayerInput.IsSpaceKeyDown)
            {
                character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_DOWN, false);
                character.State.SwitchCharacterState(CHARACTER_STATE.StandRoll);
            }
        }
        time += Time.deltaTime;
    }

    public void Exit(BaseCharacter character)
    {
        time = 0f;
    }

    #region Property
    public int StateWeight { get => stateWeight; }
    #endregion
}
