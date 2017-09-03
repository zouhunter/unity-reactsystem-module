using UnityEngine;

using System.Collections.Generic;
namespace Connector
{
    public interface INodeParent
    {
        string Name { get; }
        Transform Trans { get; }
        List<INodeItem> ChildNodes { get; }
        void ResetBodyTransform(INodeParent otherParent, Vector3 rPos, Vector3 rdDir);
    }
}