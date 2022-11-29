using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreePackage;

public class BlackDragonBehaviourTree : BehaviourTree
{
    private Enemy enemy;
    private Transform targetTransform;

    public BlackDragonBehaviourTree(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override BehaviourNode SetupTree()
    {
        BehaviourNode root = new TaskEnemyIdle(enemy);

        return root;
    }
}
