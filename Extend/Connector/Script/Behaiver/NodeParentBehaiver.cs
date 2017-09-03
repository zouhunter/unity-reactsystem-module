using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace Connector
{
    public class NodeParentBehaiver : MonoBehaviour, INodeParent
    {
        private List<INodeItem> _childNodes = new List<INodeItem>();
        public List<INodeItem> ChildNodes
        {
            get
            {
                return _childNodes;
            }
        }
        public string Name {
            get
            {
                return name;
            }
        }

        public Transform Trans
        {
            get
            {
                return transform;
            }
        }

        public void ResetBodyTransform(INodeParent otherParent, Vector3 rPos, Vector3 rdDir)
        {
            transform.position = otherParent.Trans.TransformPoint(rPos);
            transform.forward = otherParent.Trans.TransformDirection(rdDir);
        }

        private void Awake()
        {
            var nodeItems = GetComponentsInChildren<INodeItem>(true);
            _childNodes.AddRange(nodeItems);
            gameObject.layer = LayerConst.elementLayer;

            foreach (var item in nodeItems)
            {
                item.Body = this;
            }
        }
    }

}