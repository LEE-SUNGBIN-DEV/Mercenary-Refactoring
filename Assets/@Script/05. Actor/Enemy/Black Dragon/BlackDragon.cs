using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlackDragon : BaseEnemy, IStaggerable, ICompetable
{
    protected override void Awake()
    {
        base.Awake();

        state.StateDictionary.Add(ACTION_STATE.ENEMY_STAGGER, new EnemyStateStagger(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_COMPETE, new EnemyStateCompete(this));
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Spawn(Vector3 spawnPosition)
    {
        base.Spawn(spawnPosition);
        state.SetState(ACTION_STATE.ENEMY_SPAWN, STATE_SWITCH_BY.WEIGHT);
    }

    public override void OnLightHit()
    {
    }

    public override void OnHeavyHit()
    {
    }

    public void OnStagger() { state.SetState(ACTION_STATE.ENEMY_STAGGER, STATE_SWITCH_BY.WEIGHT); }
    public void OnCompete() { state.SetState(ACTION_STATE.ENEMY_COMPETE, STATE_SWITCH_BY.WEIGHT); }
}
