using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateLightAttack03 : ICharacterState
{
    private int stateWeight;
    private int animationNameHash;
    private bool mouseLeftDown;
    private bool mouseRightDown;

    public CharacterStateLightAttack03()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Light_Attack_03;
        animationNameHash = Constants.ANIMATION_NAME_LIGHT_ATTACK_03;
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
        if (Input.GetKeyDown(KeyCode.Space) && character.StatusData.CheckStamina(Constants.CHARACTER_STAMINA_CONSUMPTION_ROLL))
        {
            character.State.TryStateSwitchingByWeight(CHARACTER_STATE.Roll);
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && character.StatusData.CheckStamina(Constants.CHARACTER_STAMINA_CONSUMPTION_SKILL_COUNTER))
        {
            character.State.TryStateSwitchingByWeight(CHARACTER_STATE.Skill);
            return;
        }

        if (!mouseRightDown)
            mouseRightDown = Input.GetMouseButtonDown(1);

        if (!mouseLeftDown)
            mouseLeftDown = Input.GetMouseButtonDown(0);

        // Move State -> Smash Attack 3
        if (mouseRightDown && character.State.SetStateByUpperAnimationTime(animationNameHash, CHARACTER_STATE.Heavy_Attack_03, 0.4f))
        {
            return;
        }

        // Move State -> Light Attack 4
        if (mouseLeftDown && character.State.SetStateByUpperAnimationTime(animationNameHash, CHARACTER_STATE.Light_Attack_04, 0.4f))
        {
            return;
        }

        // !! When animation is over
        if (character.State.SetStateByUpperAnimationTime(animationNameHash, CHARACTER_STATE.Idle, 0.9f))
            return;
    }

    public void Exit(BaseCharacter character)
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
