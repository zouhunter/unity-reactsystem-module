using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace Connector
{
    [ExecuteInEditMode]
    public class NodeItemBehaiver : MonoBehaviour, INodeItem
    {
        #region Propertys
        public INodeParent Body { get; set; }
        public INodeItem ConnectedNode { get; set; }
        public GameObject Render {
            get
            {
                return gameObject;
            }
        }
        public Vector3 Pos
        {
            get
            {
                return transform.position;
            }
        }
        public int NodeID { get; private set; }
        public List<ConnectAble> connectAble
        {
            get
            {
                return _connectAble;
            }
        }
        #endregion

        public List<ConnectAble> _connectAble;

        void Awake()
        {
            NodeID = transform.GetSiblingIndex();
            gameObject.layer = LayerConst.nodeLayer;
        }

        public bool Attach(INodeItem item)
        {
            item.ConnectedNode = this;
            ConnectedNode = item;
            return true;
        }

        public void ResetTransform()
        {
            if (ConnectedNode != null)
            {
                ConnectAble connect = connectAble.Find(x => { return x.itemName == ConnectedNode.Body.Name && x.nodeId == ConnectedNode.NodeID; });
                if (connect != null)
                {
                    Body.ResetBodyTransform(ConnectedNode.Body, connect.relativePos, connect.relativeDir);
                }
            }
        }


        public INodeItem Detach()
        {
            INodeItem outItem = ConnectedNode;
            if (ConnectedNode != null)
            {
                ConnectedNode.ConnectedNode = null;
                ConnectedNode = null;
            }
            return outItem;
        }
    }

}