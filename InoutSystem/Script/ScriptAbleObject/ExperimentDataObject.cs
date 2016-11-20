using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace FlowSystem
{
    [CreateAssetMenu(fileName = "ExperimentDataObject.asset", menuName = "InOutSystem/expConfig")]
    public class ExperimentDataObject : ScriptableObject
    {
        public string expName;
        public List<RunTimeElemet> elements = new List<RunTimeElemet>();
        public NodeConnect[] defultConnect;
    }
}