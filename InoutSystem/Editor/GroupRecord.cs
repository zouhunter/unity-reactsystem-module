using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
namespace FlowSystem
{
    public class GroupRecord : ScriptableWizard
    {
        [MenuItem("Window/GroupRecord")]
        static void RecordGroup()
        {
            DisplayWizard<GroupRecord>("保存一组实验坐标", "关闭", "记录");
        }

        public ExperimentDataObject experimentData;
        public Transform objectsParent;

        protected override bool DrawWizardGUI()
        {
            return base.DrawWizardGUI();
        }
        //==================================  
        // Messages  
        //==================================  


        // Called every frame when the wizard is visible.  
        void OnDrawGizmos()
        {

        }

        // This is called when the user clicks on the Create button.  
        void OnWizardCreate()
        {

        }

        // This is called when the wizard is opened or whenever the user   
        // changes something in the wizard.  
        void OnWizardUpdate()
        {

        }

        // Allows you to provide an action when the user clicks on the   
        // other button.  
        void OnWizardOtherButton()
        {
            experimentData.elements.Clear();
            GameObject pfb;
            GameObject item;
            RunTimeElemet element;
            for (int i = 0; i < objectsParent.childCount; i++)// (Transform item in objectsParent)
            {
                item = objectsParent.GetChild(i).gameObject;
                //pfb = PrefabUtility.GetPrefabObject(item) as GameObject;
                pfb = PrefabUtility.GetPrefabParent(item) as GameObject;
                if (pfb == null)
                {
                    EditorUtility.DisplayDialog("警告", item.name + "对象未制作预制体", "确认");
                    break;
                }
                element = new RunTimeElemet();
                element.element = pfb;
                element.id = i;
                element.position = item.transform.position;
                element.rotation = item.transform.rotation;
                experimentData.elements.Add(element);
            }
        }
    }
}