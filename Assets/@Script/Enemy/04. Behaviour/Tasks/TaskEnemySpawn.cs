using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreePackage;

public class TaskEnemySpawn : BehaviourNode
{
    private Enemy enemy;

    public TaskEnemySpawn(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override NODE_STATE Evaluate()
    {
        enemy.tag = Constants.TAG_INVINCIBILITY;

        enemy.NavMeshAgent.isStopped = false;
        enemy.NavMeshAgent.SetDestination(enemy.TargetTransform.position);
        enemy.Animator.SetTrigger("doSpawn");

        return NODE_STATE.Running;
    }
}
