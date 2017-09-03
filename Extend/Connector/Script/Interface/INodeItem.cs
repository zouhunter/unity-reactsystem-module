using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace Connector
{
    /// <summary>
    /// 处理节点的连接和断开
    /// </summary>
    public interface INodeItem
    {
        int NodeID { get; }
        List<ConnectAble> connectAble { get; }
        Vector3 Pos { get; }
        INodeParent Body { get; set; }
        GameObject Render { get; }
        INodeItem ConnectedNode { get; set; }
        void ResetTransform();
        bool Attach(INodeItem item);
        INodeItem Detach();
        
    }
}