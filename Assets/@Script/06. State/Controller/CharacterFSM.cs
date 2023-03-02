using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFSM : BaseFSM<BaseCharacter>
{
    public CharacterFSM(BaseCharacter actor) : base(actor)
    {
        stateDictionary.Add(ACTION_STATE.PLAYER_IDLE, new CharacterStateIdle());
        stateDictionary.Add(ACTION_STATE.PLAYER_WALK, new CharacterStateWalk());
        stateDictionary.Add(ACTION_STATE.PLAYER_RUN, new CharacterStateRun());

        stateDictionary.Add(ACTION_STATE.PLAYER_ATTACK_LIGHT_01, new CharacterStateLightAttack01());
        stateDictionary.Add(ACTION_STATE.PLAYER_ATTACK_LIGHT_02, new CharacterStateLightAttack02());
        stateDictionary.Add(ACTION_STATE.PLAYER_ATTACK_LIGHT_03, new CharacterStateLightAttack03());
        stateDictionary.Add(ACTION_STATE.PLAYER_ATTACK_LIGHT_04, new CharacterStateLightAttack04());
        stateDictionary.Add(ACTION_STATE.PLAYER_ATTACK_HEAVY_01, new CharacterStateHeavyAttack01());
        stateDictionary.Add(ACTION_STATE.PLAYER_ATTACK_HEAVY_02, new CharacterStateHeavyAttack02());
        stateDictionary.Add(ACTION_STATE.PLAYER_ATTACK_HEAVY_03, new CharacterStateHeavyAttack03());
        stateDictionary.Add(ACTION_STATE.PLAYER_ATTACK_HEAVY_04, new CharacterStateHeavyAttack04());

        stateDictionary.Add(ACTION_STATE.PLAYER_DEFENSE_START, new CharacterStateDefense());
        stateDictionary.Add(ACTION_STATE.PLAYER_DEFENSE_LOOP, new CharacterStateDefenseLoop());
        stateDictionary.Add(ACTION_STATE.PLAYER_DEFENSE_END, new CharacterStateDefenseEnd());
        stateDictionary.Add(ACTION_STATE.PLAYER_DEFENSE_BREAK, new CharacterStateDefenseBreak());
        stateDictionary.Add(ACTION_STATE.PLAYER_PARRYING, new CharacterStateParrying());
        stateDictionary.Add(ACTION_STATE.PLAYER_PARRYING_ATTACK, new CharacterStateParryingAttack());

        stateDictionary.Add(ACTION_STATE.PLAYER_SKILL_COUNTER, new CharacterStateSkillCounter());
        stateDictionary.Add(ACTION_STATE.PLAYER_ROLL, new CharacterStateRoll());

        stateDictionary.Add(ACTION_STATE.PLAYER_HIT_LIGHT, new CharacterStateLightHit());
        stateDictionary.Add(ACTION_STATE.PLAYER_HIT_HEAVY, new CharacterStateHeavyHit());
        stateDictionary.Add(ACTION_STATE.PLAYER_HIT_HEAVY_LOOP, new CharacterStateHeavyHitLoop());
        stateDictionary.Add(ACTION_STATE.PLAYER_STAND_UP, new CharacterStateStandRoll());
        stateDictionary.Add(ACTION_STATE.PLAYER_STAND_ROLL, new CharacterStateStandRoll());

        stateDictionary.Add(ACTION_STATE.PLAYER_STUN, new CharacterStateStun());

        stateDictionary.Add(ACTION_STATE.PLAYER_COMPETE, new CharacterStateCompete());
        stateDictionary.Add(ACTION_STATE.PLAYER_DIE, new CharacterStateDie());

        currentState = stateDictionary[ACTION_STATE.PLAYER_IDLE];
    }
}
