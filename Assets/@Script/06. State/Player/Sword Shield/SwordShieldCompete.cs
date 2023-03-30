using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShieldCompete : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;

    public SwordShieldCompete(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_COMPETE;
    }

    public void Enter()
    {
        // Set Compete State
        character.IsInvincible = true;
        character.Animator.SetTrigger(Constants.ANIMATOR_PARAMETERS_TRIGGER_COMPETE);
        character.Animator.SetFloat(Constants.ANIMATOR_PARAMETERS_FLOAT_COMPETE, Managers.CompeteManager.CompetePower);
    }

    public void Update()
    {
        character.Animator.SetFloat(Constants.ANIMATOR_PARAMETERS_FLOAT_COMPETE, Managers.CompeteManager.CompetePower);

        if (character.Animator.GetNextAnimatorStateInfo(0).IsName(Constants.ANIMATOR_STATE_NAME_MOVE_BLEND_TREE))
            character.State.SetState(ACTION_STATE.PLAYER_HALBERD_WALK, STATE_SWITCH_BY.FORCED);
    }

    public void Exit()
    {
        character.IsInvincible = false;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
