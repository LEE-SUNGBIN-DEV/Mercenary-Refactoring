using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShieldGuardIn : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private int animationNameHash;

    public SwordShieldGuardIn(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_GUARD_IN;
        animationNameHash = Constants.ANIMATION_NAME_HASH_SWORD_SHIELD_GUARD_IN;
    }

    public void Enter()
    {
        character.IsInvincible = true;
        character.SwordShield.SetAndEnableShield(COMBAT_ACTION_TYPE.SWORD_SHIELD_GUARD_IN);
        character.Animator.CrossFadeInFixedTime(animationNameHash, 0.02f);
    }

    public void Update()
    {
        if (!Input.GetMouseButton(1) && character.State.SetStateNotInTransition(animationNameHash, ACTION_STATE.PLAYER_SWORD_SHIELD_GUARD_OUT))
        {
            return;
        }

        // !! When animation is over
        if (character.State.SetStateByAnimationTimeUpTo(animationNameHash, ACTION_STATE.PLAYER_SWORD_SHIELD_GUARD_LOOP, 0.9f))
        {
            return;
        }
    }

    public void Exit()
    {
        character.IsInvincible = false;
        character.SwordShield.DisableShield();
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
