using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreePackage
{
    // - 각 자식 노드를 순서대로 실행하며
    // - 자식 노드 중 하나라도 실패한다면 failure를 반환
    public class Sequence : BehaviourNode
    {
        public Sequence() : base()
        { }
        public Sequence(List<BehaviourNode> children) : base(children)
        { }

        public override NODE_STATE Evaluate()
        {
            bool isAnyChildRunning = false;

            foreach (BehaviourNode node in children)
            {
                switch (node.Evaluate())
                {
                    case NODE_STATE.Failture:
                        state = NODE_STATE.Failture;
                        return state;
                    case NODE_STATE.Success:
                        continue;
                    case NODE_STATE.Running:
                        isAnyChildRunning = true;
                        continue;

                    default:
                        state = NODE_STATE.Success;
                        return state;
                }
            }
            state = isAnyChildRunning ? NODE_STATE.Running : NODE_STATE.Success;
            return state;
        }
    }

}
