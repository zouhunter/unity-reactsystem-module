//using System;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.Events;
//using System.Collections;
//using System.Collections.Generic;
//namespace FlowSystem
//{
//    /// <summary>
//    /// 节点的关联坐标的匹配
//    /// </summary>
//    [ExecuteInEditMode]
//    public class NodeItemBehaiver : MonoBehaviour, INodeItem
//    {
//        public INodeParent Body { get; set; }
//        public NodeInfo Info
//        {
//            get
//            {
//                return nodeInfo;
//            }
//        }
//        public INodeItem ConnectedNode { get; set; }
//        public GameObject Render
//        {
//            get
//            {
//                return gameObject;
//            }
//        }
//        public Vector3 Pos
//        {
//            get
//            {
//                return transform.position;
//            }
//        }

//        [SerializeField]
//        private NodeInfo nodeInfo;
//        [SerializeField, Array]
//        public List<ConnectAble> connectAble;

//        void Awake()
//        {
//            nodeInfo.nodeID = transform.GetSiblingIndex();
//        }

//        public bool Attach(INodeItem item)
//        {
//            item.ConnectedNode = this;
//            ConnectedNode = item;
//            return true;
//        }

//        public void ResetTransform()
//        {
//            if (ConnectedNode != null)
//            {
//                ConnectAble connect = connectAble.Find(x => { return x.itemName == ConnectedNode.Body.Name && x.nodeId == ConnectedNode.Info.nodeID; });
//                if (connect != null)
//                {
//                    Body.ResetBodyTransform(ConnectedNode.Body, connect.relativePos, connect.relativeDir);
//                }
//            }
//        }


//        public INodeItem Detach()
//        {
//            INodeItem outItem = ConnectedNode;
//            if (ConnectedNode != null)
//            {
//                ConnectedNode.ConnectedNode = null;
//                ConnectedNode = null;
//            }
//            return outItem;
//        }
//    }
//}