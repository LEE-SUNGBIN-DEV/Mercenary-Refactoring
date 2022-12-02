using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreePackage;

public class TaskEnemySpawn : BehaviourNode
{
    private Enemy enemy;
    private bool isCalled;

    public TaskEnemySpawn(Enemy enemy)
    {
        this.enemy = enemy;
        isCalled = false;
    }

    public override NODE_STATE Evaluate()
    {
        if (!isCalled)
        {
            isCalled = true;
            enemy.tag = Constants.TAG_INVINCIBILITY;
            enemy.Animator.SetTrigger("doSpawn");
        }
        state = NODE_STATE.Success;

        return state;
    }
}
