using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace FlowSystem
{
    /// <summary>
    /// 负责元素拾起，连接，高亮等功能的组合
    /// </summary>
    public class ElementGroup : MonoBehaviour
    {
        [Range(0.1f, 1f)]
        public float nodeUpdateSpanTime = 0.5f;
        [Range(0.02f, 0.1f)]
        public float pickUpSpantime = 0.02f;
        [Range(10, 60)]
        public int scrollSpeed = 20;
        [SerializeField]
        private List<Color> highLightColors;
        [Range(0, 1)]
        public float sphereRange = 0.1f;
        [Range(3, 15)]
        public float distence = 1f;

        private IPickUpController pickCtrl;
        private INodeConnectController nodeConnectCtrl;
        private ShaderHighLight highLightNode;
        private ShaderHighLight highLightElement;
        private IPickUpAble pickUped;
        void Start()
        {
            pickCtrl = new PickUpController(pickUpSpantime, distence, scrollSpeed);

            nodeConnectCtrl = new NodeConnectController(sphereRange, nodeUpdateSpanTime);
            highLightNode = new ShaderHighLight();
            highLightElement = new ShaderHighLight();
        }

        void Update()
        {
            if (UnityEngine.EventSystems.EventSystem.current != null && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
            UpdateInputEvent();
            HighLightIfFindConnectable();
        }

        private void UpdateInputEvent()
        {
            if (pickCtrl.PickUped)
            {
                pickCtrl.UpdatePickUpdObject();

                if (Input.GetMouseButtonDown(0))
                {
                    if (pickCtrl.TryStayPickUpedObject())
                    {
                        if (nodeConnectCtrl.TryConnectItem())
                        {
                            highLightNode.HighLightTarget(nodeConnectCtrl.TargetNode.Render, highLightColors[2]);
                            highLightNode.HighLightTarget(nodeConnectCtrl.ActiveNode.Render, highLightColors[2]);
                            nodeConnectCtrl.PutDownInOutItem(true);
                        }
                    }
                    highLightElement.UnHighLightTarget(pickUped.Trans.gameObject);
                }
                else if (Input.GetMouseButtonDown(2))
                {
                    if (pickCtrl.PickDownPickedUpObject())
                    {
                        nodeConnectCtrl.PutDownInOutItem(false);
                    }
                    highLightElement.UnHighLightTarget(pickUped.Trans.gameObject);
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (pickCtrl.TryPickUpObject(out pickUped))
                {
                    List<INodeItem> oldItems = nodeConnectCtrl.PickUpInOutItem(pickUped.Trans.GetComponent<InOutItemBehaiver>());
                    for (int i = 0; i < oldItems.Count; i++)
                    {
                        highLightNode.UnHighLightTarget(oldItems[i].Render);
                    }
                    highLightElement.HighLightTarget(pickUped.Trans.gameObject, Color.clear);
                }
            }

        }
        private void HighLightIfFindConnectable()
        {
            if (nodeConnectCtrl.WaitForPickUp())
            {
                if (nodeConnectCtrl.TargetNode != null)
                {
                    highLightNode.UnHighLightTarget(nodeConnectCtrl.TargetNode.Render);
                }
                if (nodeConnectCtrl.ActiveNode != null)
                {
                    highLightNode.UnHighLightTarget(nodeConnectCtrl.ActiveNode.Render);
                }
                if (nodeConnectCtrl.FindConnectableObject())
                {
                    highLightNode.HighLightTarget(nodeConnectCtrl.TargetNode.Render, highLightColors[1]);
                    highLightNode.HighLightTarget(nodeConnectCtrl.ActiveNode.Render, highLightColors[1]);
                }
            }
        }
    }
}