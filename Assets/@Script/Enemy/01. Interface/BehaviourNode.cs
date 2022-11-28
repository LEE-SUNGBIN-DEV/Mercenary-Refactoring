using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreePackage
{
    public enum NODE_STATE
    {
        RUNNING,
        SUCCESS,
        FAILTURE,
    }

    public class BehaviourNode
    {
        private Dictionary<string, object> dataContext = new Dictionary<string, object>();
        protected NODE_STATE state;
        protected BehaviourNode parent;
        protected List<BehaviourNode> children = new List<BehaviourNode>();

        public BehaviourNode()
        {
            parent = null;
        }
        public BehaviourNode(List<BehaviourNode> children)
        {
            foreach (var child in children)
            {
                AddNode(child);
            }
        }

        public void AddNode(BehaviourNode node)
        {
            node.parent = this;
            children.Add(node);
        }

        public virtual NODE_STATE Evaluate()
        {
            return NODE_STATE.FAILTURE;
        }

        public void SetData(string key, object value)
        {
            dataContext[key] = value;
        }

        public object GetData(string key)
        {
            if (dataContext.TryGetValue(key, out object value))
            {
                return value;
            }

            BehaviourNode node = parent;
            while(node != null)
            {
                value = node.GetData(key);
                if(value != null)
                {
                    return value;
                }
                node = node.parent;
            }

            return null;
        }

        public bool CleanData(string key)
        {
            if(dataContext.ContainsKey(key))
            {
                dataContext.Remove(key);
                return true;
            }

            BehaviourNode node = parent;
            while (node != null)
            {
                bool isCleared = node.CleanData(key);
                if (isCleared)
                {
                    return true;
                }
                node = node.parent;
            }
            return false;
        }
    }
}



