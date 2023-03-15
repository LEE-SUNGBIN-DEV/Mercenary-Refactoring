using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateStagger : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private float staggerTime;
    private float cumulativeTime;

    public EnemyStateStagger(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_STAGGER;
        staggerTime = 5f;
        cumulativeTime = 0f;
    }

    public void Enter()
    {
        cumulativeTime = 0f;
        enemy.Animator.SetTrigger(Constants.ANIMATOR_PARAMETERS_TRIGGER_STAGGER);
        enemy.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_STAGGER, true);
    }

    public void Update()
    {
        cumulativeTime += Time.deltaTime;
        if(cumulativeTime >= staggerTime)
        {
            enemy.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_STAGGER, false);
        }
    }

    public void Exit()
    {
        enemy.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_STAGGER, false);
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
