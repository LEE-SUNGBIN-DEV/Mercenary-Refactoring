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
            character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_DOWN, false);

        else if (time > 1.133f && time < duration)
        {
            if(Managers.InputManager.IsSpaceKeyDown)
            {
                character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_DOWN, false);
                character.State.SwitchState(CHARACTER_STATE.StandRoll);
            }
        }
        time += Time.deltaTime;

        if (character.Animator.GetNextAnimatorStateInfo(0).IsName(Constants.ANIMATOR_STATE_NAME_MOVE_BLEND_TREE))
            character.SwitchState(CHARACTER_STATE.Move);
    }

    public void Exit(BaseCharacter character)
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
