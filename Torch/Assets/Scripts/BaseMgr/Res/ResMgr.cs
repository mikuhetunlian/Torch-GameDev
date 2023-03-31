using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResMgr : BaseManager<ResMgr>
{
    //同步加载资源
    //如果是加载GameObj的话，实例化出来后再传出去
    public T LoadRes<T>(string pathName) where T:Object
    {
        T res = Resources.Load<T>(pathName);
        if (res is GameObject)
        {
            GameObject obj = GameObject.Instantiate(res) as GameObject;
            obj.name = pathName;
            return res as T;
        }
        return res;
    }


    //异步加载资源
    //如果是加载GameObj的话，实例化出来后再传出去
    public void LoadResAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        MonoManager.GetInstance().StartCoroutine(DoLoadResAsync<T>(name, callback));
    }

    //真正异步加载的协程
    //通过callback把异步加载的资源返回出去
    private IEnumerator DoLoadResAsync<T>(string name,UnityAction<T> callback) where T:Object
    {
        ResourceRequest req = Resources.LoadAsync<T>(name);
        while (!req.isDone)
        {
            EventMgr.GetInstance().EventTrigger("resLoading", req.progress);
            yield return req.progress;
        }

        if (req.asset is GameObject)
        {
            GameObject gameObject = GameObject.Instantiate(req.asset) as GameObject;
            gameObject.name = name;
            callback(gameObject as T);
        }
        else
        {
            callback(req.asset as T);
        }
      
    }

}
