using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreePackage;

public class TaskEnemyDie : BehaviourNode
{
    private Enemy enemy;

    public TaskEnemyDie(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override NODE_STATE Evaluate()
    {
        enemy.Die();
        state = NODE_STATE.Success;
        return state;
    }
}
