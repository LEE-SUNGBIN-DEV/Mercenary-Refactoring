using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShieldLightAttack03 : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;

    private PlayerSwordShield swordShield;
    private AnimationClipInformation animationClipInfo;

    private bool mouseLeftDown;
    private bool mouseRightDown;
    private Coroutine combatCoroutine;

    public SwordShieldLightAttack03(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_ATTACK_LIGHT_03;

        swordShield = character.WeaponController.GetWeapon<PlayerSwordShield>(WEAPON_TYPE.SWORD_SHIELD);
        animationClipInfo = character.AnimationClipTable["Sword_Shield_Light_Attack_03"];

        mouseLeftDown = false;
        mouseRightDown = false;
    }

    public void Enter()
    {
        character.Status.ConsumeStamina(Constants.SWORD_SHIELD_STAMINA_CONSUMPTION_LIGHT_ATTACK_03);
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

        // -> Heavy Attack 3
        if (mouseRightDown && character.Status.CheckStamina(Constants.SWORD_SHIELD_STAMINA_CONSUMPTION_HEAVY_ATTACK_03) 
            && character.State.SetStateByAnimationTimeUpTo(animationClipInfo.nameHash, ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_HEAVY_03, 0.75f))
            return;

        // -> Light Attack 4
        if (mouseLeftDown && character.Status.CheckStamina(Constants.SWORD_SHIELD_STAMINA_CONSUMPTION_LIGHT_ATTACK_04) 
            && character.State.SetStateByAnimationTimeUpTo(animationClipInfo.nameHash, ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_LIGHT_04, 0.75f))
            return;

        // -> Idle
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInfo.nameHash, character.CurrentWeapon.IdleState, 0.9f))
            return;

        // Movement
        if (character.Animator.IsAnimationFrameBetweenTo(animationClipInfo, 0, 7))
            character.MoveController.SetMovementAndRotation(character.transform.forward, 8f * character.Status.AttackSpeed);
        else if (character.Animator.IsAnimationFrameBetweenTo(animationClipInfo, 28, 32))
            character.MoveController.SetMovementAndRotation(character.transform.forward, 12f * character.Status.AttackSpeed);
        else
            character.MoveController.SetMovementAndRotation(Vector3.zero, 0f);
    }

    public void Exit()
    {
        if (combatCoroutine != null)
            swordShield.StopCoroutine(combatCoroutine);

        swordShield.DisableShield();
        character.MoveController.SetMovementAndRotation(Vector3.zero, 0f);
    }

    private IEnumerator CoEnableCombat()
    {
        yield return new WaitUntil(() => character.Animator.IsAnimationFrameUpTo(animationClipInfo, 28) && !character.Animator.IsInTransition((int)ANIMATOR_LAYER.BASE));
        swordShield.EnableShield(COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_LIGHT_03);
        character.SFXPlayer.PlaySFX("Audio_Shield_Swing_01");

        yield return new WaitUntil(() => character.Animator.IsAnimationFrameUpTo(animationClipInfo, 39) && !character.Animator.IsInTransition((int)ANIMATOR_LAYER.BASE));
        swordShield.DisableShield();
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
