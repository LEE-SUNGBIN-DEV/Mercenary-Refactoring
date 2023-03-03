using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlackDragon : BaseEnemy, IStaggerable, ICompetable
{
    public override void Awake()
    {
        base.Awake();

        state = new EnemyFSM(this);
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Spawn()
    {
        base.Spawn();
        state.SetState(ACTION_STATE.ENEMY_SPAWN, STATE_SWITCH_BY.WEIGHT);
    }

    public override void OnDie()
    {
        base.OnDie();
        StartCoroutine(WaitForDisapear(10f));
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
