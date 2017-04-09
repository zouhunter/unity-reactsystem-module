using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Tuples;
/// <summary>
/// 处理输入和输出事件触发
/// </summary>
namespace FlowSystem
{
    public interface IInOutItem
    {
        bool AutoReact();//初始状态触发自动反应（对象）
        void FunctionIn(int nodeID, string type,UnityAction onComplete);//反应物进入
        void RecordReactInfo(IFlowSystemCtrl ctrl);//发生反应
    }
}