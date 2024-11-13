using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System;

public class UIManager
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UIManager();
            }
            return _instance;
        }
    }

    private Dictionary<string, UIPanelAsset> m_AllPanelAsset;

    private Dictionary<string, BasePanel> m_CurrentPanels;

    private Stack<BasePanel> m_PanelStack;

    private Canvas canvas;

    private readonly string panelPaths = UIConfigDef.PANEL_PATHS_PATH;

    private Transform backGround;
    private Transform normal;
    private Transform poped;
    private Transform top;
    
    private UIManager()
    {
        m_AllPanelAsset = new();
        m_CurrentPanels = new();
        m_PanelStack = new();
        GetAllPanels();

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        if (!canvas)
        {
            canvas = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/Canvas")).GetComponent<Canvas>();
            canvas.name = canvas.name.Replace("(Clone)", "");
            canvas.sortingOrder = 0;
        }

        backGround = canvas.transform.Find("Background");
        if (!backGround)
        {
            backGround = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/Background")).GetComponent<Transform>();
            backGround.name = backGround.name.Replace("(Clone)", "");
            backGround.SetParent(canvas.transform);
            backGround.GetComponent<Canvas>().sortingOrder = 1;
            backGround.SetAsFirstSibling();
        }

        normal = canvas.transform.Find("Normal");
        if (!normal)
        {
            normal = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/Normal")).GetComponent<Transform>();
            normal.name = normal.name.Replace("(Clone)", "");
            normal.SetParent(canvas.transform);
            backGround.GetComponent<Canvas>().sortingOrder = 10;
        }

        poped = canvas.transform.Find("Poped");
        if (!poped)
        {
            poped = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/Poped")).GetComponent<Transform>();
            poped.name = poped.name.Replace("(Clone)", "");
            poped.SetParent(canvas.transform);
            backGround.GetComponent<Canvas>().sortingOrder = 50;
        }

        top = canvas.transform.Find("Top");
        if (!top)
        {
            top = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/Top")).GetComponent<Transform>();
            top.name = top.name.Replace("(Clone)", "");
            top.SetParent(canvas.transform);
            backGround.GetComponent<Canvas>().sortingOrder = 100;
            top.SetAsLastSibling();
        }
        GameObject.DontDestroyOnLoad(canvas);
    }

    //根据配置文件加载所有panel的路径
    private void GetAllPanels()
    {
        StreamReader reader = File.OpenText(panelPaths);
        string result = reader.ReadToEnd();
        List<UIPanelAsset> panelAssetList = JsonMapper.ToObject<List<UIPanelAsset>>(result);
        foreach(var panelAsset in panelAssetList)
        {
            m_AllPanelAsset.Add(panelAsset.panelName, panelAsset);
        }
    }

    /// <summary>
    /// 获取指定页面
    /// </summary>
    /// <returns></returns>
    private BasePanel GetPanel(string panelName)
    {
        m_AllPanelAsset.TryGetValue(panelName, out UIPanelAsset panelAsset);
        GameObject newPanelObject = GameObject.Instantiate(Resources.Load<GameObject>(panelAsset.panelPath), canvas.GetComponent<Transform>());
        newPanelObject.name = newPanelObject.name.Replace("(Clone)", "");

        BasePanel newPanel = newPanelObject.GetComponent<BasePanel>();
        if (newPanel == null) {
            Debug.Log(panelName);
        }
        newPanel.panelAsset = panelAsset;

        if (m_PanelStack.Count > 0)
        {
            BasePanel peekPanel = m_PanelStack.Peek();
            peekPanel.OnCover();
        }
        m_PanelStack.Push(newPanel);
        m_CurrentPanels.Add(panelName, newPanel);
        newPanel.OnEnter();
        

        UIPanelType type = (UIPanelType)Enum.Parse(typeof(UIPanelType), newPanel.panelAsset.panelTypeString);

        switch (type)
        {
            case UIPanelType.Background:
                newPanel.transform.SetParent(backGround);
                break;
            case UIPanelType.Normal:
                newPanel.transform.SetParent(normal);
                break;
            case UIPanelType.Poped:
                newPanel.transform.SetParent(poped);
                break;
            case UIPanelType.Top:
                newPanel.transform.SetParent(top);
                break;
        }

        return newPanel;
    }

    /// <summary>
    /// 打开界面
    /// </summary>
    public BasePanel OpenPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        BasePanel panel;
        if(m_CurrentPanels.TryGetValue(panelName, out panel))
        {
            panel.OnEnter();
            m_PanelStack.Push(panel);
            return panel;
        }

        panel = GetPanel(panelName);

        return panel;
    }



    /// <summary>
    /// 关闭界面
    /// </summary>
    public void ClosePanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        //Debug.Log("当前栈里还有" + m_PanelStack.Count);
        if(m_PanelStack.Peek().name == panelName)
        {
            BasePanel topPanel = m_PanelStack.Pop();
            topPanel.OnExit();
            m_CurrentPanels.Remove(topPanel.name);
            if(m_PanelStack.Count > 0)
            {
                m_PanelStack.Peek().OnResume();
            }
            return;
        }
    }
}
