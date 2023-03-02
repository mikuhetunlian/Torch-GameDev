using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public interface IinfoEvent
{

}

public class EventInfo<T>: IinfoEvent
{
    public UnityAction<T> eventAction;
    public EventInfo(UnityAction<T> action)
    {
        eventAction += action;
    }
}


public class EventMgr :BaseManager<EventMgr>
{
    private Dictionary<string, IinfoEvent> eventDic = new Dictionary<string, IinfoEvent>();

    /// <summary>
    /// ����¼��������¼��ֵ���
    /// </summary>
    /// <typeparam name="T">ί�к����Ĳ������ͣ�����ʡ��</typeparam>
    /// <param name="eventName">��Ҫ�������¼���</param>
    /// <param name="action">��Ҫ������ί�к���</param>
    public void AddLinstener<T>(string eventName,UnityAction<T> action)
    {
        if (!eventDic.ContainsKey(eventName))
        {
            eventDic.Add(eventName, new EventInfo<T>(action));
        }
        else
        {
            (eventDic[eventName] as EventInfo<T>).eventAction += action;
        }
    }

    /// <summary>
    /// ���¼������ֵ����Ƴ��¼�
    /// </summary>
    /// <typeparam name="T">ί�к����Ĳ������ͣ�����ʡ��</typeparam>
    /// <param name="eventName">��Ҫ�Ƴ���ʱ����</param>
    /// <param name="action">��Ҫ�Ƴ���ί�к���</param>
    public void RemoveLinstener<T>(string eventName,UnityAction<T> action)
    {
        if(eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo<T>).eventAction -= action;
        }
    }

    /// <summary>
    /// ������֪ͨע���˼������¼��Ķ���ִ��ί�к���
    /// </summary>
    /// <typeparam name="T">ί�к����Ĳ������ͣ�����ʡ��</typeparam>
    /// <param name="eventName">��Ҫ�������¼�����</param>
    /// <param name="info">��Ҫ���ݵ�ί���еķ��Ͳ���</param>
    public void EventTrigger<T>(string eventName,T info)
    {
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo<T>).eventAction.Invoke(info);
        }
    }

    /// <summary>
    /// ��ֹ�л�������ʱ��evnetDic���������ϸ�����������,���л�������ʱ��ǵ�Clearһ��
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }


    
}
