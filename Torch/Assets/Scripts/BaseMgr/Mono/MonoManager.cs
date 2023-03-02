using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 这个类是用来帮助执行一些 有Mono生命周期 的脚本的
/// </summary>
public class MonoManager : BaseManager<MonoManager>
{
    private MonoController controller;

    public MonoManager()
    {
        if (controller == null)
        {
            GameObject obj = new GameObject("controller");
            controller = obj.AddComponent<MonoController>();
            GameObject.DontDestroyOnLoad(obj);
        }
    }


    ///待执行的函数加入到controller的eventAction链中，在Update中执行
    public void AddUpdateLinstener(UnityAction action)
    {
        controller.AddUpdateLinstener(action);
    }

    ///在controller的eventAction链移除特定的函数
    public void RemoveUpdateLinstener(UnityAction action)
    {
        controller.RemoveLinstener(action);
    }


    /// <summary>
    /// 以IEnumerator为参数开启协程
    /// </summary>
    /// <param name="routine"></param>
    /// <returns></returns>
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        //想要开启协程，让带有mono的controller自己直接开启就好了
        return controller.StartCoroutine(routine);
    }

    /// <summary>
    /// 以字符串为参数开启协程
    /// </summary>
    /// <param name="routineName"></param>
    /// <returns></returns>
    public Coroutine StartCoroutine(string routineName)
    {
        return controller.StartCoroutine(routineName);
    }

    /// <summary>
    /// 以IEnumerator为参数停止协程函数
    /// </summary>
    /// <param name="routine"></param>
    public void StopCoroutine(IEnumerator routine)
    {
        controller.StopCoroutine(routine);
    }

    /// <summary>
    /// 以字符串为参数停止协程函数，
    /// </summary>
    /// <param name="routineName"></param>
    public void StopCoroutine(string routineName)
    {
        controller.StopCoroutine(routineName);
    }


}
