using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreePackage;

public class TaskEnemyDie : BehaviourNode
{
    private Enemy enemy;
    private bool isCalled;

    public TaskEnemyDie(Enemy enemy)
    {
        this.enemy = enemy;
        isCalled = false;
    }

    public override NODE_STATE Evaluate()
    {
        if(!isCalled)
        {
            isCalled = true;
            enemy.Die();
        }
        state = NODE_STATE.Success;
        return state;
    }
}
