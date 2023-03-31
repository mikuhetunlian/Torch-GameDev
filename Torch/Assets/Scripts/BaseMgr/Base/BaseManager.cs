using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//没继承mono的 单例模式类的基类
public class BaseManager<T> where T:new()
{
   
    private static T instance;

    public static T GetInstance()
    {
        if (instance == null)
        {
            instance = new T();
        }
        return instance;
    }


}
