using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdParryingAttack : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;

    private PlayerHalberd halberd;
    private AnimationClipInformation animationClipInformation;

    public HalberdParryingAttack(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_PARRYING_ATTACK;

        halberd = character.WeaponController.GetWeapon<PlayerHalberd>(WEAPON_TYPE.HALBERD);
        animationClipInformation = character.AnimationClipTable["Halberd_Parrying_Attack"];
    }

    public void Enter()
    {
        character.SetForwardDirection(character.PlayerCamera.GetForward(true));
        character.Animator.CrossFadeInFixedTime(animationClipInformation.nameHash, 0.1f);
    }

    public void Update()
    {
        // -> Idle
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, ACTION_STATE.PLAYER_HALBERD_IDLE, 0.7f))
        {
            return;
        }
    }

    public void Exit()
    {
        halberd.DisableHalberd();
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
