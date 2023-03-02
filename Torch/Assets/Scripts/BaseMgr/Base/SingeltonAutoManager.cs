using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//继承了monoBehaviour的单例模式
public class SingeltonAutoManager<T>:MonoBehaviour where T:MonoBehaviour
{
    private static T instance;
    public static T GetInstance()
    {
        if (instance == null)
        {
            GameObject obj = new GameObject(typeof(T).Name);
            instance = obj.AddComponent<T>();
            GameObject.DontDestroyOnLoad(obj);
        }
        return instance;
    }
   
}
