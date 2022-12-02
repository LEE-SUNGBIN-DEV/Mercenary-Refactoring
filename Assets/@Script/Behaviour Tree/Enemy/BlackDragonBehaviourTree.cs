using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreePackage;

public class BlackDragonBehaviourTree : BehaviourTree
{
    private Enemy enemy;

    public BlackDragonBehaviourTree(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override BehaviourNode SetupTree()
    {
        BehaviourNode root = new Selector(new List<BehaviourNode>()
        {
            // Die
            new Sequence(new List<BehaviourNode>()
            {
                new ConditionEnemyState(enemy, ENEMY_STATE.Die),
                new TaskEnemyDie(enemy)
            }),

            // Spawn
            new Sequence(new List<BehaviourNode>()
            {
                new ConditionEnemyState(enemy, ENEMY_STATE.Spawn),
                new TaskEnemySpawn(enemy)
            }),
               
            // Compete

            // Stun

            // Heavy Hit
            
            new Sequence(new List<BehaviourNode>()
            {
                new ConditionTarget(enemy),
                new TaskLookOn(enemy),

                new Selector(new List<BehaviourNode>()
                {
                    // Skill
                    new Sequence(new List<BehaviourNode>()
                    {
                        new ConditionEnemyState(enemy, ENEMY_STATE.Idle),
                        new ConditionSkill(enemy),
                        new TaskSkill(enemy)
                    }),

                    // Attack Wait
                    new Sequence(new List<BehaviourNode>()
                    {
                        new ConditionEnemyState(enemy, ENEMY_STATE.Attack),
                        new TaskEnemyWait(enemy)
                    }),

                    // Chase
                    new Sequence(new List<BehaviourNode>()
                    {
                        new ConditionEnemyState(enemy, ENEMY_STATE.Idle),
                        new ConditionChaseRange(enemy),
                        new TaskEnemyChase(enemy)
                    }),

                })

            }),

            // Idle
            new Sequence(new List<BehaviourNode>()
            {
                new ConditionEnemyState(enemy, ENEMY_STATE.Idle),
                new TaskEnemyIdle(enemy)
            })
        });

        return root;
    }
}
