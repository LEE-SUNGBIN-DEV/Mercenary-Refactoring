using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreePackage
{
    // - �� �ڽ� ��带 ������� �����ϸ�
    // - �ڽ� ��� �� �ϳ��� �����Ѵٸ� failure�� ��ȯ
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
