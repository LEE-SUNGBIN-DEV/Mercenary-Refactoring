using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdHeavyAttack01 : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;

    private PlayerHalberd halberd;
    private AnimationClipInfo animationClipInfo;

    private bool mouseLeftDown;
    private Coroutine combatCoroutine;

    public HalberdHeavyAttack01(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_ATTACK_HEAVY_01;

        halberd = character.UniqueEquipmentController.GetWeapon<PlayerHalberd>(WEAPON_TYPE.HALBERD);
        animationClipInfo = character.AnimationClipTable["Halberd_Heavy_Attack_01"];

        mouseLeftDown = false;
    }

    public void Enter()
    {
        character.StatusData.ConsumeStamina(Constants.HALBERD_STAMINA_CONSUMPTION_HEAVY_ATTACK_01);
        character.SetForwardDirection(character.PlayerCamera.GetVerticalDirection());
        character.Animator.CrossFadeInFixedTime(animationClipInfo.nameHash, 0.2f);

        mouseLeftDown = false;
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

        if (!mouseLeftDown)
            mouseLeftDown = Managers.InputManager.CharacterLightAttackButton.WasPressedThisFrame();

        // -> Light Attack 1
        if (mouseLeftDown && character.StatusData.CheckStamina(Constants.HALBERD_STAMINA_CONSUMPTION_LIGHT_ATTACK_01) 
            && character.State.SetStateByAnimationTimeUpTo(animationClipInfo.nameHash, ACTION_STATE.PLAYER_HALBERD_ATTACK_LIGHT_01, 0.75f))
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
        yield return new WaitUntil(() => character.Animator.IsAnimationFrameUpTo(animationClipInfo, 27) && !character.Animator.IsInTransition((int)ANIMATOR_LAYER.BASE));
        halberd.EnableHalberd(COMBAT_ACTION_TYPE.HALBERD_ATTACK_HEAVY_01);
        character.SFXPlayer.PlaySFX("Audio_Halberd_Swing_01");

        yield return new WaitUntil(() => character.Animator.IsAnimationFrameUpTo(animationClipInfo, 35) && !character.Animator.IsInTransition((int)ANIMATOR_LAYER.BASE));
        halberd.DisableHalberd();
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
