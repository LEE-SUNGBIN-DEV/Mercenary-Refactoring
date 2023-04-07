using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateFall : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private int animationNameHash;

    private float fallTime;

    public PlayerStateFall(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_FALL;
        animationNameHash = Constants.ANIMATION_NAME_HASH_FALL;

        fallTime = 0f;
    }

    public void Enter()
    {
        character.Animator.Play(animationNameHash);
        fallTime = 0f;
    }

    public void Update()
    {
        switch (character.GetGroundState())
        {
            case ACTOR_GROUND_STATE.GROUND:
                character.FallDamageProcess(fallTime);
                character.State.SetState(ACTION_STATE.PLAYER_LANDING, STATE_SWITCH_BY.WEIGHT);
                break;

            case ACTOR_GROUND_STATE.SLOPE:
            case ACTOR_GROUND_STATE.AIR:
                fallTime += Time.deltaTime;
                break;
        }

        if (fallTime > 2.5f)
            character.Status.CurrentHP = 0f;
    }

    public void Exit()
    {
        fallTime = 0f;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
