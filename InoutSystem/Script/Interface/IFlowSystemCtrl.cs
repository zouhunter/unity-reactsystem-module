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
        event UnityAction onComplete;
        event UnityAction<IInOutItem> onStepBreak;
        IInOutItem ActiveItem { get; }
        void ReStart();
        bool TryStartProducer();
        void TryNextContainer();
    }
}