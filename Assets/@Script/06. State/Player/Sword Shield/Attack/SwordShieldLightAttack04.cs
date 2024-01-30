using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShieldLightAttack04 : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;

    private PlayerSwordShield swordShield;
    private AnimationClipInfo animationClipInfo;

    private bool mouseLeftDown;
    private bool mouseRightDown;
    private Coroutine combatCoroutine;

    public SwordShieldLightAttack04(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_ATTACK_LIGHT_04;

        swordShield = character.UniqueEquipmentController.GetWeapon<PlayerSwordShield>(WEAPON_TYPE.SWORD_SHIELD);
        animationClipInfo = character.AnimationClipTable["Sword_Shield_Light_Attack_04"];

        mouseLeftDown = false;
        mouseRightDown = false;
    }

    public void Enter()
    {
        character.StatusData.ConsumeStamina(Constants.SWORD_SHIELD_STAMINA_CONSUMPTION_LIGHT_ATTACK_04);
        character.SetForwardDirection(character.PlayerCamera.GetVerticalDirection());
        character.Animator.CrossFadeInFixedTime(animationClipInfo.nameHash, 0.1f);

        mouseLeftDown = false;
        mouseRightDown = false;
        combatCoroutine = swordShield.StartCoroutine(CoEnableCombat());
    }

    public void Update()
    {
        if (Managers.InputManager.CharacterRollButton.WasPressedThisFrame() && character.StatusData.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_ROLL))
        {
            character.State.SetState(ACTION_STATE.PLAYER_ROLL, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (!mouseRightDown)
            mouseRightDown = Managers.InputManager.CharacterHeavyAttackButton.WasPressedThisFrame();

        if (!mouseLeftDown)
            mouseLeftDown = Managers.InputManager.CharacterLightAttackButton.WasPressedThisFrame();

        // -> Heavy Attack 4
        if (mouseRightDown && character.StatusData.CheckStamina(Constants.SWORD_SHIELD_STAMINA_CONSUMPTION_HEAVY_ATTACK_04) 
            && character.State.SetStateByAnimationTimeUpTo(animationClipInfo.nameHash, ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_HEAVY_04, 0.7f))
            return;

        // -> Light Attack 1
        if (mouseLeftDown && character.StatusData.CheckStamina(Constants.SWORD_SHIELD_STAMINA_CONSUMPTION_LIGHT_ATTACK_01)
            && character.State.SetStateByAnimationTimeUpTo(animationClipInfo.nameHash, ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_LIGHT_01, 0.75f))
            return;

        // -> Idle
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInfo.nameHash, character.CurrentWeapon.IdleState, 0.9f))
            return;
    }

    public void Exit()
    {
        if (combatCoroutine != null)
            swordShield.StopCoroutine(combatCoroutine);

        swordShield.DisableSword();
    }

    private IEnumerator CoEnableCombat()
    {
        yield return new WaitUntil(() => character.Animator.IsAnimationFrameUpTo(animationClipInfo, 24) && !character.Animator.IsInTransition((int)ANIMATOR_LAYER.BASE));
        swordShield.EnableSword(COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_LIGHT_04);
        character.SFXPlayer.PlaySFX("Audio_Sword_Swing_03");

        yield return new WaitUntil(() => character.Animator.IsAnimationFrameUpTo(animationClipInfo, 40) && !character.Animator.IsInTransition((int)ANIMATOR_LAYER.BASE));
        swordShield.DisableSword();
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion

}
