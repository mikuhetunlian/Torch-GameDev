using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum E_UI_Layer
{
    Bot,
    Middle,
    Top,
    System
}


public class UIMgr : BaseManager<UIMgr>
{
    
    public Dictionary<string,BasePanel> panelDic = new Dictionary<string,BasePanel>();


    protected GameObject canvasObj;
    protected GameObject eventObj;

    private Transform CanvasTransform;
    private Transform EventTransform;

    public UIMgr()
    {
        //在调用GetInstance的时候找到canvas的Transform ,并让其在切换场景的时候不移除
        //ResMgr中如果是加载GameObject的话直接返回的就是GameObject



        //canvasObj = ResMgr.GetInstance().LoadRes<GameObject>("UI/Canvas");
        canvasObj = GameObject.Find("Canvas");
        if (canvasObj != null)
        {
            canvasObj.name = "Canvas";
            CanvasTransform = canvasObj.transform;
            GameObject.DontDestroyOnLoad(canvasObj);
        }

        //eventObj = ResMgr.GetInstance().LoadRes<GameObject>("UI/EventSystem");
        eventObj = GameObject.Find("EventSystem");
        if (eventObj != null)
        {
            eventObj.name = "EventSystem";
            EventTransform = eventObj.transform;
            GameObject.DontDestroyOnLoad(eventObj);
        }
     
        //如果场景中自带Canvas和EventSytem的话，那就直接找到就行了
        //Canvas = GameObject.Find("Canvas").transform;
    }


    //目前是通过ResMgr的同步加载来加载UI界面，因为异步加载的话感觉会有一帧的空隙没有显示东西
    public GameObject ShowPanel<T>(UnityAction<T> callback = null) where T:BasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
        {
            return null;
        }

        //GameObject panel =  ResMgr.GetInstance().LoadRes<GameObject>("UI/panel/" + panelName);


        GameObject panel = GameObject.Instantiate(Resources.Load("UI/Panel/" + panelName)) as GameObject;

        

        panel.name = panelName;
        Debug.Log("成功加载了" + panel.name + "对象");

        //放到对应的canvas层级下
        if (CanvasTransform == null)
        {
            CanvasTransform = GameObject.Find("Canvas").transform;
        }
        panel.transform.parent = CanvasTransform;

        //设置相对Canvas的位置和自己panel的缩放
        panel.transform.localPosition = Vector3.zero;
        panel.transform.localScale = Vector3.one * 1.01f;
        //设置屏幕大小自适应的 up = 0 和 down = 0
        (panel.transform as RectTransform).offsetMax = Vector2.zero;
        (panel.transform as RectTransform).offsetMin = Vector2.zero;

        T component = panel.GetComponent<T>();
        component.ShowMe();

        if (callback != null)
        {
            callback(component);
        }

        panelDic.Add(panelName, component);
        return panel;


    }

    


    /// <summary>
    /// 移除panel 并销毁它
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void HidePanel<T>() where T:BasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
        {
            Debug.Log("destory" + "找到了panel");

            if (panelDic[panelName].gameObject != null)
            {
                GameObject.Destroy(panelDic[panelName].gameObject);
            }
            panelDic.Remove(panelName);
        }
    }

    public T GetPanel<T>(string panelName) where T:BasePanel
    {
        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }
        return null;
    }


    public void Clear()
    {
        panelDic.Clear();
    }

    /// <summary>
    /// 为组件添加自定义的UI互动事件
    /// </summary>
    /// <param name="control"></param>
    /// <param name="type"></param>
    /// <param name="action"></param>
    public static void AddCustomEventTrigger(UIBehaviour control,EventTriggerType type,UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = control.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(action);

        trigger.triggers.Add(entry);
    }
}
