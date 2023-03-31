using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//�������ִ��mono�ĵط�
public class MonoController:MonoBehaviour
{
    private event UnityAction eventAction;

    private void Update()
    {
        //�����ί��ִ�еģ��ǾͲ�����update��ִ��
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
