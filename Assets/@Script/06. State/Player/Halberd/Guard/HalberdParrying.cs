using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdParrying : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;

    private AnimationClipInfo animationClipInformation;
    private bool mouseRightDown;

    public HalberdParrying(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_PARRYING;

        animationClipInformation = character.AnimationClipTable["Halberd_Parrying"];
        mouseRightDown = false;
    }

    public void Enter()
    {
        character.StatusData.RecoverStamina(Constants.PLAYER_STAMINA_PARRYING_RECOVERY_RATIO, VALUE_TYPE.PERCENTAGE);
        character.SFXPlayer.PlaySFX(Constants.Audio_Halberd_Parrying);
        character.Animator.Play(animationClipInformation.nameHash);

        mouseRightDown = false;
        character.HitState = HIT_STATE.INVINCIBLE;
    }

    public void Update()
    {
        if (!mouseRightDown)
            mouseRightDown = Managers.InputManager.CharacterParryingAttackButton.WasPressedThisFrame();

        // -> Parrying Attack
        if (mouseRightDown && character.State.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, ACTION_STATE.PLAYER_HALBERD_PARRYING_ATTACK, 0.5f))
            return;

        // -> Idle
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, ACTION_STATE.PLAYER_HALBERD_IDLE, 1f))
            return;
    }
    public void Exit()
    {
        character.HitState = HIT_STATE.HITTABLE;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
