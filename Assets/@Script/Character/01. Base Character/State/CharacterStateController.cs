using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateController
{
    [SerializeField] protected Character character;
    [SerializeField] protected ICharacterState currentState;
    protected Dictionary<CHARACTER_STATE, ICharacterState> stateDictionary;

    public CharacterStateController(Character character)
    {
        this.character = character;

        stateDictionary = new Dictionary<CHARACTER_STATE, ICharacterState>
        {
            // Common
            { CHARACTER_STATE.MOVE, new CharacterStateMove() },
            { CHARACTER_STATE.ATTACK, new CharacterStateAttack() },
            { CHARACTER_STATE.SKILL, new CharacterStateSkill() },
            { CHARACTER_STATE.ROLL, new CharacterStateRoll() },
            { CHARACTER_STATE.HIT, new CharacterStateHit() },
            { CHARACTER_STATE.HEAVY_HIT, new CharacterStateHeavyHit() },
            { CHARACTER_STATE.STUN, new CharacterStateStun() },
            { CHARACTER_STATE.COMPETE, new CharacterStateCompete() },
            { CHARACTER_STATE.DIE, new CharacterStateDie() }
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
