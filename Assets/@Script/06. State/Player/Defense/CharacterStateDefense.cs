using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateDefense : IActionState
{
    private BaseCharacter character;
    private int stateWeight;
    private int animationNameHash;

    public CharacterStateDefense(BaseCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_DEFENSE_START;
        animationNameHash = Constants.ANIMATION_NAME_HASH_DEFENSE;
    }

    public void Enter()
    {
        character.IsInvincible = true;
        character.Shield.OnEnableDefense(COMBAT_TYPE.PARRYING);
        character.Animator.CrossFade(animationNameHash, 0.1f);
    }

    public void Update()
    {
        if(Input.GetMouseButton(1) && character.State.SetStateByAnimationTimeUpTo(animationNameHash, ACTION_STATE.PLAYER_DEFENSE_LOOP, 1.0f))
        {
            return;
        }

        // !! When animation is over
        if (character.State.SetStateByAnimationTimeUpTo(animationNameHash, ACTION_STATE.PLAYER_DEFENSE_END, 1.0f))
        {
            return;
        }
    }

    public void Exit()
    {
        character.IsInvincible = false;
        character.Shield.OnDisableDefense();
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
