using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PoolMgr:BaseManager<PoolMgr>
{
    private Dictionary<string, List<GameObject>> poolDic = new Dictionary<string, List<GameObject>>();
    private GameObject poolObj;

    /// <summary>
    /// �����������ж��������ã����û�У������µĵ����������
    /// </summary>
    /// <param name="pathName"></param>
    /// <param name="callback"></param>
    public void GetObj(string pathName, UnityAction<GameObject> callback)
    {
        if (poolDic.ContainsKey(pathName) && poolDic[pathName].Count > 0)
        {
            GameObject obj = poolDic[pathName][0];
            obj.transform.parent = null;
            obj.SetActive(true);
            callback(obj);
            poolDic[pathName].RemoveAt(0);
        }
        else 
        {
            ResMgr.GetInstance().LoadResAsync<GameObject>(pathName, (o) =>
             {
                 GameObject obj = o;
                 obj.transform.parent = null;
                 obj.SetActive(true);
                 callback(obj);
             });
        }
    }

    /// <summary>
    /// ѹ���µĶ��󵽻��������
    /// </summary>
    /// <param name="pathName"></param>
    /// <param name="obj"></param>
    public void PushObj(string pathName, GameObject obj)
    {
        obj.SetActive(false);

        if (poolObj == null)
        {
            poolObj = new GameObject("poolObj");
        }

        GameObject subObj = GameObject.Find(pathName);
        if (subObj == null)
        {
            subObj = new GameObject(pathName);
            subObj.transform.parent = poolObj.transform;
        }

        obj.transform.parent = subObj.transform;

        if (!poolDic.ContainsKey(pathName))
        {
            poolDic.Add(pathName, new List<GameObject>());
        }
        poolDic[pathName].Add(obj);
    }

    /// <summary>
    /// ���������ֵ�
    /// </summary>
    public void Clear()
    {
        poolDic.Clear();
    }


   
}
