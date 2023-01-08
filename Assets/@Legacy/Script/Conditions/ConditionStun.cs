using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreePackage;

namespace Legacy
{    // ======================================
     //              Legacy Script
     // ======================================

    /*
    public class ConditionStun : BehaviourNode
    {
        private BaseEnemy enemy;

        public ConditionStun(BaseEnemy enemy)
        {
            this.enemy = enemy;
        }

        public override NODE_STATE Evaluate()
        {
            if (enemy.AbnormalStateController.CheckState(ABNORMAL_TYPE.Stun))
            {
                state = NODE_STATE.Success;
            }
            else
            {
                enemy.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_STUN, false);
                state = NODE_STATE.Failture;
            }
            return state;
        }
    }
    */
}