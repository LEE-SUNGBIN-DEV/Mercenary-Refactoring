using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateHeavyAttack01 : IActionState<BaseCharacter>
{
    private int stateWeight;
    private int animationNameHash;
    private bool mouseLeftDown;

    public CharacterStateHeavyAttack01()
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_ATTACK_HEAVY_01;
        animationNameHash = Constants.ANIMATION_NAME_HASH_HEAVY_ATTACK_01;
        mouseLeftDown = false;
    }

    public void Enter(BaseCharacter character)
    {
        mouseLeftDown = false;
        character.transform.forward = new Vector3(character.PlayerCamera.transform.forward.x, 0, character.PlayerCamera.transform.forward.z);
        character.Animator.CrossFade(animationNameHash, 0.1f);
    }

    public void Update(BaseCharacter character)
    {
        if (Input.GetKeyDown(KeyCode.Space) && character.Status.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_ROLL))
        {
            character.State.TryStateSwitchingByWeight(ACTION_STATE.PLAYER_ROLL);
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && character.Status.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_SKILL_COUNTER))
        {
            character.State.TryStateSwitchingByWeight(ACTION_STATE.PLAYER_SKILL_COUNTER);
            return;
        }

        if (!mouseLeftDown)
            mouseLeftDown = Input.GetMouseButtonDown(0);

        // Move State -> Light Attack 1
        if (mouseLeftDown && character.State.SetStateByUpperAnimationTime(animationNameHash, ACTION_STATE.PLAYER_ATTACK_LIGHT_01, 0.8f))
        {
            return;
        }

        // !! When animation is over
        if (character.State.SetStateByUpperAnimationTime(animationNameHash, ACTION_STATE.PLAYER_IDLE, 0.9f))
            return;
    }

    public void Exit(BaseCharacter character)
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
