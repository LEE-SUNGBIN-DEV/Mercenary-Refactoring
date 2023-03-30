using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdLightAttack02 : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private int animationNameHash;
    private bool mouseLeftDown;
    private bool mouseRightDown;

    public HalberdLightAttack02(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_ATTACK_LIGHT_02;
        animationNameHash = Constants.ANIMATION_NAME_HASH_HALBERD_LIGHT_ATTACK_02;
        mouseLeftDown = false;
        mouseRightDown = false;
    }

    public void Enter()
    {
        mouseLeftDown = false;
        mouseRightDown = false;
        character.transform.forward = new Vector3(character.PlayerCamera.transform.forward.x, 0, character.PlayerCamera.transform.forward.z);
        character.Animator.CrossFadeInFixedTime(animationNameHash, 0.1f);
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
        if (mouseRightDown && character.State.SetStateByAnimationTimeUpTo(animationNameHash, ACTION_STATE.PLAYER_HALBERD_ATTACK_HEAVY_02, 0.7f))
        {
            return;
        }

        // -> Light Attack 3
        if (mouseLeftDown && character.State.SetStateByAnimationTimeUpTo(animationNameHash, ACTION_STATE.PLAYER_HALBERD_ATTACK_LIGHT_03, 0.7f))
        {
            return;
        }

        // -> Idle
        if (character.State.SetStateByAnimationTimeUpTo(animationNameHash, ACTION_STATE.PLAYER_HALBERD_IDLE, 0.9f))
            return;
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
