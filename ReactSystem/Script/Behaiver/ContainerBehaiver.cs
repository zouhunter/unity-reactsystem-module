using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace ReactSystem
{
    public class ContainerBehaiver : MonoBehaviour, IContainer
    {
        public TextAsset inportText;
        public TextAsset outportText;
        public TextAsset equationText;
        public InputField.SubmitEvent onExportError;//元素导出失败
        public InputField.SubmitEvent onElementAppear;//元素生成事件

        public GameObject Go
        {
            get
            {
                return gameObject;
            }
        }
        public event Func<IContainer, int, string[], bool> onExport;
        public event UnityAction<IContainer> onComplete;

        private List<Port> inPorts = new List<Port>();
        private List<Port> outPorts = new List<Port>();
        private List<Equation> equations = new List<Equation>();
        private InteractPool interactPool;
        private Coroutine coroutine;
        private void Start()
        {
            LoadConfigData();
            interactPool = new InteractPool(equations);
            interactPool.onNewElementGenerat = OnGenerateNewItem;
            interactPool.onAllEquationComplete = OnAllEquationComplete;
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadConfigData()
        {
            if (inportText != null)
            {
                var grid = ParserCSV.Parse(inportText.text);
                for (int i = 1; i < grid.Length; i++)
                {
                    Port row = new Port();
                    row.id = int.Parse(grid[i][0]);
                    row.active = bool.Parse(grid[i][1]);
                    row.supportTypes = grid[i][2].Split('|');
                    row.closeinfomation = grid[i][3];
                    inPorts.Add(row);
                }
            }
            if (outportText != null)
            {
                var grid = ParserCSV.Parse(outportText.text);
                for (int i = 1; i < grid.Length; i++)
                {
                    Port row = new Port();
                    row.id = int.Parse(grid[i][0]);
                    row.active = bool.Parse(grid[i][1]);
                    row.supportTypes = grid[i][2].Split('|');
                    row.closeinfomation = grid[i][3];
                    outPorts.Add(row);
                }
            }
            if (equationText != null)
            {
                var grid = ParserCSV.Parse(equationText.text);
                for (int i = 1; i < grid.Length; i++)
                {
                    Equation row = new Equation();
                    if(!string.IsNullOrEmpty(grid[i][0])) row.intypes = grid[i][0].Split('|');
                    if (!string.IsNullOrEmpty(grid[i][1])) row.outtypes = grid[i][1].Split('|');
                    row.interactTime = float.Parse(grid[i][2]);
                    if (!string.IsNullOrEmpty(grid[i][3])) row.conditions = grid[i][3].Split('|');
                    row.illustrate = grid[i][4];
                    equations.Add(row);
                }
            }
           
        }
        /// <summary>
        /// 开始的时候选择性启动,达到优化
        /// </summary>
        /// <param name="force"></param>
        public void Active(bool force = false)
        {
            //防止重复启动
            if (coroutine != null) return;
            //如果有不需要反应物或条件的则启动反应器
            if (force || equations.Find(x => x.conditions.Length == 0 && x.intypes.Length == 0) != null)
            {
                coroutine = StartCoroutine(interactPool.LunchInteractPool());
            }
        }

        /// <summary>
        /// 输入反应物
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="types"></param>
        /// <param name="onComplete"></param>
        public void Import(int nodeId, string[] types)
        {
            Active(true);
            var import = inPorts.Find(x => x.id == nodeId);
            if (import != null)
            {
                foreach (var item in types)
                {
                    if (Array.Find(import.supportTypes, x => item == x)!=null)//.Contains(item))
                    {
                        if (import.active)
                        {
                            interactPool.AddElements(item);
                            onElementAppear.Invoke(item);
                        }
                        else
                        {
                            Debug.Log(import.id + "接口关闭");
                            Debug.Log(import.closeinfomation);
                        }
                    }
                    else
                    {
                        Debug.Log(item + "未添加匹配");
                    }
                }
            }
            else
            {
                foreach (var item in types)
                {
                    Debug.Log(item + "未添加匹配");
                }
            }
        }

        /// <summary>
        /// 试图将生成的元素导出
        /// </summary>
        /// <param name="element"></param>
        private void OnGenerateNewItem(string element)
        {
            onElementAppear.Invoke(element);
            //判断状态
            if (onExport == null) return;
            Dictionary<int, List<string>> exportDic = new Dictionary<int, List<string>>();
            var mightExporters = outPorts.FindAll(x => Array.Find(x.supportTypes, y => y == element) != null);
            if (mightExporters.Count > 0)
            {
                foreach (var item in mightExporters)
                {
                    if (item.active)
                    {
                        if (exportDic.ContainsKey(item.id))
                        {
                            exportDic[item.id].Add(element);
                        }
                        else
                        {
                            exportDic[item.id] = new List<string>() { element };
                        }
                    }
                    else
                    {
                        Debug.Log(item.id + "接口关闭");
                        Debug.Log(item.closeinfomation);
                    }
                }
            }
            foreach (var item in exportDic)
            {
                var status = onExport.Invoke(this, item.Key, item.Value.ToArray());
                if (!status)
                {
                    onExportError.Invoke(element);
                    Debug.Log("导出失败:" + element);
                }
            }
        }

        /// <summary>
        /// 完成当前步骤
        /// </summary>
        private void OnAllEquationComplete()
        {
            Debug.Log(name + ":反应结束",gameObject);
            if (onComplete != null) onComplete.Invoke(this);
        }
    }
}