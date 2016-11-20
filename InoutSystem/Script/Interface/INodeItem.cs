using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace FlowSystem
{
    /// <summary>
    /// 处理节点的连接和断开
    /// </summary>
    public interface INodeItem
    {
        INodeParent Body { get; set; }
        Renderer Render { get; }
        NodeInfo Info { get; }
        INodeItem ConnectedNode { get; set; }
        bool Attach(INodeItem item);
        INodeItem Detach();
    }
}