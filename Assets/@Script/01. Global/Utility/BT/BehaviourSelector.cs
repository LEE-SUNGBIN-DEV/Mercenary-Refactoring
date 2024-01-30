using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreePackage
{
    // - �ڽ� ��� ��� �ϳ��� �����ϱ� ���� ���
    // - Selector�� �ڽ� ��� �� �ϳ��� Success�� Running�� ��ȯ�ϸ�, Selector�� �ٷ� Success�� Running�� �θ� ��忡 ��ȯ
    // - Selector�� ��� �ڽ� ��尡 Failure�� ��ȯ���� ���� Selector�� �θ� ��忡 Failure�� ��ȯ�Ѵ�.
    public class Selector : BehaviourNode
    {
        public Selector() : base() { }
        public Selector(List<BehaviourNode> children) : base(children) { }


        public override NODE_STATE Evaluate()
        {
            foreach (var node in children)
            {
                switch (node.Evaluate())
                {
                    case NODE_STATE.Failture:
                        continue;
                    case NODE_STATE.Success:
                        return state;
                    case NODE_STATE.Running:
                        state = NODE_STATE.Running;
                        return state;

                    default:
                        continue;
                }
            }
            state = NODE_STATE.Failture;
            return state;
        }
    }
}
