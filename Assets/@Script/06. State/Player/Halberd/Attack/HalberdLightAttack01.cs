using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdLightAttack01 : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;

    private PlayerHalberd halberd;
    private AnimationClipInformation animationClipInfo;

    private bool mouseLeftDown;
    private bool mouseRightDown;
    private Coroutine combatCoroutine;

    public HalberdLightAttack01(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_ATTACK_LIGHT_01;

        halberd = character.WeaponController.GetWeapon<PlayerHalberd>(WEAPON_TYPE.HALBERD);
        animationClipInfo = character.AnimationClipTable["Halberd_Light_Attack_01"];

        mouseLeftDown = false;
        mouseRightDown = false;
    }

    public void Enter()
    {
        character.Status.ConsumeStamina(Constants.HALBERD_STAMINA_CONSUMPTION_LIGHT_ATTACK_01);
        character.SetForwardDirection(character.PlayerCamera.GetZeroYForward());
        character.Animator.CrossFadeInFixedTime(animationClipInfo.nameHash, 0.2f);

        mouseLeftDown = false;
        mouseRightDown = false;
        combatCoroutine = halberd.StartCoroutine(CoEnableCombat());
    }

    public void Update()
    {
        // Input
        if (character.GetInput().RollDown && character.Status.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_ROLL))
        {
            character.State.SetState(ACTION_STATE.PLAYER_ROLL, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (character.GetInput().CounterDown && character.Status.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_SKILL_COUNTER))
        {
            character.State.SetState(ACTION_STATE.PLAYER_HALBERD_SKILL_COUNTER, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (!mouseRightDown)
            mouseRightDown = character.GetInput().RightMouseDown;

        if (!mouseLeftDown)
            mouseLeftDown = character.GetInput().LeftMouseDown;

        // -> Heavy Attack 1
        if (mouseRightDown && character.Status.CheckStamina(Constants.HALBERD_STAMINA_CONSUMPTION_HEAVY_ATTACK_01)
            && character.State.SetStateByAnimationTimeUpTo(animationClipInfo.nameHash, ACTION_STATE.PLAYER_HALBERD_ATTACK_HEAVY_01, 0.5f))
            return;

        // -> Light Attack 2
        if (mouseLeftDown && character.Status.CheckStamina(Constants.HALBERD_STAMINA_CONSUMPTION_LIGHT_ATTACK_02) 
            && character.State.SetStateByAnimationTimeUpTo(animationClipInfo.nameHash, ACTION_STATE.PLAYER_HALBERD_ATTACK_LIGHT_02, 0.5f))
            return;

        // -> Idle
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInfo.nameHash, ACTION_STATE.PLAYER_HALBERD_IDLE, 0.9f))
            return;

        // Movement
        if (character.Animator.IsAnimationFrameBetweenTo(animationClipInfo, 0, 16))
            character.MoveController.SetMovementAndRotation(character.transform.forward, 4f * character.Status.AttackSpeed);
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
        yield return new WaitUntil(() => character.Animator.IsAnimationFrameUpTo(animationClipInfo, 13) && !character.Animator.IsInTransition((int)ANIMATOR_LAYER.BASE));
        halberd.EnableHalberd(COMBAT_ACTION_TYPE.HALBERD_ATTACK_LIGHT_01);
        character.SFXPlayer.PlaySFX("Audio_Halberd_Sting_01");

        yield return new WaitUntil(() => character.Animator.IsAnimationFrameUpTo(animationClipInfo, 19) && !character.Animator.IsInTransition((int)ANIMATOR_LAYER.BASE));
        halberd.DisableHalberd();
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
