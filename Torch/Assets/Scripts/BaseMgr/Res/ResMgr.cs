using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResMgr : BaseManager<ResMgr>
{
    //ͬ��������Դ
    //����Ǽ���GameObj�Ļ���ʵ�����������ٴ���ȥ
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


    //�첽������Դ
    //����Ǽ���GameObj�Ļ���ʵ�����������ٴ���ȥ
    public void LoadResAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        MonoManager.GetInstance().StartCoroutine(DoLoadResAsync<T>(name, callback));
    }

    //�����첽���ص�Э��
    //ͨ��callback���첽���ص���Դ���س�ȥ
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
