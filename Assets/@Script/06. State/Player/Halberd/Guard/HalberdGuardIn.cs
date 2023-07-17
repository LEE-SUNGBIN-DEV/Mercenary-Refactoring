using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdGuardIn : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private PlayerHalberd halberd;
    private AnimationClipInformation animationClipInformation;

    public HalberdGuardIn(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_GUARD_IN;
        if (character != null)
        {
            halberd = character.WeaponController.GetWeapon<PlayerHalberd>(WEAPON_TYPE.HALBERD);
            animationClipInformation = character.AnimationClipTable["Halberd_Guard_In"];
        }
    }

    public void Enter()
    {
        character.Status.ConsumeStamina(Constants.PLAYER_STAMINA_CONSUMPTION_GUARD_IN);
        character.HitState = HIT_STATE.Parryable;
        character.SetForwardDirection(character.PlayerCamera.GetZeroYForward());
        character.Animator.Play(animationClipInformation.nameHash);
        halberd.EnableHalberd(COMBAT_ACTION_TYPE.HALBERD_GUARD_IN);
    }

    public void Update()
    {
        // -> Guard Out
        if (!character.GetInput().RightMouseHold && character.State.SetStateNotInTransition(animationClipInformation.nameHash, ACTION_STATE.PLAYER_HALBERD_GUARD_OUT))
            return;

        // -> Guard Loop
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, ACTION_STATE.PLAYER_HALBERD_GUARD_LOOP, 1f))
            return;
    }

    public void Exit()
    {
        character.HitState = HIT_STATE.Hittable;
        halberd.DisableHalberd();
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
