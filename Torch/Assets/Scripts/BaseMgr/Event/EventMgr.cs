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
    /// 添加事件监听到事件字典中
    /// </summary>
    /// <typeparam name="T">委托函数的参数类型，不能省略</typeparam>
    /// <param name="eventName">想要触发的事件名</param>
    /// <param name="action">想要触发的委托函数</param>
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
    /// 从事件监听字典中移除事件
    /// </summary>
    /// <typeparam name="T">委托函数的参数类型，不能省略</typeparam>
    /// <param name="eventName">想要移除的时间名</param>
    /// <param name="action">想要移除的委托函数</param>
    public void RemoveLinstener<T>(string eventName,UnityAction<T> action)
    {
        if(eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo<T>).eventAction -= action;
        }
    }

    /// <summary>
    /// 触发并通知注册了监听该事件的对象执行委托函数
    /// </summary>
    /// <typeparam name="T">委托函数的参数类型，不能省略</typeparam>
    /// <param name="eventName">想要触发的事件名字</param>
    /// <param name="info">想要传递到委托中的泛型参数</param>
    public void EventTrigger<T>(string eventName,T info)
    {
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo<T>).eventAction.Invoke(info);
        }
    }

    /// <summary>
    /// 防止切换场景的时候evnetDic还保留着上个场景的引用,在切换场景的时候记得Clear一下
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }


    
}
