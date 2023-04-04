using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShieldGuardLoop : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private int animationNameHash;

    public SwordShieldGuardLoop(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_GUARD_LOOP;
        animationNameHash = Constants.ANIMATION_NAME_HASH_SWORD_SHIELD_GUARD_LOOP;
    }

    public void Enter()
    {
        character.IsInvincible = true;
        character.SwordShield.SetAndEnableShield(COMBAT_ACTION_TYPE.SWORD_SHIELD_GUARD_LOOP);
        character.Animator.Play(animationNameHash);
    }

    public void Update()
    {
        if (!Input.GetMouseButton(1) && character.State.SetStateNotInTransition(animationNameHash, ACTION_STATE.PLAYER_SWORD_SHIELD_GUARD_OUT))
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