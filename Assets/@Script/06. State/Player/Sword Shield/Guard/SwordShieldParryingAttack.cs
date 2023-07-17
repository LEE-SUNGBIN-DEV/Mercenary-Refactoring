using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShieldParryingAttack : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;

    private PlayerSwordShield swordShield;
    private AnimationClipInformation animationClipInfo;

    private Coroutine combatCoroutine;

    public SwordShieldParryingAttack(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_PARRYING_ATTACK;

        swordShield = character.WeaponController.GetWeapon<PlayerSwordShield>(WEAPON_TYPE.SWORD_SHIELD);
        animationClipInfo = character.AnimationClipTable["Sword_Shield_Parrying_Attack"];
    }

    public void Enter()
    {
        character.SetForwardDirection(character.PlayerCamera.GetZeroYForward());
        character.Animator.CrossFadeInFixedTime(animationClipInfo.nameHash, 0.1f);

        combatCoroutine = swordShield.StartCoroutine(CoEnableCombat());
    }

    public void Update()
    {
        // -> Idle
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInfo.nameHash, ACTION_STATE.PLAYER_SWORD_SHIELD_IDLE, 0.7f))
            return;

        // Movement
        if (character.Animator.IsAnimationFrameBetweenTo(animationClipInfo, 0, 34))
            character.MoveController.SetMovementAndRotation(character.transform.forward, 5f * character.Status.AttackSpeed);
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
        yield return new WaitUntil(() => character.Animator.IsAnimationFrameUpTo(animationClipInfo, 31) && !character.Animator.IsInTransition((int)ANIMATOR_LAYER.BASE));
        swordShield.EnableShield(COMBAT_ACTION_TYPE.SWORD_SHIELD_PARRYING_ATTACK);
        character.SFXPlayer.PlaySFX("Audio_Shield_Swing_03");

        yield return new WaitUntil(() => character.Animator.IsAnimationFrameUpTo(animationClipInfo, 42) && !character.Animator.IsInTransition((int)ANIMATOR_LAYER.BASE));
        swordShield.DisableShield();
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
