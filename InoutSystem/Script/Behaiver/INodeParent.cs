using UnityEngine;

using System.Collections.Generic;
namespace FlowSystem
{
    public interface INodeParent
    {
        string Name { get; }
        Transform Trans { get; }
        List<NodeItemBehaiver> ChildNodes { get; }
        void ResetBodyTransform(INodeParent otherParent, Vector3 rPos, Quaternion rRot);
    }
}