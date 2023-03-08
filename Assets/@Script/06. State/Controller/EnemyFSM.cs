using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyFSM : BaseFSM<BaseEnemy>
{
    public EnemyFSM(BaseEnemy actor) : base(actor)
    {
        stateDictionary.Add(ACTION_STATE.ENEMY_IDLE, new EnemyStateIdle());
        stateDictionary.Add(ACTION_STATE.ENEMY_DIE, new EnemyStateDie());


        stateDictionary.Add(ACTION_STATE.ENEMY_STAGGER, new EnemyStateStagger());
        stateDictionary.Add(ACTION_STATE.ENEMY_COMPETE, new EnemyStateCompete());

        currentState = stateDictionary[ACTION_STATE.ENEMY_IDLE];
    }
}
