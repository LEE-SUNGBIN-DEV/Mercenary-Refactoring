using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateLightAttack01 : IActionState
{
    private BaseCharacter character;
    private int stateWeight;
    private int animationNameHash;
    private bool mouseLeftDown;
    private bool mouseRightDown;

    public CharacterStateLightAttack01(BaseCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_ATTACK_LIGHT_01;
        animationNameHash = Constants.ANIMATION_NAME_HASH_LIGHT_ATTACK_01;
        mouseLeftDown = false;
        mouseRightDown = false;
    }

    public void Enter()
    {
        mouseLeftDown = false;
        mouseRightDown = false;
        character.transform.forward = new Vector3(character.PlayerCamera.transform.forward.x, 0, character.PlayerCamera.transform.forward.z);
        character.Animator.CrossFade(animationNameHash, 0.1f);
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
            character.State.SetState(ACTION_STATE.PLAYER_SKILL_COUNTER, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (!mouseRightDown)
            mouseRightDown = Input.GetMouseButtonDown(1);

        if (!mouseLeftDown)
            mouseLeftDown = Input.GetMouseButtonDown(0);

        // -> Smash Attack 1
        if (mouseRightDown && character.State.SetStateByAnimationTimeUpTo(animationNameHash, ACTION_STATE.PLAYER_ATTACK_HEAVY_01, 0.75f))
        {
            return;
        }

        // -> Light Attack 2
        if (mouseLeftDown && character.State.SetStateByAnimationTimeUpTo(animationNameHash, ACTION_STATE.PLAYER_ATTACK_LIGHT_02, 0.75f))
        {
            return;
        }

        // -> Idle
        if (character.State.SetStateByAnimationTimeUpTo(animationNameHash, ACTION_STATE.PLAYER_IDLE, 0.9f))
            return;
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
