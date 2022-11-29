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
}

