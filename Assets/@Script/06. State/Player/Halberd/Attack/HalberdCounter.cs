using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdCounter : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;

    private PlayerHalberd halberd;
    private AnimationClipInformation animationClipInfo;

    private Coroutine combatCoroutine;

    public HalberdCounter(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_SKILL_COUNTER;

        halberd = character.WeaponController.GetWeapon<PlayerHalberd>(WEAPON_TYPE.HALBERD);
        animationClipInfo = character.AnimationClipTable["Halberd_Counter"];
    }

    public void Enter()
    {
        character.Status.ConsumeStamina(Constants.PLAYER_STAMINA_CONSUMPTION_SKILL_COUNTER);
        character.SetForwardDirection(character.PlayerCamera.GetZeroYForward());
        character.Animator.CrossFadeInFixedTime(animationClipInfo.nameHash, 0.1f);

        combatCoroutine = halberd.StartCoroutine(CoEnableCombat());
    }

    public void Update()
    {
        if (character.GetInput().RollDown && character.Status.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_ROLL))
        {
            character.State.SetState(ACTION_STATE.PLAYER_ROLL, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        // -> Idle
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInfo.nameHash, ACTION_STATE.PLAYER_HALBERD_IDLE, 0.7f))
            return;

        // Movement
        if (character.Animator.IsAnimationFrameBetweenTo(animationClipInfo, 0, 23))
            character.MoveController.SetMovementAndRotation(character.transform.forward, 3f * character.Status.AttackSpeed);

        else if (character.Animator.IsAnimationFrameBetweenTo(animationClipInfo, 24, 31))
            character.MoveController.SetMovementAndRotation(character.transform.forward, 6f * character.Status.AttackSpeed);
        else
            character.MoveController.SetMovementAndRotation(Vector3.zero, 0f);
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
        yield return new WaitUntil(() => character.Animator.IsAnimationFrameUpTo(animationClipInfo, 7) && !character.Animator.IsInTransition((int)ANIMATOR_LAYER.BASE));
        halberd.EnableHalberd(COMBAT_ACTION_TYPE.HALBERD_SKILL_COUNTER);
        character.SFXPlayer.PlaySFX("Audio_Halberd_Swing_03");

        yield return new WaitUntil(() => character.Animator.IsAnimationFrameUpTo(animationClipInfo, 12) && !character.Animator.IsInTransition((int)ANIMATOR_LAYER.BASE));
        halberd.DisableHalberd();

        yield return new WaitUntil(() => character.Animator.IsAnimationFrameUpTo(animationClipInfo, 29) && !character.Animator.IsInTransition((int)ANIMATOR_LAYER.BASE));
        halberd.EnableHalberd(COMBAT_ACTION_TYPE.HALBERD_SKILL_COUNTER);
        character.SFXPlayer.PlaySFX("Audio_Halberd_Swing_04");

        yield return new WaitUntil(() => character.Animator.IsAnimationFrameUpTo(animationClipInfo, 33) && !character.Animator.IsInTransition((int)ANIMATOR_LAYER.BASE));
        halberd.DisableHalberd();
    }

    #region Property
    public int StateWeight { get => stateWeight; }
    #endregion
}
