using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateStandUp : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private int animationNameHash;

    public PlayerStateStandUp(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_STAND_UP;
        animationNameHash = Constants.ANIMATION_NAME_HASH_STAND_UP;
    }

    public void Enter()
    {
        character.Animator.Play(animationNameHash);
        //character.IsInvincible = true;
    }

    public void Update()
    {
        // !! When animation is over
        if (character.State.SetStateByAnimationTimeUpTo(animationNameHash, character.CurrentWeapon.BasicStateInformation.idleState, 1.0f))
            return;
    }

    public void Exit()
    {
        //character.IsInvincible = false;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
