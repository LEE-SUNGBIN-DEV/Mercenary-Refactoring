using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreePackage;

namespace Legacy
{
    /*
    // ======================================
    //              Legacy Script
    // ======================================
    public class BlackDragonBehaviourTree : BehaviourTree
    {
        private BaseEnemy enemy;

        public BlackDragonBehaviourTree(BaseEnemy enemy)
        {
            this.enemy = enemy;
        }

        public override BehaviourNode SetupTree()
        {
            BehaviourNode root = new Selector(new List<BehaviourNode>()
        {
            // Combat State
            new Sequence(new List<BehaviourNode>()
            {
                new ConditionHasTarget(enemy),
                new Selector(new List<BehaviourNode>()
                {
                    // Skill
                    new Sequence(new List<BehaviourNode>()
                    {
                        new ConditionUpperWeight(enemy, ENEMY_STATE.Skill),
                        new ConditionSkill(enemy),
                        new TaskSkill(enemy)
                    }),

                    // Chase
                    new Sequence(new List<BehaviourNode>()
                    {
                        new ConditionUpperWeight(enemy, ENEMY_STATE.Move),
                        new ConditionUpperRange(enemy, enemy.EnemyData.MinChaseRange),
                        new TaskEnemyChase(enemy)
                    }),
                })
            })
        });

            return root;
        }
    }
    */
}