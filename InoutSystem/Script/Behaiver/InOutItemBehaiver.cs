using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Tuples;
namespace FlowSystem
{
    /// <summary>
    /// 进入和输出事件处理,节点关联功能
    /// </summary>
    public class InOutItemBehaiver : MonoBehaviour, IInOutItem, INodeParent
    {
        public ItemInfo itemInfo;
        public string defultAutoExport;
        public InputField.OnChangeEvent autoExportEvent;
        public List<Interact> interact;
        public List<NodeItemBehaiver> nodeItems = new List<NodeItemBehaiver>();

        public Transform Trans { get { return transform; } }

        public string Name
        {
            get
            {
                return itemInfo.name;
            }
        }

        public List<NodeItemBehaiver> ChildNodes
        {
            get
            {
                return nodeItems;
            }
        }

        void Start()
        {
            RegisterNodes();
        }

        private void RegisterNodes()
        {
            for (int i = 0; i < nodeItems.Count; i++)
            {
                nodeItems[i].Body = this;
            }
        }

        public void FunctionIn(int nodeId, string type)
        {
            Interact inter = interact.Find((x) => x.nodeID == nodeId && x.intype == type);
            if (inter != null)
            {
                defultAutoExport = inter.outtype;
                inter.interact.Invoke(defultAutoExport);
                //输出下一步
            }
        }

        public void ResetBodyTransform(INodeParent otherParent, Vector3 rPos, Quaternion rRot)
        {
            transform.position = otherParent.Trans.TransformPoint(rPos);
            transform.rotation = rRot * otherParent.Trans.rotation;
        }

        public void AutoReact()
        {
            autoExportEvent.Invoke(defultAutoExport);
        }

        public bool ReactComplete(out List<Tuple<IInOutItem, bool, int, string>> completeData)
        {
            completeData = new List<System.Tuples.Tuple<IInOutItem, bool, int, string>>();
            //将没有连接作为输出并且不是单向的node产生对应的事件，对链接的点产生输出
            for (int i = 0; i < ChildNodes.Count; i++)
            {
                if (ChildNodes[i].ConnectedNode != null && !ChildNodes[i].Info.isIn)
                {
                    IInOutItem inout = ChildNodes[i].ConnectedNode.Body.Trans.GetComponent<IInOutItem>();
                    completeData.Add(new System.Tuples.Tuple<IInOutItem, bool, int, string>(
                        inout, false, ChildNodes[i].ConnectedNode.Info.nodeID, defultAutoExport));

                }
            }
            if (completeData.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}