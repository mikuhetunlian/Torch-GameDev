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

    private Transform Canvas;

    public UIMgr()
    {
        //�ڵ���GetInstance��ʱ���ҵ�canvas��Transform ,���������л�������ʱ���Ƴ�
        //ResMgr������Ǽ���GameObject�Ļ�ֱ�ӷ��صľ���GameObject



        //GameObject canvasObj = ResMgr.GetInstance().LoadRes<GameObject>("UI/Canvas");
        //canvasObj.name = "Canvas";
        //GameObject.DontDestroyOnLoad(canvasObj);
        
        //����������Դ�Canvas��EventSytem�Ļ����Ǿ�ֱ���ҵ�������
         Canvas = GameObject.Find("Canvas").transform;


        //GameObject eventObj = ResMgr.GetInstance().LoadRes<GameObject>("UI/EventSystem");
        //eventObj.name = "EventSystem";
        //GameObject.DontDestroyOnLoad(eventObj);
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


        GameObject panel = GameObject.Instantiate(Resources.Load("UI/panel/" + panelName)) as GameObject;

        

        panel.name = panelName;
        Debug.Log("�ɹ�������" + panel.name + "����");

        //�ŵ���Ӧ��canvas�㼶��
        if (Canvas == null)
        {
            Canvas = GameObject.Find("Canvas").transform;
        }
        panel.transform.parent = Canvas;

        //�������Canvas��λ�ú��Լ�panel������
        panel.transform.localPosition = Vector3.zero;
        panel.transform.localScale = Vector3.one;
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
