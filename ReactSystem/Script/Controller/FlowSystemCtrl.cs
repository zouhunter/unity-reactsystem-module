using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Tuples;
namespace ReactSystem
{
    /// <summary>
    /// 流动反应，并触发事件
    /// </summary>
    public class ReactSystemCtrl : IFlowSystemCtrl
    {
        public IContainer ActiveItem
        {
            get
            {
                return activeItem;
            }
        }
        public Func<IContainer,int, Dictionary<IContainer, int>> GetConnectedDic;

        private List<RunTimeElemet> elements;
        readonly List<IContainer> loadedItems = new List<IContainer>();
        private IContainer activeItem;
        private Queue<Tuple<IContainer, int, string[]>> reactTuple = new Queue<Tuple<IContainer, int, string[]>>();
        private bool isReact;

        public event UnityAction onComplete;
        public event UnityAction<IContainer> onStepBreak;

        public void InitExperiment(List<RunTimeElemet> elements)
        {
            this.elements = elements;
        }

        public void ReStart()
        {
            ResatToBeginState();

            GameObject item;
            IContainer inoutItem;
            for (int i = 0; i < elements.Count; i++)
            {
                RunTimeElemet element = elements[i];
                item = GameObject.Instantiate(element.element, element.position, element.rotation) as GameObject;
                item.name = element.name;
                inoutItem = item.GetComponent<IContainer>();
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
                Tuple<IContainer, int, string[]> item = reactTuple.Dequeue();
                activeItem = item.Element1;
                var outInfo = GetConnectedDic(activeItem,item.Element2);

                if (outInfo != null && outInfo.Count != 0)
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
            reactTuple.Clear();
            activeItem = null;
            foreach (var item in loadedItems){
                GameObject.Destroy(item.Go);
            }
            loadedItems.Clear();
        }

        private bool OnReact(IContainer item, int id, string[] type)
        {
            reactTuple.Enqueue(new Tuple<IContainer, int, string[]>(item, id, type));
            var outInfo = GetConnectedDic(item, id);
            return outInfo != null && outInfo.Count != 0;
        }
        private void OnOneStep(IContainer item)
        {
            isReact = false;
            if (reactTuple.Count == 0)
            {
                if (onComplete != null) onComplete.Invoke();
            }
        }

    }
}