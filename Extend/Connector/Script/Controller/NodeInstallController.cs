using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace Connector
{
    public class NodeConnectController : INodeConnectController
    {
        public event UnityAction<INodeItem[]> onConnected;
        public event UnityAction<INodeItem> onMatch;
        public event UnityAction<INodeItem> onDisMatch;
        public event UnityAction<INodeItem[]> onDisconnected;
        public Dictionary<INodeParent, List<INodeItem>> ConnectedDic { get { return connectedNodes; } }

        private float timeSpan;
        private float spanTime;
        private float sphereRange = 0.0001f;
        private INodeParent pickedUpItem;
        private INodeItem activeNode;
        private INodeItem targetNode;
        private Dictionary<INodeParent, List<INodeItem>> connectedNodes = new Dictionary<INodeParent, List<INodeItem>>();
        public NodeConnectController(float sphereRange, float spanTime)
        {
            this.spanTime = spanTime;
            this.sphereRange = sphereRange;
        }

        public void Update()
        {
            timeSpan += Time.deltaTime;
            if (pickedUpItem != null && timeSpan > spanTime)
            {
                timeSpan = 0f;
                if (targetNode != null)
                {
                    onDisMatch.Invoke(targetNode);
                }
                if (activeNode != null)
                {
                    onDisMatch.Invoke(activeNode);
                }
                if (FindConnectableObject())
                {
                    onDisMatch.Invoke(activeNode);
                    onDisMatch.Invoke(targetNode);
                }
            }
        }

        public bool FindConnectableObject()
        {
            if (pickedUpItem != null)
            {
                INodeItem tempNode;
                foreach (var item in pickedUpItem.ChildNodes)
                {
                    if (FindInstallableNode(item, out tempNode))
                    {
                        activeNode = item;
                        targetNode = tempNode;
                        return true;
                    }
                }
            }
            activeNode = null;
            targetNode = null;
            return false;
        }

        private bool FindInstallableNode(INodeItem item, out INodeItem node)
        {
            Collider[] colliders = Physics.OverlapSphere(item.Pos, sphereRange, 1 << LayerConst.nodeLayer);
            if (colliders != null && colliders.Length > 0)
            {
                foreach (var collider in colliders)
                {
                    INodeItem tempNode = collider.GetComponent<INodeItem>();
                    //主被动动连接点，非自身点，相同名，没有建立连接
                    if (tempNode.Body != item.Body && tempNode.ConnectedNode == null)
                    {
                        if (tempNode.connectAble.Find((x) => x.itemName == item.Body.Name && x.nodeId == item.NodeID) != null)
                        {
                            node = tempNode;
                            return true;
                        }
                    }
                }
            }
            node = null;
            return false;
        }

        public void SetActiveItem(INodeParent item)
        {
            this.pickedUpItem = item;
            List<INodeItem> olditems = new List<INodeItem>();
            if (connectedNodes.ContainsKey(item))
            {
                List<INodeItem> needClear = new List<INodeItem>();
                for (int i = 0; i < connectedNodes[item].Count; i++)
                {
                    INodeItem nodeItem = connectedNodes[item][i];
                    needClear.Add(nodeItem);

                    INodeItem target = nodeItem.ConnectedNode;
                    connectedNodes[target.Body].Remove(target);
                    Debug.Log(connectedNodes[item][i].Detach());

                    olditems.Add(nodeItem);
                    olditems.Add(target);
                }

                for (int i = 0; i < needClear.Count; i++)
                {
                    connectedNodes[item].Remove(needClear[i]);
                }
                if (onDisconnected != null) onDisconnected.Invoke(needClear.ToArray());
            }
        }

        public void SetDisableItem(INodeParent item)
        {
            pickedUpItem = null;
            targetNode = null;
            activeNode = null;
        }

        public void TryConnect()
        {
            if (activeNode != null && activeNode != null)
            {
                if (targetNode.Attach(activeNode))
                {
                    activeNode.ResetTransform();

                    if (!connectedNodes.ContainsKey(pickedUpItem))
                    {
                        connectedNodes[pickedUpItem] = new List<INodeItem>();
                    }

                    connectedNodes[pickedUpItem].Add(activeNode);

                    if (!connectedNodes.ContainsKey(targetNode.Body))
                    {
                        connectedNodes[targetNode.Body] = new List<INodeItem>();
                    }

                    connectedNodes[targetNode.Body].Add(targetNode);

                    if (onConnected != null) onConnected.Invoke(new INodeItem[] { activeNode, targetNode });
                }
            }
        }
    }
}