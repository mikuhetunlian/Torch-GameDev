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
        //�ڵ���GetInstance��ʱ���ҵ�canvas��Transform ,���������л�������ʱ���Ƴ�
        //ResMgr������Ǽ���GameObject�Ļ�ֱ�ӷ��صľ���GameObject



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
     
        //����������Դ�Canvas��EventSytem�Ļ����Ǿ�ֱ���ҵ�������
        //Canvas = GameObject.Find("Canvas").transform;
    }


    //Ŀǰ��ͨ��ResMgr��ͬ������������UI���棬��Ϊ�첽���صĻ��о�����һ֡�Ŀ�϶û����ʾ����
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
        Debug.Log("�ɹ�������" + panel.name + "����");

        //�ŵ���Ӧ��canvas�㼶��
        if (CanvasTransform == null)
        {
            CanvasTransform = GameObject.Find("Canvas").transform;
        }
        panel.transform.parent = CanvasTransform;

        //�������Canvas��λ�ú��Լ�panel������
        panel.transform.localPosition = Vector3.zero;
        panel.transform.localScale = Vector3.one * 1.01f;
        //������Ļ��С����Ӧ�� up = 0 �� down = 0
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
    /// �Ƴ�panel ��������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void HidePanel<T>() where T:BasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
        {
            Debug.Log("destory" + "�ҵ���panel");

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
    /// Ϊ�������Զ����UI�����¼�
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
