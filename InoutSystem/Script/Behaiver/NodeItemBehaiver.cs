using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace FlowSystem
{
    /// <summary>
    /// 节点的关联坐标的匹配
    /// </summary>
    public class NodeItemBehaiver : MonoBehaviour, INodeItem
    {
        public INodeParent Body { get; set; }
        public NodeInfo Info
        {
            get
            {
                return nodeInfo;
            }
        }
        public INodeItem ConnectedNode { get; set; }
        public Renderer Render
        {
            get
            {
                return render;
            }
        }
        public Vector3 Pos
        {
            get
            {
                return transform.position;
            }
        }

        [SerializeField]
        private NodeInfo nodeInfo;
        [SerializeField, Array]
        public List<ConnectAble> connectAble;
        [SerializeField]
        private Renderer render;

        void Awake()
        {
            if (render == null)
            {
                render = GetComponentInChildren<Renderer>();
            }
        }

        public bool Attach(INodeItem item)
        {
            //如果不存在对应的安装点，无法安装
            if (connectAble.Find((x) => x.itemName == item.Body.Name && x.nodeId == item.Info.nodeID) == null)
                return false;
            if (item.Info.nodeName == Info.nodeName && item.Info.isIn != Info.isIn && item.ConnectedNode == null)
            {
                item.ConnectedNode = this;
                ConnectedNode = item;
                ResetTargetTrans();
                return true;
            }
            return false;
        }

        private void ResetTargetTrans()
        {
            if (ConnectedNode != null)
            {
                ConnectAble connect = connectAble.Find(x => { return x.itemName == ConnectedNode.Body.Name && x.nodeId == ConnectedNode.Info.nodeID; });
                if (connect != null)
                {
                    ConnectedNode.Body.ResetBodyTransform(Body, connect.relativePos, connect.relativeRot);
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