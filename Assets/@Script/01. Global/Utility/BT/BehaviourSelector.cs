using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreePackage
{
    // - 자식 노드 가운데 하나를 실행하기 위한 노드
    // - Selector의 자식 노드 중 하나라도 Success나 Running을 반환하면, Selector는 바로 Success나 Running을 부모 노드에 반환
    // - Selector의 모든 자식 노드가 Failure를 반환했을 때는 Selector도 부모 노드에 Failure를 반환한다.
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
