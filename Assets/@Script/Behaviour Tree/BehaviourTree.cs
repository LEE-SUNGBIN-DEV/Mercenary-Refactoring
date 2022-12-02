using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreePackage
{
    public abstract class BehaviourTree
    {
        private BehaviourNode root = null;

        public void Initialize()
        {
            root = SetupTree();
        }

        public void Update()
        {
            if (root != null)
            {
                root.Evaluate();
            }
        }

        public abstract BehaviourNode SetupTree();
    }
}

