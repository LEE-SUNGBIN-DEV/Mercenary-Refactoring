using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreePackage;

public class TaskEnemyChase : BehaviourNode
{
    private BaseEnemy enemy;

    public TaskEnemyChase(BaseEnemy enemy)
    {
        this.enemy = enemy;
    }

    public override NODE_STATE Evaluate()
    {
        Debug.Log("Task Chase");
        enemy.NavMeshAgent.isStopped = false;
        enemy.NavMeshAgent.SetDestination(enemy.TargetTransform.position);
        enemy.Animator.SetBool("isMove", true);

        return NODE_STATE.Running;
    }
}
