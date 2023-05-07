using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdCounter : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;

    private PlayerHalberd halberd;
    private AnimationClipInformation animationClipInformation;

    private Vector3 moveDirection;

    public HalberdCounter(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_SKILL_COUNTER;

        halberd = character.WeaponController.GetWeapon<PlayerHalberd>(WEAPON_TYPE.HALBERD);
        animationClipInformation = character.AnimationClipTable["Halberd_Counter"];
    }

    public void Enter()
    {
        character.Status.CurrentSP -= Constants.PLAYER_STAMINA_CONSUMPTION_SKILL_COUNTER;
        character.SetForwardDirection(character.PlayerCamera.GetForward(true));
        character.Animator.CrossFadeInFixedTime(animationClipInformation.nameHash, 0.1f);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && character.Status.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_ROLL))
        {
            character.State.SetState(ACTION_STATE.PLAYER_ROLL, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        // !! When animation is over
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, ACTION_STATE.PLAYER_HALBERD_IDLE, 0.7f))
            return;
    }

    public void Exit()
    {
        halberd.DisableHalberd();
    }

    #region Property
    public int StateWeight { get => stateWeight; }
    #endregion
}
