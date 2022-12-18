using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateCompete : ICharacterState
{
    private int stateWeight;
    private float competeTime;

    public CharacterStateCompete()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Compete;
    }

    public void Enter(BaseCharacter character)
    {
        // Effect
        Managers.SceneManagerCS.CurrentScene.RequestObject("Prefab_Effect_Player_Compete_Start").transform.position = character.transform.position;

        // Set Compete State
        character.Animator.SetTrigger(Constants.ANIMATOR_PARAMETERS_TRIGGER_COMPETE);
        competeTime = 0f;
    }

    public void Update(BaseCharacter character)
    {
        if(competeTime < Constants.TIME_COMPETE)
        {
            competeTime += Time.deltaTime;
        }

        else
        {
            //shield.gameObject.SetActive(false);

        }
    }

    public void Exit(BaseCharacter character)
    {
        character.Animator.SetTrigger(Constants.ANIMATOR_PARAMETERS_TRIGGER_COMPETE);
    }

    #region Property
    public int StateWeight { get => stateWeight; }
    #endregion
}
