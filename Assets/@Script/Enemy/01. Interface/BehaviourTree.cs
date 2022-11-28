using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreePackage
{
    public abstract class BehaviourTree : MonoBehaviour
    {
        private BehaviourNode root = null;

        private void Awake()
        {
            root = SetupTree();
        }

        private void Update()
        {
            if(root != null)
            {
                root.Evaluate();
            }
        }

        public abstract BehaviourNode SetupTree();
    }

    public class Sequence : BehaviourNode
    {
        public Sequence() : base() {}
        public Sequence(List<BehaviourNode> children) : base(children) { }


        public override NODE_STATE Evaluate()
        {
            bool isAnyChildRunning = false;

            foreach(var node in children)
            {
                switch(node.Evaluate())
                {
                    case NODE_STATE.FAILTURE:
                        state = NODE_STATE.FAILTURE;
                        return state;
                    case NODE_STATE.SUCCESS:
                        continue;
                    case NODE_STATE.RUNNING:
                        isAnyChildRunning = true;
                        continue;

                    default:
                        state = NODE_STATE.SUCCESS;
                        return state;
                }
            }
            state = isAnyChildRunning ? NODE_STATE.RUNNING: NODE_STATE.SUCCESS;
            return state;
        }
    }
}

