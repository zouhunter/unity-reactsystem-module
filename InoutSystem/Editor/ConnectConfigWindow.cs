
using UnityEditor;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace FlowSystem
{
    public class ConnectConfigWindow : EditorWindow
    {
        private InOutItemBehaiver itemA;
        private NodeItemBehaiver node_A;
        private bool aIn;

        private InOutItemBehaiver itemB;
        private NodeItemBehaiver node_B;

        [MenuItem("Window/ConnectConfig")]
        static void OpenConnectConfigWindow()
        {
            EditorWindow window = GetWindow<ConnectConfigWindow>("连接配制", true);
            window.position = new Rect(400, 300, 500, 300);
            window.Show();
        }

        void OnGUI()
        {
            aIn = EditorGUILayout.Toggle("A作输入", aIn);
            itemA = EditorGUILayout.ObjectField("元素A", itemA, typeof(InOutItemBehaiver), true) as InOutItemBehaiver;
            node_A = EditorGUILayout.ObjectField("A子节点", node_A, typeof(NodeItemBehaiver), true) as NodeItemBehaiver;
            itemB = EditorGUILayout.ObjectField("元素B", itemB, typeof(InOutItemBehaiver), true) as InOutItemBehaiver;
            node_B = EditorGUILayout.ObjectField("B子节点", node_B, typeof(NodeItemBehaiver), true) as NodeItemBehaiver;

            if (GUILayout.Button("建立坐标关系"))
            {
                CreateConnect();
            }
        }

        void CreateConnect()
        {
            if (itemA == null || itemB == null || node_A == null || itemA == null || node_B == null)
            {
                EditorUtility.DisplayDialog("警告", "请将对象赋值", "确认");
            }
            else
            {
                RecordInout();
                ConnectAble nodeArecored = node_A.connectAble.Find((x) => x.itemName == itemA.itemInfo.name && x.nodeId == node_B.Info.nodeID);
                ConnectAble nodeBrecored = node_B.connectAble.Find((x) => x.itemName == itemB.itemInfo.name && x.nodeId == node_A.Info.nodeID);
                //已经记录过
                if (nodeArecored == null)
                {
                    nodeArecored = new ConnectAble();
                    node_A.connectAble.Add(nodeArecored);
                }
                if (nodeBrecored == null)
                {
                    nodeBrecored = new ConnectAble();
                    node_B.connectAble.Add(nodeBrecored);
                }

                RecoreNameAndID(nodeArecored, nodeBrecored);
                RecordTransform(nodeArecored, nodeBrecored, itemA.transform, itemB.transform);

                EditorUtility.SetDirty(node_A);
                EditorUtility.SetDirty(node_B);
                EditorUtility.DisplayDialog("通知", "Complete", "确认");
            }
        }

        void RecordInout()
        {
            node_A.Info.isIn = aIn;
            node_B.Info.isIn = !aIn;
            EditorUtility.SetDirty(node_A);
            EditorUtility.SetDirty(node_B);
        }

        void RecoreNameAndID(ConnectAble nodeArecored, ConnectAble nodeBrecored)
        {
            nodeArecored.itemName = itemB.itemInfo.name;
            nodeBrecored.itemName = itemA.itemInfo.name;
            nodeArecored.nodeId = node_B.Info.nodeID;
            nodeBrecored.nodeId = node_A.Info.nodeID;
        }

        void RecordTransform(ConnectAble nodeArecored, ConnectAble nodeBrecored, Transform ourItem, Transform otherItem)
        {
            nodeArecored.relativePos = ourItem.InverseTransformPoint(otherItem.position);
            nodeArecored.relativeRot = Quaternion.Inverse(ourItem.rotation) * otherItem.rotation;

            nodeBrecored.relativePos = otherItem.InverseTransformPoint(ourItem.position);
            nodeBrecored.relativeRot = Quaternion.Inverse(otherItem.rotation) * ourItem.rotation;
        }
    }
}