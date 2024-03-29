using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShieldGuardLoop : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;

    private PlayerSwordShield swordShield;
    private AnimationClipInfo animationClipInformation;

    public SwordShieldGuardLoop(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_GUARD_LOOP;
        if (character != null)
        {
            swordShield = character.UniqueEquipmentController.GetWeapon<PlayerSwordShield>(WEAPON_TYPE.SWORD_SHIELD);
            animationClipInformation = character.AnimationClipTable["Sword_Shield_Guard_Loop"];
        }
    }

    public void Enter()
    {
        character.HitState = HIT_STATE.GUARDABLE;
        character.Animator.Play(animationClipInformation.nameHash);
        swordShield.EnableShield(COMBAT_ACTION_TYPE.SWORD_SHIELD_GUARD_LOOP);
    }

    public void Update()
    {
        character.StatusData.ConsumeStamina(Constants.PLAYER_STAMINA_CONSUMPTION_GUARD_LOOP * Time.deltaTime);

        // -> Guard Out
        if ((!Managers.InputManager.CharacterGuardButton.IsPressed() || !character.StatusData.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_GUARD_LOOP))
            && character.State.SetStateNotInTransition(animationClipInformation.nameHash, ACTION_STATE.PLAYER_SWORD_SHIELD_GUARD_OUT))
            return;
    }

    public void Exit()
    {
        character.HitState = HIT_STATE.HITTABLE;
        swordShield.DisableShield();
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
