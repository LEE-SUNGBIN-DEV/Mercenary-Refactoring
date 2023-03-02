using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : BaseFSM<BaseEnemy>
{
    public EnemyFSM(BaseEnemy actor) : base(actor)
    {
        stateDictionary.Add(ACTION_STATE.ENEMY_SPAWN, new EnemyStateSpawn());
        stateDictionary.Add(ACTION_STATE.ENEMY_IDLE, new EnemyStateIdle());
        stateDictionary.Add(ACTION_STATE.ENEMY_PATROL, new EnemyStatePatrol());
        stateDictionary.Add(ACTION_STATE.ENEMY_CHASE, new EnemyStateChase());
        stateDictionary.Add(ACTION_STATE.ENEMY_SKILL, new EnemyStateSkill());

        stateDictionary.Add(ACTION_STATE.ENEMY_HIT_LIGHT, new EnemyStateLightHit());
        stateDictionary.Add(ACTION_STATE.ENEMY_HIT_HEAVY, new EnemyStateHeavyHit());

        stateDictionary.Add(ACTION_STATE.ENEMY_STAGGER, new EnemyStateStagger());
        stateDictionary.Add(ACTION_STATE.ENEMY_COMPETE, new EnemyStateCompete());
        stateDictionary.Add(ACTION_STATE.ENEMY_DIE, new EnemyStateDie());

        currentState = stateDictionary[ACTION_STATE.ENEMY_IDLE];
    }
}
