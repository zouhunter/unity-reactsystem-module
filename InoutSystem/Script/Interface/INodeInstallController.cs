using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
namespace FlowSystem
{
    internal interface INodeConnectController
    {
        INodeParent SelectedItem { get; }
        NodeItemBehaiver ActiveNode { get; }
        NodeItemBehaiver TargetNode { get; }

        List<INodeItem> PickUpInOutItem(INodeParent item);
        void PutDownInOutItem(bool connected);
        bool TryConnectItem();
        bool WaitForPickUp();
        bool FindConnectableObject();
    }
}