using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateDefense : IActionState<BaseCharacter>
{
    private int stateWeight;
    private int animationNameHash;

    public CharacterStateDefense()
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_DEFENSE_START;
        animationNameHash = Constants.ANIMATION_NAME_HASH_DEFENSE;
    }

    public void Enter(BaseCharacter character)
    {
        character.IsInvincible = true;
        character.Shield.OnEnableDefense(COMBAT_TYPE.PARRYING);
        character.Animator.CrossFade(animationNameHash, 0.1f);
    }

    public void Update(BaseCharacter character)
    {
        if(Input.GetMouseButton(1) && character.State.SetStateByUpperAnimationTime(animationNameHash, ACTION_STATE.PLAYER_DEFENSE_LOOP, 1.0f))
        {
            return;
        }

        // !! When animation is over
        if (character.State.SetStateByUpperAnimationTime(animationNameHash, ACTION_STATE.PLAYER_DEFENSE_END, 1.0f))
        {
            return;
        }
    }

    public void Exit(BaseCharacter character)
    {
        character.IsInvincible = false;
        character.Shield.OnDisableDefense();
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
