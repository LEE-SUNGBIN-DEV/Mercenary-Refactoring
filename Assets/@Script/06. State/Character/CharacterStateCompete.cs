using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateCompete : ICharacterState
{
    private int stateWeight;

    public CharacterStateCompete()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Compete;
    }

    public void Enter(BaseCharacter character)
    {
        // Set Compete State
        character.Animator.SetFloat(Constants.ANIMATOR_PARAMETERS_FLOAT_COMPETE, Managers.CompeteManager.CompetePower);
        character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_COMPETE, true);
    }

    public void Update(BaseCharacter character)
    {
        Managers.CompeteManager.CompetePower -= (0.3f * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.A))
            Managers.CompeteManager.CompetePower += 0.06f;

        if (Input.GetKeyDown(KeyCode.D))
            Managers.CompeteManager.CompetePower += 0.06f;

        if (Managers.CompeteManager.CumulativeTime < Managers.CompeteManager.CompeteDuration)
            Managers.CompeteManager.CumulativeTime += Time.deltaTime;

        // Compete Success Condition
        if(Managers.CompeteManager.CompetePower >= 1.0f)
            Managers.CompeteManager.OnSuccessCompete();

        // Compete Fail Condition
        if(Managers.CompeteManager.CompetePower <= 0f || Managers.CompeteManager.CumulativeTime >= Managers.CompeteManager.CompeteDuration)
            Managers.CompeteManager.OnFailCompete();

        character.Animator.SetFloat(Constants.ANIMATOR_PARAMETERS_FLOAT_COMPETE, Managers.CompeteManager.CompetePower);
    }

    public void Exit(BaseCharacter character)
    {
        character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_COMPETE, false);
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
