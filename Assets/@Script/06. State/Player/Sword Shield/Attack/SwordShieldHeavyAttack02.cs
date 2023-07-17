using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShieldHeavyAttack02 : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;

    private PlayerSwordShield swordShield;
    private AnimationClipInformation animationClipInfo;

    private bool mouseLeftDown;
    private Coroutine combatCoroutine;

    public SwordShieldHeavyAttack02(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_ATTACK_HEAVY_02;

        swordShield = character.WeaponController.GetWeapon<PlayerSwordShield>(WEAPON_TYPE.SWORD_SHIELD);
        animationClipInfo = character.AnimationClipTable["Sword_Shield_Heavy_Attack_02"];

        mouseLeftDown = false;
    }

    public void Enter()
    {
        character.Status.ConsumeStamina(Constants.SWORD_SHIELD_STAMINA_CONSUMPTION_HEAVY_ATTACK_02);
        character.SetForwardDirection(character.PlayerCamera.GetZeroYForward());
        character.Animator.CrossFadeInFixedTime(animationClipInfo.nameHash, 0.1f);

        mouseLeftDown = false;
        combatCoroutine = swordShield.StartCoroutine(CoEnableCombat());
    }

    public void Update()
    {
        if (character.GetInput().RollDown && character.Status.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_ROLL))
        {
            character.State.SetState(ACTION_STATE.PLAYER_ROLL, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (!mouseLeftDown)
            mouseLeftDown = character.GetInput().LeftMouseDown;

        // -> Light Attack 1
        if (mouseLeftDown && character.Status.CheckStamina(Constants.SWORD_SHIELD_STAMINA_CONSUMPTION_LIGHT_ATTACK_01) 
            && character.State.SetStateByAnimationTimeUpTo(animationClipInfo.nameHash, ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_LIGHT_01, 0.8f))
            return;

        // -> Idle
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInfo.nameHash, character.CurrentWeapon.IdleState, 0.9f))
            return;

        // Movement
        if (character.Animator.IsAnimationFrameBetweenTo(animationClipInfo, 0, 21))
            character.MoveController.SetMovementAndRotation(character.transform.forward, 6f * character.Status.AttackSpeed);
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
        yield return new WaitUntil(() => character.Animator.IsAnimationFrameUpTo(animationClipInfo, 22) && !character.Animator.IsInTransition((int)ANIMATOR_LAYER.BASE));
        swordShield.EnableSword(COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_HEAVY_02);
        character.SFXPlayer.PlaySFX("Audio_Sword_Swing_02");

        yield return new WaitUntil(() => character.Animator.IsAnimationFrameUpTo(animationClipInfo, 30) && !character.Animator.IsInTransition((int)ANIMATOR_LAYER.BASE));
        swordShield.DisableSword();
    }


    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
