using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateHeavyAttack04 : ICharacterState
{
    private int stateWeight;
    private int animationNameHash;
    private bool mouseLeftDown;

    public CharacterStateHeavyAttack04()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Heavy_Attack_04;
        animationNameHash = Constants.ANIMATION_NAME_HEAVY_ATTACK_04;
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

        if (!mouseLeftDown)
            mouseLeftDown = Input.GetMouseButtonDown(0);

        // Move State -> Light Attack 1
        if (mouseLeftDown && character.State.SetStateByUpperAnimationTime(animationNameHash, CHARACTER_STATE.Light_Attack_01, 0.8f))
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
