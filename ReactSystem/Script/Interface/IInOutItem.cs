using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 处理输入和输出事件触发
/// </summary>
namespace ReactSystem
{
    public interface IContainer
    {
        GameObject Go { get; }
        event UnityAction<IContainer> onComplete;
        event Func<IContainer,int, string[],bool> onExport;//发生反应事件
        void Active(bool force = false);
        void Import(int nodeID, string[] type);//反应物进入,返回是否会反应
    }
}