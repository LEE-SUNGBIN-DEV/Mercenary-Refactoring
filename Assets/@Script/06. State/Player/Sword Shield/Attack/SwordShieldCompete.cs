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
        character.HitState = HIT_STATE.INVINCIBLE;
        character.Animator.SetTrigger(Constants.ANIMATOR_PARAMETERS_TRIGGER_COMPETE);
        character.Animator.SetFloat(Constants.ANIMATOR_PARAMETERS_FLOAT_COMPETE, Managers.SpecialCombatManager.CompetePower);
    }

    public void Update()
    {
        character.Animator.SetFloat(Constants.ANIMATOR_PARAMETERS_FLOAT_COMPETE, Managers.SpecialCombatManager.CompetePower);
    }

    public void Exit()
    {
        character.HitState = HIT_STATE.HITTABLE;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
