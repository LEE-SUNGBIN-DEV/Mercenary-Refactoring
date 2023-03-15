using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SmallStoneGolem : BaseEnemy, IStunable
{
    public override void Awake()
    {
        base.Awake();

        state.StateDictionary.Add(ACTION_STATE.ENEMY_SPAWN, new EnemyStateSpawn(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_PATROL, new EnemyStatePatrol(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_CHASE_WAIT, new EnemyStateChaseWait(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_CHASE_WALK, new EnemyStateChaseWalk(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_CHASE_RUN, new EnemyStateChaseRun(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_SKILL, new EnemyStateSkill(this));

        state.StateDictionary.Add(ACTION_STATE.ENEMY_HIT_LIGHT, new EnemyStateLightHit(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_HIT_HEAVY, new EnemyStateHeavyHit(this));

        state.StateDictionary.Add(ACTION_STATE.ENEMY_STUN, new EnemyStateStun(this));
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
        state?.SetState(ACTION_STATE.COMMON_DIE, STATE_SWITCH_BY.WEIGHT);

        StartCoroutine(WaitForDisapear(Constants.TIME_NORMAL_MONSTER_DISAPEAR));
    }
    #endregion
}
