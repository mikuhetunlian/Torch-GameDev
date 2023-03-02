using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//真正替代执行mono的地方
public class MonoController:MonoBehaviour
{
    private event UnityAction eventAction;

    private void Update()
    {
        //如果有委托执行的，那就不断在update中执行
        if (eventAction != null)
        {
            eventAction.Invoke();
        }
    }

   

    public void AddUpdateLinstener(UnityAction action)
    {
        eventAction += action;
    }

    public void RemoveLinstener(UnityAction action)
    {
        eventAction -= action;
    }

}
