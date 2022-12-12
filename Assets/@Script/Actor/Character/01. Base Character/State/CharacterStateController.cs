using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateController
{
    [SerializeField] protected BaseCharacter character;
    [SerializeField] protected ICharacterState currentState;
    protected Dictionary<CHARACTER_STATE, ICharacterState> stateDictionary;

    public CharacterStateController(BaseCharacter character)
    {
        this.character = character;

        stateDictionary = new Dictionary<CHARACTER_STATE, ICharacterState>
        {
            // Common
            { CHARACTER_STATE.Move, new CharacterStateMove() },
            { CHARACTER_STATE.Attack, new CharacterStateAttack() },
            { CHARACTER_STATE.Skill, new CharacterStateCounter() },
            { CHARACTER_STATE.Roll, new CharacterStateRoll() },
            { CHARACTER_STATE.Hit, new CharacterStateHit() },
            { CHARACTER_STATE.HeavyHit, new CharacterStateHeavyHit() },
            { CHARACTER_STATE.Stun, new CharacterStateStun() },
            { CHARACTER_STATE.Compete, new CharacterStateCompete() },
            { CHARACTER_STATE.Die, new CharacterStateDie() }
        };
    }

    public void SwitchCharacterState(CHARACTER_STATE targetState)
    {
        currentState?.Exit(character);
        currentState = stateDictionary[targetState];
        currentState?.Enter(character);
    }

    public void SwitchCharacterStateByWeight(CHARACTER_STATE targetState)
    {
        if (currentState?.StateWeight < stateDictionary[targetState].StateWeight)
        {
            SwitchCharacterState(targetState);
        }
    }

    public CHARACTER_STATE CompareStateWeight(CHARACTER_STATE targetStateA, CHARACTER_STATE targetStateB)
    {
        return stateDictionary[targetStateA].StateWeight > stateDictionary[targetStateB].StateWeight ? targetStateA : targetStateB;
    }

    #region Property
    public ICharacterState CurrentState
    {
        get => currentState;
    }
    public Dictionary<CHARACTER_STATE, ICharacterState> StateDictionary
    {
        get => stateDictionary;
    }
    #endregion
}
