using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Tuples;
namespace FlowSystem
{
    /// <summary>
    /// 流动反应，并触发事件
    /// </summary>
    public class FlowSystemCtrl : IFlowSystemCtrl
    {
        private ElementGroup groupPrefab;
        private ElementGroup group;
        private string experimentName;
        private List<RunTimeElemet> elements;
        readonly List<GameObject> loadedItems = new List<GameObject>();

        public IInOutItem ActiveItem
        {
            get
            {
                return activeItem;
            }
        }

        public string ExperimentName
        {
            get
            {
                return experimentName;
            }
        }

        private IInOutItem activeItem;
        private Queue<Tuple<IInOutItem, bool, int, string>> reactTuple = new Queue<Tuple<IInOutItem, bool, int, string>>();

        public FlowSystemCtrl(ElementGroup group)
        {
            this.groupPrefab = group;
        }

        public void InitExperiment(ExperimentDataObject objectData)
        {
            experimentName = objectData.expName;
            elements = objectData.elements;
        }

        public void ReStart()
        {
            ResatToBeginState();

            GameObject item;
            IInOutItem inoutItem;
            for (int i = 0; i < elements.Count; i++)
            {
                RunTimeElemet element = elements[i];
                item = GameObject.Instantiate(element.element, element.position, element.rotation) as GameObject;
                item.transform.SetParent(group.transform);
                inoutItem = item.GetComponent<IInOutItem>();
                loadedItems.Add(item);
                if (element.autoReact)
                {
                    reactTuple.Enqueue(new Tuple<IInOutItem, bool, int, string>(inoutItem, true));
                }
            }
        }

        private void ResatToBeginState()
        {
            activeItem = null;
            reactTuple.Clear();
            loadedItems.Clear();
            if (group != null)
            {
                GameObject.Destroy(group.gameObject);
            }
            group = GameObject.Instantiate<ElementGroup>(groupPrefab);
        }


        public void StartProducer()
        {
            ReactFunction();
        }

        public bool NextContainer()
        {
            if (ActiveItem == null) return false;
            List<Tuple<IInOutItem, bool, int, string>> newItems;
            if (ActiveItem.ReactComplete(out newItems))
            {
                for (int i = 0; i < newItems.Count; i++)
                {
                    reactTuple.Enqueue(newItems[i]);
                }
            }
            return ReactFunction();
        }

        public void AddActiveItem(IInOutItem item, int nodeID, string type)
        {
            reactTuple.Enqueue(new Tuple<IInOutItem, bool, int, string>(item, false, nodeID, type));
        }

        private bool ReactFunction()
        {
            if (reactTuple.Count > 0)
            {
                Tuple<IInOutItem, bool, int, string> item = reactTuple.Dequeue();
                activeItem = item.Element1;
                if (item.Element2)
                {
                    item.Element1.AutoReact();
                }
                else
                {
                    item.Element1.FunctionIn(item.Element3, item.Element4);
                }
                return true;
            }
            return false;
        }
    }
}