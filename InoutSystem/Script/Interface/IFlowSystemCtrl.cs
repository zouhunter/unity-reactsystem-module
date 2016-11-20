using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace FlowSystem
{
    public interface IFlowSystemCtrl
    {
        string ExperimentName { get; }
        IInOutItem ActiveItem { get; }
        void ReStart();
        void StartProducer();
        bool NextContainer();
        void AddActiveItem(IInOutItem item, int nodeID, string type);
    }
}