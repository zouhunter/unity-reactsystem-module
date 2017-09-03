using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
namespace Connector
{
    internal interface INodeConnectController
    {
        event UnityAction<INodeItem> onMatch;
        event UnityAction<INodeItem> onDisMatch;
        event UnityAction<INodeItem[]> onConnected;
        event UnityAction<INodeItem[]> onDisconnected;
        Dictionary<INodeParent,List<INodeItem>> ConnectedDic { get; }
        void SetActiveItem(INodeParent item);
        void SetDisableItem(INodeParent item);
        void TryConnect();
        void Update();
    }
}