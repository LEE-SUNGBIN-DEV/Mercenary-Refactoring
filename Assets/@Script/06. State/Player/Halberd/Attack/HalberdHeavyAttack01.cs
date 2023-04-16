using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdHeavyAttack01 : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private AnimationClipInformation animationClipInformation;
    private bool mouseLeftDown;

    public HalberdHeavyAttack01(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_ATTACK_HEAVY_01;
        animationClipInformation = character.AnimationClipDictionary["Halberd_Heavy_Attack_01"];
        mouseLeftDown = false;
    }

    public void Enter()
    {
        mouseLeftDown = false;
        character.transform.forward = new Vector3(character.PlayerCamera.transform.forward.x, 0, character.PlayerCamera.transform.forward.z);
        character.Animator.CrossFadeInFixedTime(animationClipInformation.nameHash, 0.2f);
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

        if (!mouseLeftDown)
            mouseLeftDown = Input.GetMouseButtonDown(0);

        if(character.Animator.IsAnimationFrameUpTo(animationClipInformation, 24))
        {

        }

        // -> Light Attack 1
        if (mouseLeftDown && character.State.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, ACTION_STATE.PLAYER_HALBERD_ATTACK_LIGHT_01, 0.9f))
        {
            return;
        }

        // -> Idle
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, ACTION_STATE.PLAYER_HALBERD_IDLE, 0.9f))
            return;
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
