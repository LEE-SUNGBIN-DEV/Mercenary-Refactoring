using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdLightAttack03 : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;

    private PlayerHalberd halberd;
    private AnimationClipInfo animationClipInfo;

    private bool mouseLeftDown;
    private bool mouseRightDown;
    private Coroutine combatCoroutine;

    public HalberdLightAttack03(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_ATTACK_LIGHT_03;

        halberd = character.UniqueEquipmentController.GetWeapon<PlayerHalberd>(WEAPON_TYPE.HALBERD);
        animationClipInfo = character.AnimationClipTable["Halberd_Light_Attack_03"];

        mouseLeftDown = false;
        mouseRightDown = false;
    }

    public void Enter()
    {
        character.StatusData.ConsumeStamina(Constants.HALBERD_STAMINA_CONSUMPTION_LIGHT_ATTACK_03);
        character.SetForwardDirection(character.PlayerCamera.GetVerticalDirection());
        character.Animator.CrossFadeInFixedTime(animationClipInfo.nameHash, 0.2f);

        mouseLeftDown = false;
        mouseRightDown = false;
        combatCoroutine = halberd.StartCoroutine(CoEnableCombat());
    }

    public void Update()
    {
        if (Managers.InputManager.CharacterRollButton.WasPressedThisFrame() && character.StatusData.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_ROLL))
        {
            character.State.SetState(ACTION_STATE.PLAYER_ROLL, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (Managers.InputManager.CharacterCounterAttackButton.WasPressedThisFrame() && character.StatusData.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_SKILL_COUNTER))
        {
            character.State.SetState(ACTION_STATE.PLAYER_HALBERD_SKILL_COUNTER, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (!mouseRightDown)
            mouseRightDown = Managers.InputManager.CharacterHeavyAttackButton.WasPressedThisFrame();

        if (!mouseLeftDown)
            mouseLeftDown = Managers.InputManager.CharacterLightAttackButton.WasPressedThisFrame();

        // -> Heavy Attack 3
        if (mouseRightDown && character.StatusData.CheckStamina(Constants.HALBERD_STAMINA_CONSUMPTION_HEAVY_ATTACK_03)
            && character.State.SetStateByAnimationTimeUpTo(animationClipInfo.nameHash, ACTION_STATE.PLAYER_HALBERD_ATTACK_HEAVY_03, 0.5f))
            return;

        // -> Light Attack 4
        if (mouseLeftDown && character.StatusData.CheckStamina(Constants.HALBERD_STAMINA_CONSUMPTION_LIGHT_ATTACK_04)
            && character.State.SetStateByAnimationTimeUpTo(animationClipInfo.nameHash, ACTION_STATE.PLAYER_HALBERD_ATTACK_LIGHT_04, 0.5f))
            return;

        // -> Idle
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInfo.nameHash, ACTION_STATE.PLAYER_HALBERD_IDLE, 0.9f))
            return;
    }

    public void Exit()
    {
        if (combatCoroutine != null)
            halberd.StopCoroutine(combatCoroutine);

        halberd.DisableHalberd();
    }

    private IEnumerator CoEnableCombat()
    {
        yield return new WaitUntil(() => character.Animator.IsAnimationFrameUpTo(animationClipInfo, 18) && !character.Animator.IsInTransition((int)ANIMATOR_LAYER.BASE));
        halberd.EnableHalberd(COMBAT_ACTION_TYPE.HALBERD_ATTACK_LIGHT_03);
        character.SFXPlayer.PlaySFX("Audio_Halberd_Swing_03");

        yield return new WaitUntil(() => character.Animator.IsAnimationFrameUpTo(animationClipInfo, 25) && !character.Animator.IsInTransition((int)ANIMATOR_LAYER.BASE));
        halberd.DisableHalberd();
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
