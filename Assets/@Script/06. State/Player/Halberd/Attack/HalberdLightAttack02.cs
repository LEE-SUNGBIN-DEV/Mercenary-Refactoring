using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdLightAttack02 : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;

    private PlayerHalberd halberd;
    private AnimationClipInformation animationClipInformation;

    private bool mouseLeftDown;
    private bool mouseRightDown;

    public HalberdLightAttack02(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_ATTACK_LIGHT_02;

        halberd = character.WeaponController.GetWeapon<PlayerHalberd>(WEAPON_TYPE.HALBERD);
        animationClipInformation = character.AnimationClipTable["Halberd_Light_Attack_02"];

        mouseLeftDown = false;
        mouseRightDown = false;
    }

    public void Enter()
    {
        character.SetForwardDirection(character.PlayerCamera.GetForward(true));
        character.Animator.CrossFadeInFixedTime(animationClipInformation.nameHash, 0.25f);

        mouseLeftDown = false;
        mouseRightDown = false;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && character.Status.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_ROLL))
        {
            character.State.SetState(ACTION_STATE.PLAYER_ROLL, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && character.Status.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_SKILL_COUNTER))
        {
            character.State.SetState(ACTION_STATE.PLAYER_HALBERD_SKILL_COUNTER, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (!mouseRightDown)
            mouseRightDown = Input.GetMouseButtonDown(1);

        if (!mouseLeftDown)
            mouseLeftDown = Input.GetMouseButtonDown(0);

        // -> Smash Attack 2
        if (mouseRightDown && character.State.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, ACTION_STATE.PLAYER_HALBERD_ATTACK_HEAVY_02, 0.55f))
        {
            return;
        }

        // -> Light Attack 3
        if (mouseLeftDown && character.State.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, ACTION_STATE.PLAYER_HALBERD_ATTACK_LIGHT_03, 0.55f))
        {
            return;
        }

        // -> Idle
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, ACTION_STATE.PLAYER_HALBERD_IDLE, 0.9f))
            return;
    }

    public void Exit()
    {
        halberd.DisableHalberd();
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
