using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace ReactSystem
{
    public interface IFlowSystemCtrl
    {
        event UnityAction onComplete;
        event UnityAction<IContainer> onStepBreak;
        IContainer ActiveItem { get; }
        void ReStart();
        bool TryStartProducer();
        void TryNextContainer();
    }
}