using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShieldLightAttack02 : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;

    private PlayerSwordShield swordShield;
    private AnimationClipInformation animationClipInfo;

    private bool mouseLeftDown;
    private bool mouseRightDown;
    private Coroutine combatCoroutine;

    public SwordShieldLightAttack02(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_ATTACK_LIGHT_02;

        swordShield = character.WeaponController.GetWeapon<PlayerSwordShield>(WEAPON_TYPE.SWORD_SHIELD);
        animationClipInfo = character.AnimationClipTable["Sword_Shield_Light_Attack_02"];

        mouseLeftDown = false;
        mouseRightDown = false;
    }

    public void Enter()
    {
        character.Status.ConsumeStamina(Constants.SWORD_SHIELD_STAMINA_CONSUMPTION_LIGHT_ATTACK_02);
        character.SetForwardDirection(character.PlayerCamera.GetZeroYForward());
        character.Animator.CrossFadeInFixedTime(animationClipInfo.nameHash, 0.1f);

        mouseLeftDown = false;
        mouseRightDown = false;
        combatCoroutine = swordShield.StartCoroutine(CoEnableCombat());
    }

    public void Update()
    {
        if (character.GetInput().RollDown && character.Status.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_ROLL))
        {
            character.State.SetState(ACTION_STATE.PLAYER_ROLL, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (!mouseRightDown)
            mouseRightDown = character.GetInput().RightMouseDown;

        if (!mouseLeftDown)
            mouseLeftDown = character.GetInput().LeftMouseDown;

        // -> Heavy Attack 2
        if (mouseRightDown && character.Status.CheckStamina(Constants.SWORD_SHIELD_STAMINA_CONSUMPTION_HEAVY_ATTACK_02)
            && character.State.SetStateByAnimationTimeUpTo(animationClipInfo.nameHash, ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_HEAVY_02, 0.75f))
            return;

        // -> Light Attack 3
        if (mouseLeftDown && character.Status.CheckStamina(Constants.SWORD_SHIELD_STAMINA_CONSUMPTION_LIGHT_ATTACK_03) 
            && character.State.SetStateByAnimationTimeUpTo(animationClipInfo.nameHash, ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_LIGHT_03, 0.75f))
            return;

        // -> Idle
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInfo.nameHash, character.CurrentWeapon.IdleState, 0.9f))
            return;

        // Movement
        if (character.Animator.IsAnimationFrameBetweenTo(animationClipInfo, 0, 19))
            character.MoveController.SetMovementAndRotation(character.transform.forward, 4f * character.Status.AttackSpeed);
        else
            character.MoveController.SetMovementAndRotation(Vector3.zero, 0f);
    }

    public void Exit()
    {
        if (combatCoroutine != null)
            swordShield.StopCoroutine(combatCoroutine);

        swordShield.DisableSword();
        character.MoveController.SetMovementAndRotation(Vector3.zero, 0f);
    }

    private IEnumerator CoEnableCombat()
    {
        yield return new WaitUntil(() => character.Animator.IsAnimationFrameUpTo(animationClipInfo, 18) && !character.Animator.IsInTransition((int)ANIMATOR_LAYER.BASE));
        swordShield.EnableSword(COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_LIGHT_02);
        character.SFXPlayer.PlaySFX("Audio_Sword_Swing_02");

        yield return new WaitUntil(() => character.Animator.IsAnimationFrameUpTo(animationClipInfo, 30) && !character.Animator.IsInTransition((int)ANIMATOR_LAYER.BASE));
        swordShield.DisableSword();
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
