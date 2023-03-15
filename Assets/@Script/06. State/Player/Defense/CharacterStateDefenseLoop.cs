using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateDefenseLoop : IActionState
{
    private BaseCharacter character;
    private int stateWeight;
    private int animationNameHash;

    public CharacterStateDefenseLoop(BaseCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_DEFENSE_LOOP;
        animationNameHash = Constants.ANIMATION_NAME_HASH_DEFENSE_LOOP;
    }

    public void Enter()
    {
        character.IsInvincible = true;
        character.Shield.OnEnableDefense(COMBAT_TYPE.DEFENSE);
        character.Animator.Play(animationNameHash);
    }

    public void Update()
    {
        if (!Input.GetMouseButton(1) && character.State.SetStateNotInTransition(animationNameHash, ACTION_STATE.PLAYER_DEFENSE_END))
        {
            return;
        }
    }

    public void Exit()
    {
        character.IsInvincible = false;
        character.Shield.OnDisableDefense();
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
