using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateLanding : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private int animationNameHash;

    public PlayerStateLanding(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_LANDING;
        animationNameHash = Constants.ANIMATION_NAME_HASH_LANDING;
    }

    public void Enter()
    {
        character.Animator.Play(animationNameHash);
    }

    public void Update()
    {
        // !! When animation is over
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
