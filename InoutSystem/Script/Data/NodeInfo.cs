using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace FlowSystem
{
    [System.Serializable]
    public class NodeInfo
    {
        public string nodeName;
        public bool isIn;
        public int nodeID;
        public bool unilateral;//单向（防止回流）
    }
}
