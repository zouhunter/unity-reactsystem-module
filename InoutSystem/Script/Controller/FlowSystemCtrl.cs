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
        public IInOutItem ActiveItem
        {
            get
            {
                return activeItem;
            }
        }
        public Func<IInOutItem,int, Dictionary<IInOutItem, int>> GetConnectedDic;

        private List<RunTimeElemet> elements;
        readonly List<IInOutItem> loadedItems = new List<IInOutItem>();
        private IInOutItem activeItem;
        private Queue<Tuple<IInOutItem, int, string[]>> reactTuple = new Queue<Tuple<IInOutItem, int, string[]>>();
        private bool isReact;

        public event UnityAction onComplete;
        public event UnityAction<IInOutItem> onStepBreak;

        public void InitExperiment(List<RunTimeElemet> elements)
        {
            this.elements = elements;
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
                item.name = element.name;
                inoutItem = item.GetComponent<IInOutItem>();
                inoutItem.onExport += OnReact;
                inoutItem.onComplete += OnOneStep;
                loadedItems.Add(inoutItem);
            }
        }

        public bool TryStartProducer()
        {
            if (elements == null) return false;
            if (GetConnectedDic == null) return false;
            foreach (var item in loadedItems)
            {
                item.Active();
            }
            return true;
        }

        public void TryNextContainer()
        {
            if (isReact) return;

            while (reactTuple.Count > 0)
            {
                Tuple<IInOutItem, int, string[]> item = reactTuple.Dequeue();
                activeItem = item.Element1;
                var outInfo = GetConnectedDic(activeItem,item.Element2);

                if (outInfo != null || outInfo.Count != 0)
                {
                    foreach (var node in outInfo)
                    {
                        var outInoutItem = node.Key;
                        var outInoutId= node.Value;
                        outInoutItem.Import(outInoutId, item.Element3);
                    }
                }
                else
                {
                    if (onStepBreak != null) onStepBreak.Invoke(activeItem);
                }

                isReact = true;
            }
        }

        private void ResatToBeginState()
        {
            activeItem = null;
            foreach (var item in loadedItems){
                GameObject.Destroy(item.Go);
            }
            loadedItems.Clear();
        }

        private bool OnReact(IInOutItem item, int id, string[] type)
        {
            reactTuple.Enqueue(new Tuple<IInOutItem, int, string[]>(item, id, type));
            var outInfo = GetConnectedDic(item, id);
            return outInfo != null;
        }
        private void OnOneStep(IInOutItem item)
        {
            isReact = false;
            if (reactTuple.Count == 0)
            {
                if (onComplete != null) onComplete.Invoke();
            }
        }

    }
}