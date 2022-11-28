using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreePackage;

public class EnemyBehaviourTree : BehaviourTree
{
    private Enemy enemy;

    public EnemyBehaviourTree(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override BehaviourNode SetupTree()
    {
        BehaviourNode root = new TaskEnemyIdle(enemy);

        return root;
    }
}
