using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace FlowSystem
{
    /// <summary>
    /// 负责实验的初始化，加载元素，重置元素
    /// </summary>
    public class FlowSystemBehaiver : MonoBehaviour
    {
        public ExperimentDataObject experimentData;

        public Button startBtn;
        public Button interactBtn;
        public Button nextBtn;

        FlowSystemCtrl _systemCtrl;
        public ElementGroup groupParent;
        void Start()
        {
            _systemCtrl = new FlowSystemCtrl(groupParent);
            _systemCtrl.InitExperiment(experimentData);

            startBtn.onClick.AddListener(RestartExperiment);
            interactBtn.onClick.AddListener(StartExperiment);
            nextBtn.onClick.AddListener(NextStep);
        }
        void RestartExperiment()
        {
            _systemCtrl.ReStart();
        }
        void StartExperiment()
        {
            if (_systemCtrl.StartProducer())
            {
                Debug.Log("实验启动成功");
            }
            else
            {
                Debug.Log("实验启动失败");
            }
        }
        void NextStep()
        {
            if (!_systemCtrl.NextContainer(NextStep))
            {
                Debug.Log("没有下一步实验");
            }
            else
            {
                Debug.Log("进行下一步");
            }
        }
    }
}