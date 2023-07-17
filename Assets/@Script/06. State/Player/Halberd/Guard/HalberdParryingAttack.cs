using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdParryingAttack : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;

    private PlayerHalberd halberd;
    private AnimationClipInformation animationClipInfo;

    private Coroutine combatCoroutine;

    public HalberdParryingAttack(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_PARRYING_ATTACK;

        halberd = character.WeaponController.GetWeapon<PlayerHalberd>(WEAPON_TYPE.HALBERD);
        animationClipInfo = character.AnimationClipTable["Halberd_Parrying_Attack"];
    }

    public void Enter()
    {
        character.SetForwardDirection(character.PlayerCamera.GetZeroYForward());
        character.Animator.CrossFadeInFixedTime(animationClipInfo.nameHash, 0.1f);

        combatCoroutine = halberd.StartCoroutine(CoEnableCombat());
    }

    public void Update()
    {
        if (character.Animator.IsAnimationFrameBetweenTo(animationClipInfo, 0, 22))
            character.MoveController.SetMovementAndRotation(character.transform.forward, 5f * character.Status.AttackSpeed);
        else
            character.MoveController.SetMovementAndRotation(Vector3.zero, 0f);

        // -> Idle
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInfo.nameHash, ACTION_STATE.PLAYER_HALBERD_IDLE, 0.7f))
            return;
    }

    public void Exit()
    {
        if (combatCoroutine != null)
            halberd.StopCoroutine(combatCoroutine);

        halberd.DisableHalberd();
        character.MoveController.SetMovementAndRotation(Vector3.zero, 0f);
    }

    private IEnumerator CoEnableCombat()
    {
        yield return new WaitUntil(() => character.Animator.IsAnimationFrameUpTo(animationClipInfo, 22) && !character.Animator.IsInTransition((int)ANIMATOR_LAYER.BASE));
        halberd.EnableHalberd(COMBAT_ACTION_TYPE.HALBERD_PARRYING_ATTACK);
        character.SFXPlayer.PlaySFX("Audio_Halberd_Swing_04");

        yield return new WaitUntil(() => character.Animator.IsAnimationFrameUpTo(animationClipInfo, 30) && !character.Animator.IsInTransition((int)ANIMATOR_LAYER.BASE));
        halberd.DisableHalberd();
    }
    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
