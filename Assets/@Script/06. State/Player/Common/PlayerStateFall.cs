using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateFall : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private int animationNameHash;

    private float slopeLimit;
    private Vector3 slideDirection;
    private RaycastHit slopeHit;
    private float slideSpeed;

    public PlayerStateFall(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_FALL;
        animationNameHash = Constants.ANIMATION_NAME_HASH_FALL;

        slideDirection = Vector3.zero;
        slideSpeed = 3f;
    }

    public void Enter()
    {
        slopeLimit = character.CharacterController.slopeLimit;
        character.Animator.Play(animationNameHash);
    }

    public void Update()
    {

        // !! When Animation is over
        if (character.State.SetStateByAnimationTimeUpTo(animationNameHash, ACTION_STATE.PLAYER_HALBERD_IDLE, 0.9f))
            return;
    }

    public void Exit()
    {
        character.IsInvincible = false;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
