using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateIdle : IEnemyState
{
    private int stateWeight;
    private int animationNameHash;
    private float walkChaseRange;
    private float runChaseRange;

    public EnemyStateIdle()
    {
        stateWeight = (int)ENEMY_STATE_WEIGHT.Idle;
        animationNameHash = Constants.ANIMATION_NAME_IDLE;
    }

    public void Enter(BaseEnemy enemy)
    {
        enemy.NavMeshAgent.isStopped = true;
        enemy.NavMeshAgent.velocity = Vector3.zero;
        enemy.Animator.CrossFade(animationNameHash, 0.2f);
        walkChaseRange = enemy.EnemyData.ChaseRange;
        runChaseRange = walkChaseRange * 0.5f;
    }

    public void Update(BaseEnemy enemy)
    {
        if (enemy.State.IsUpperStateThanCurrentState(ENEMY_STATE.Skill))
        {
            foreach (var skill in enemy.SkillDictionary.Values)
            {
                if (skill.CheckCondition(enemy.TargetDistance))
                {
                    enemy.SelectSkill = skill;
                    enemy.State.TryStateSwitchingByWeight(ENEMY_STATE.Skill);
                    return;
                }
            }
        }

        if (enemy.State.StateDictionary.ContainsKey(ENEMY_STATE.Run)
            && enemy.TargetDistance >= runChaseRange)
        {
            enemy.State.TryStateSwitchingByWeight(ENEMY_STATE.Run);
            return;
        }

        if (enemy.TargetDistance > enemy.EnemyData.ChaseRange)
        {
            enemy.State.TryStateSwitchingByWeight(ENEMY_STATE.Walk);
            return;
        }

    }

    public void Exit(BaseEnemy enemy)
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
