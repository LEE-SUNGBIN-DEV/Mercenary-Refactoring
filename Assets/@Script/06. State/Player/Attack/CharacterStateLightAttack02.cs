using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateLightAttack02 : IActionState<BaseCharacter>
{
    private int stateWeight;
    private int animationNameHash;
    private bool mouseLeftDown;
    private bool mouseRightDown;

    public CharacterStateLightAttack02()
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_ATTACK_LIGHT_02;
        animationNameHash = Constants.ANIMATION_NAME_HASH_LIGHT_ATTACK_02;
        mouseLeftDown = false;
        mouseRightDown = false;
    }

    public void Enter(BaseCharacter character)
    {
        mouseLeftDown = false;
        mouseRightDown = false;
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

        if (!mouseRightDown)
            mouseRightDown = Input.GetMouseButtonDown(1);

        if (!mouseLeftDown)
            mouseLeftDown = Input.GetMouseButtonDown(0);

        // Move State -> Smash Attack 2
        if (mouseRightDown && character.State.SetStateByUpperAnimationTime(animationNameHash, ACTION_STATE.PLAYER_ATTACK_HEAVY_02, 0.4f))
        {
            return;
        }

        // Move State -> Light Attack 3
        if (mouseLeftDown && character.State.SetStateByUpperAnimationTime(animationNameHash, ACTION_STATE.PLAYER_ATTACK_LIGHT_03, 0.4f))
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