using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateController
{
    protected ICharacterState prevState;
    protected ICharacterState currentState;
    protected Dictionary<CHARACTER_STATE, ICharacterState> stateDictionary;
    protected BaseCharacter character;

    public CharacterStateController(BaseCharacter character)
    {
        this.character = character;

        stateDictionary = new Dictionary<CHARACTER_STATE, ICharacterState>
        {
            // Common
            { CHARACTER_STATE.Idle, new CharacterStateIdle() },
            { CHARACTER_STATE.Walk, new CharacterStateWalk() },
            { CHARACTER_STATE.Run, new CharacterStateRun() },
            { CHARACTER_STATE.Combo_1, new CharacterStateCombo1() },
            { CHARACTER_STATE.Combo_2, new CharacterStateCombo2() },
            { CHARACTER_STATE.Combo_3, new CharacterStateCombo3() },
            { CHARACTER_STATE.Combo_4, new CharacterStateCombo4() },

            { CHARACTER_STATE.Smash_1, new CharacterStateSmash1() },
            { CHARACTER_STATE.Smash_2, new CharacterStateSmash2() },
            { CHARACTER_STATE.Smash_3, new CharacterStateSmash3() },
            { CHARACTER_STATE.Smash_4, new CharacterStateSmash4() },

            { CHARACTER_STATE.Defense, new CharacterStateDefense() },
            { CHARACTER_STATE.Defense_Breaked, new CharacterStateDefenseBreaked() },
            { CHARACTER_STATE.Parrying, new CharacterStateParrying() },
            { CHARACTER_STATE.Parrying_Attack, new CharacterStateParryingAttack() },

            { CHARACTER_STATE.Skill, new CharacterStateCounter() },

            { CHARACTER_STATE.Roll, new CharacterStateRoll() },
            { CHARACTER_STATE.StandRoll, new CharacterStateStandRoll() },

            { CHARACTER_STATE.LightHit, new CharacterStateLightHit() },
            { CHARACTER_STATE.HeavyHit, new CharacterStateHeavyHit() },

            { CHARACTER_STATE.Compete, new CharacterStateCompete() },
            { CHARACTER_STATE.Die, new CharacterStateDie() }
        };
    }

    public void Update()
    {
        currentState?.Update(character);
    }

    public void SetState(CHARACTER_STATE targetState)
    {
        prevState = currentState;
        currentState?.Exit(character);
        currentState = stateDictionary[targetState];
        currentState?.Enter(character);
    }

    public void TrySwitchState(CHARACTER_STATE targetState)
    {
        if (IsUpperStateThanCurrentState(targetState))
            SetState(targetState);
    }

    public CHARACTER_STATE CompareStateWeight(CHARACTER_STATE targetStateA, CHARACTER_STATE targetStateB)
    {
        return stateDictionary[targetStateA].StateWeight > stateDictionary[targetStateB].StateWeight ? targetStateA : targetStateB;
    }

    public bool IsUpperStateThanCurrentState(CHARACTER_STATE targetState)
    {
        return stateDictionary[targetState].StateWeight > currentState?.StateWeight;
    }

    public bool IsCurrentState(CHARACTER_STATE targetState)
    {
        return currentState == stateDictionary[targetState];
    }

    public bool IsPrevState(CHARACTER_STATE targetState)
    {
        return prevState == stateDictionary[targetState];
    }

    #region Property
    public Dictionary<CHARACTER_STATE, ICharacterState> StateDictionary { get { return stateDictionary; } }
    public ICharacterState PrevState { get { return prevState; } }
    public ICharacterState CurrentState { get { return currentState; } }
    #endregion
}
