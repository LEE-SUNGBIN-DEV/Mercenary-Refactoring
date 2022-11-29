using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreePackage;

public class CheckHP : BehaviourNode
{
    private Enemy enemy;
    private bool isDie;

    public CheckHP(Enemy enemy)
    {
        this.enemy = enemy;
        isDie = false;
    }

    public override NODE_STATE Evaluate()
    {
        state = NODE_STATE.Failture;
        if (!isDie && enemy.EnemyData.CurrentHP <= 0)
        {
            isDie = true;
            state = NODE_STATE.Success;
        }

        return state;
    }
}