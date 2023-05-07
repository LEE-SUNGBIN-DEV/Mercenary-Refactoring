using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShieldLightAttack03 : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;

    private PlayerSwordShield swordShield;
    private AnimationClipInformation animationClipInformation;

    private bool mouseLeftDown;
    private bool mouseRightDown;

    public SwordShieldLightAttack03(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_ATTACK_LIGHT_03;

        swordShield = character.WeaponController.GetWeapon<PlayerSwordShield>(WEAPON_TYPE.SWORD_SHIELD);
        animationClipInformation = character.AnimationClipTable["Sword_Shield_Light_Attack_03"];

        mouseLeftDown = false;
        mouseRightDown = false;
    }

    public void Enter()
    {
        character.SetForwardDirection(character.PlayerCamera.GetForward(true));
        character.Animator.CrossFadeInFixedTime(animationClipInformation.nameHash, 0.1f);

        mouseLeftDown = false;
        mouseRightDown = false;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && character.Status.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_ROLL))
        {
            character.State.SetState(ACTION_STATE.PLAYER_ROLL, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (!mouseRightDown)
            mouseRightDown = Input.GetMouseButtonDown(1);

        if (!mouseLeftDown)
            mouseLeftDown = Input.GetMouseButtonDown(0);

        // -> Smash Attack 3
        if (mouseRightDown && character.State.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_HEAVY_03, 0.75f))
        {
            return;
        }

        // -> Light Attack 4
        if (mouseLeftDown && character.State.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_LIGHT_04, 0.75f))
        {
            return;
        }

        // -> Idle
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, character.CurrentWeapon.IdleState, 0.9f))
            return;
    }

    public void Exit()
    {
        swordShield.DisableSword();
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
