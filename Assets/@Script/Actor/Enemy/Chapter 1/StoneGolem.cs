using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StoneGolem : BaseEnemy, IStunable
{
    public override void Awake()
    {
        base.Awake();

        state.StateDictionary.Add(ACTION_STATE.ENEMY_SPAWN, new EnemyStateSpawn());
        state.StateDictionary.Add(ACTION_STATE.ENEMY_PATROL, new EnemyStatePatrol());
        state.StateDictionary.Add(ACTION_STATE.ENEMY_CHASE, new EnemyStateChaseWait());
        state.StateDictionary.Add(ACTION_STATE.ENEMY_WALK, new EnemyStateChaseWalk());
        state.StateDictionary.Add(ACTION_STATE.ENEMY_RUN, new EnemyStateChaseRun());
        state.StateDictionary.Add(ACTION_STATE.ENEMY_SKILL, new EnemyStateSkill());

        state.StateDictionary.Add(ACTION_STATE.ENEMY_HIT_LIGHT, new EnemyStateLightHit());
        state.StateDictionary.Add(ACTION_STATE.ENEMY_HIT_HEAVY, new EnemyStateHeavyHit());

        state.StateDictionary.Add(ACTION_STATE.ENEMY_STUN, new EnemyStateStun());
    }

    public override void Update()
    {
        base.Update();
    }

    #region Override Function   
    public override void OnLightHit()
    {
        state?.SetState(ACTION_STATE.ENEMY_HIT_LIGHT, STATE_SWITCH_BY.WEIGHT);
    }

    public override void OnHeavyHit()
    {
        state?.SetState(ACTION_STATE.ENEMY_HIT_HEAVY, STATE_SWITCH_BY.WEIGHT);
    }

    public virtual void OnStun(float duration)
    {
        state?.SetState(ACTION_STATE.ENEMY_STUN, STATE_SWITCH_BY.WEIGHT, duration);
    }

    public override void OnDie()
    {
        IsDie = true;
        state?.SetState(ACTION_STATE.ENEMY_DIE, STATE_SWITCH_BY.WEIGHT);

        StartCoroutine(WaitForDisapear(Constants.TIME_NORMAL_MONSTER_DISAPEAR));
    }
    #endregion
}
