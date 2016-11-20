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
        public Button pauseTog;

        FlowSystemCtrl _systemCtrl;
        public ElementGroup groupParent;
        void Start()
        {
            _systemCtrl = new FlowSystemCtrl(groupParent);
            _systemCtrl.InitExperiment(experimentData);

            startBtn.onClick.AddListener(RestartExperiment);
            interactBtn.onClick.AddListener(StartExperiment);
            pauseTog.onClick.AddListener(OnPauseExperiemnt);
        }
        void RestartExperiment()
        {
            _systemCtrl.ReStart();
        }
        void StartExperiment()
        {
            _systemCtrl.StartProducer();
        }
        void OnPauseExperiemnt()
        {
            _systemCtrl.NextContainer();
        }
    }
}