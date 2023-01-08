using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateStagger : IEnemyState
{
    private int stateWeight;
    private float staggerTime;
    private float cumulativeTime;

    public EnemyStateStagger()
    {
        stateWeight = (int)ENEMY_STATE_WEIGHT.Stagger;
        staggerTime = 5f;
        cumulativeTime = 0f;
    }

    public void Enter(BaseEnemy enemy)
    {
        enemy.Animator.SetTrigger(Constants.ANIMATOR_PARAMETERS_TRIGGER_STAGGER);
        enemy.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_STAGGER, true);
    }

    public void Update(BaseEnemy enemy)
    {
        cumulativeTime += Time.deltaTime;
        if(cumulativeTime >= staggerTime)
        {
            enemy.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_STAGGER, false);
        }
    }

    public void Exit(BaseEnemy enemy)
    {
        enemy.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_STAGGER, false);
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
