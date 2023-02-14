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

            { CHARACTER_STATE.Light_Attack_01, new CharacterStateLightAttack01() },
            { CHARACTER_STATE.Light_Attack_02, new CharacterStateLightAttack02() },
            { CHARACTER_STATE.Light_Attack_03, new CharacterStateLightAttack03() },
            { CHARACTER_STATE.Light_Attack_04, new CharacterStateLightAttack04() },

            { CHARACTER_STATE.Heavy_Attack_01, new CharacterStateHeavyAttack01() },
            { CHARACTER_STATE.Heavy_Attack_02, new CharacterStateHeavyAttack02() },
            { CHARACTER_STATE.Heavy_Attack_03, new CharacterStateHeavyAttack03() },
            { CHARACTER_STATE.Heavy_Attack_04, new CharacterStateHeavyAttack04() },

            { CHARACTER_STATE.Defense, new CharacterStateDefense() },
            { CHARACTER_STATE.Defense_Loop, new CharacterStateDefenseLoop() },
            { CHARACTER_STATE.Defense_End, new CharacterStateDefenseEnd() },
            { CHARACTER_STATE.Defense_Breaked, new CharacterStateDefenseBreak() },
            { CHARACTER_STATE.Parrying, new CharacterStateParrying() },
            { CHARACTER_STATE.Parrying_Attack, new CharacterStateParryingAttack() },

            { CHARACTER_STATE.Skill, new CharacterStateSkillCounter() },

            { CHARACTER_STATE.Roll, new CharacterStateRoll() },

            { CHARACTER_STATE.Light_Hit, new CharacterStateLightHit() },
            { CHARACTER_STATE.Heavy_Hit, new CharacterStateHeavyHit() },
            { CHARACTER_STATE.Heavy_Hit_Loop, new CharacterStateHeavyHitLoop() },
            { CHARACTER_STATE.Stand_Up, new CharacterStateStandRoll() },
            { CHARACTER_STATE.Stand_Roll, new CharacterStateStandRoll() },

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

    public void TryStateSwitchingByWeight(CHARACTER_STATE targetState)
    {
        if (stateDictionary[targetState].StateWeight > currentState?.StateWeight)
            SetState(targetState);
    }

    public CHARACTER_STATE CompareStateWeight(CHARACTER_STATE targetStateA, CHARACTER_STATE targetStateB)
    {
        return stateDictionary[targetStateA].StateWeight > stateDictionary[targetStateB].StateWeight ? targetStateA : targetStateB;
    }

    public bool IsCurrentState(CHARACTER_STATE targetState)
    {
        return currentState == stateDictionary[targetState];
    }

    public bool IsPrevState(CHARACTER_STATE targetState)
    {
        return prevState == stateDictionary[targetState];
    }

    public bool SetStateNotInTransition(int currentNameHash, CHARACTER_STATE targetState)
    {
        if (character.Animator.GetCurrentAnimatorStateInfo(0).shortNameHash == currentNameHash
            && !character.Animator.IsInTransition(0))
        {
            SetState(targetState);
            return true;
        }
        return false;
    }

    public bool SetStateByUpperAnimationTime(int currentNameHash, CHARACTER_STATE targetState, float normalizedTime)
    {
        if (character.Animator.GetCurrentAnimatorStateInfo(0).shortNameHash == currentNameHash
            && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= normalizedTime
            && !character.Animator.IsInTransition(0))
        {
            SetState(targetState);
            return true;
        }
        return false;
    }
    public bool SetStateByLowerAnimationTime(int currentNameHash, CHARACTER_STATE targetState, float normalizedTime)
    {
        if (character.Animator.GetCurrentAnimatorStateInfo(0).shortNameHash == currentNameHash
            && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= normalizedTime
            && !character.Animator.IsInTransition(0))
        {
            SetState(targetState);
            return true;
        }
        return false;
    }
    public bool SetStateByBetweenAnimationTime(int currentNameHash, CHARACTER_STATE targetState, float lowerNormalizedTime, float upperNormalizedTime)
    {
        if (character.Animator.GetCurrentAnimatorStateInfo(0).shortNameHash == currentNameHash
            && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= lowerNormalizedTime
            && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= upperNormalizedTime
            && !character.Animator.IsInTransition(0))
        {
            SetState(targetState);
            return true;
        }
        return false;
    }

    #region Property
    public Dictionary<CHARACTER_STATE, ICharacterState> StateDictionary { get { return stateDictionary; } }
    public ICharacterState PrevState { get { return prevState; } }
    public ICharacterState CurrentState { get { return currentState; } }
    #endregion
}
