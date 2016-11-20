using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace FlowSystem
{
    [System.Serializable]
    public class Interact
    {
        public string intype;
        public int nodeID;
        public string outtype;
        public InputField.OnChangeEvent interact;
    }
}