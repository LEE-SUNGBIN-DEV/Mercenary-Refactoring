using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateCompete : ICharacterState
{
    private int stateWeight;
    private float competeTime;
    private float inputTime;

    public CharacterStateCompete()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Compete;
        competeTime = Constants.TIME_COMPETE;
        inputTime = 0f;
    }

    public void Enter(BaseCharacter character)
    {
        // Effect
        //Managers.SceneManagerCS.CurrentScene.RequestObject("Prefab_Effect_Player_Compete_Start").transform.position = character.transform.position;

        // Set Compete State
        character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_COMPETE, true);
        inputTime = 0f;
    }

    public void Update(BaseCharacter character)
    {
        if(inputTime < competeTime)
        {
            inputTime += Time.deltaTime;
        }

        else
        {
            //shield.gameObject.SetActive(false);

        }
    }

    public void Exit(BaseCharacter character)
    {
        character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_COMPETE, false);
    }

    #region Property
    public int StateWeight { get => stateWeight; }
    #endregion
}
