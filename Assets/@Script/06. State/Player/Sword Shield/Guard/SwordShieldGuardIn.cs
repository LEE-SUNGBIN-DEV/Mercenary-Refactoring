using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShieldGuardIn : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private PlayerSwordShield swordShield;
    private AnimationClipInformation animationClipInformation;

    public SwordShieldGuardIn(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_GUARD_IN;
        if (character != null)
        {
            swordShield = character.WeaponController.GetWeapon<PlayerSwordShield>(WEAPON_TYPE.SWORD_SHIELD);
            animationClipInformation = character.AnimationClipTable["Sword_Shield_Guard_In"];
        }
    }

    public void Enter()
    {
        character.Status.ConsumeStamina(Constants.PLAYER_STAMINA_CONSUMPTION_GUARD_IN);
        character.HitState = HIT_STATE.Parryable;
        character.SetForwardDirection(character.PlayerCamera.GetZeroYForward());
        character.Animator.Play(animationClipInformation.nameHash);
        swordShield.EnableShield(COMBAT_ACTION_TYPE.SWORD_SHIELD_GUARD_IN);
    }

    public void Update()
    {
        // -> Guard Out
        if (!character.GetInput().RightMouseHold && character.State.SetStateNotInTransition(animationClipInformation.nameHash, ACTION_STATE.PLAYER_SWORD_SHIELD_GUARD_OUT))
            return;

        // -> Guard Loop
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, ACTION_STATE.PLAYER_SWORD_SHIELD_GUARD_LOOP, 1f))
            return;
    }

    public void Exit()
    {
        character.HitState = HIT_STATE.Hittable;
        swordShield.DisableShield();
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
