using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateHeavyAttack02 : IActionState
{
    private BaseCharacter character;
    private int stateWeight;
    private int animationNameHash;
    private bool mouseLeftDown;

    public CharacterStateHeavyAttack02(BaseCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_ATTACK_HEAVY_02;
        animationNameHash = Constants.ANIMATION_NAME_HASH_HEAVY_ATTACK_02;
        mouseLeftDown = false;
    }

    public void Enter()
    {
        mouseLeftDown = false;
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

        if (!mouseLeftDown)
            mouseLeftDown = Input.GetMouseButtonDown(0);

        // -> Light Attack 1
        if (mouseLeftDown && character.State.SetStateByAnimationTimeUpTo(animationNameHash, ACTION_STATE.PLAYER_ATTACK_LIGHT_01, 0.8f))
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
