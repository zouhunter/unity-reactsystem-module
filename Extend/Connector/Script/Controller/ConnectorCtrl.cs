using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace Connector
{
    [System.Serializable]
    public class ConnectorCtrl
    {
        [Range(0.1f, 1f)]
        public float nodeUpdateSpanTime = 0.5f;
        [Range(0.02f, 0.1f)]
        public float pickUpSpantime = 0.02f;
        [Range(10, 60)]
        public int scrollSpeed = 20;
        [Range(0, 1)]
        public float sphereRange = 0.1f;
        [Range(3, 15)]
        public float distence = 1f;

        public Dictionary<INodeParent, List<INodeItem>> ConnectedDic { get { return nodeConnectCtrl.ConnectedDic; } }

        public GameObjectEvent onConnect;
        public GameObjectEvent onDisConnect;
        public GameObjectEvent onPickDown;
        public GameObjectEvent onPickStatu;
        public GameObjectEvent onPickUp;
        public GameObjectEvent onMatch;
        public GameObjectEvent onDisMatch;

        private IPickUpController pickCtrl;
        private INodeConnectController nodeConnectCtrl;

        public void Start()
        {
            pickCtrl = new PickUpController(pickUpSpantime, distence, scrollSpeed);
            nodeConnectCtrl = new NodeConnectController(sphereRange, nodeUpdateSpanTime);
            pickCtrl.onPickUp += OnPickUp;
            pickCtrl.onPickDown += OnPickDown;
            pickCtrl.onPickStatu += OnPickStatu;
            nodeConnectCtrl.onDisMatch += OnDisMath;
            nodeConnectCtrl.onMatch += OnMatch;
            nodeConnectCtrl.onConnected += OnConnected;
            nodeConnectCtrl.onDisconnected += OnDisConnected;
        }

        public void Update()
        {
            if (UnityEngine.EventSystems.EventSystem.current != null && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
            if (pickCtrl != null) pickCtrl.Update();
            if (nodeConnectCtrl != null) nodeConnectCtrl.Update();
        }

        void OnMatch(INodeItem item)
        {
            onMatch.Invoke(item.Render);
        }
        void OnDisMath(INodeItem item)
        {
            onDisMatch.Invoke(item.Render);
        }
        void OnPickUp(GameObject obj)
        {
            nodeConnectCtrl.SetActiveItem(obj.GetComponent<INodeParent>());
            onPickUp.Invoke(obj);
        }

        void OnPickDown(GameObject obj)
        {
            nodeConnectCtrl.SetDisableItem(obj.GetComponent<INodeParent>());
            onPickDown.Invoke(obj);
        }
        void OnConnected(INodeItem[] nodes)
        {
            foreach (var item in nodes)
            {
                onConnect.Invoke(item.Render);
            }
        }
        void OnDisConnected(INodeItem[] nodes)
        {
            foreach (var item in nodes)
            {
                onDisConnect.Invoke(item.Render);
            }
        }
        void OnPickStatu(GameObject go)
        {
            nodeConnectCtrl.TryConnect();
            onPickStatu.Invoke(go);
        }
    }
}