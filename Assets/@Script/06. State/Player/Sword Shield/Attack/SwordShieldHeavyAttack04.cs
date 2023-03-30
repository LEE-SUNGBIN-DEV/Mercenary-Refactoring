using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShieldHeavyAttack04 : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private int animationNameHash;
    private bool mouseLeftDown;

    public SwordShieldHeavyAttack04(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_ATTACK_HEAVY_04;
        animationNameHash = Constants.ANIMATION_NAME_HASH_SWORD_SHIELD_HEAVY_ATTACK_04;
        mouseLeftDown = false;
    }

    public void Enter()
    {
        mouseLeftDown = false;
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

        if (!mouseLeftDown)
            mouseLeftDown = Input.GetMouseButtonDown(0);

        // -> Light Attack 1
        if (mouseLeftDown && character.State.SetStateByAnimationTimeUpTo(animationNameHash, ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_LIGHT_04, 0.8f))
        {
            return;
        }

        // -> Idle
        if (character.State.SetStateByAnimationTimeUpTo(animationNameHash, character.CurrentWeapon.BasicStateInformation.idleState, 0.9f))
            return;
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
