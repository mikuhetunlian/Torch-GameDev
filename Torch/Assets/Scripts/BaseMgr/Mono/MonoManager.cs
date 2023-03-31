using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ���������������ִ��һЩ ��Mono�������� �Ľű���
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


    ///��ִ�еĺ������뵽controller��eventAction���У���Update��ִ��
    public void AddUpdateLinstener(UnityAction action)
    {
        controller.AddUpdateLinstener(action);
    }

    ///��controller��eventAction���Ƴ��ض��ĺ���
    public void RemoveUpdateLinstener(UnityAction action)
    {
        controller.RemoveLinstener(action);
    }


    /// <summary>
    /// ��IEnumeratorΪ��������Э��
    /// </summary>
    /// <param name="routine"></param>
    /// <returns></returns>
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        //��Ҫ����Э�̣��ô���mono��controller�Լ�ֱ�ӿ����ͺ���
        return controller.StartCoroutine(routine);
    }

    /// <summary>
    /// ���ַ���Ϊ��������Э��
    /// </summary>
    /// <param name="routineName"></param>
    /// <returns></returns>
    public Coroutine StartCoroutine(string routineName)
    {
        return controller.StartCoroutine(routineName);
    }

    /// <summary>
    /// ��IEnumeratorΪ����ֹͣЭ�̺���
    /// </summary>
    /// <param name="routine"></param>
    public void StopCoroutine(IEnumerator routine)
    {
        controller.StopCoroutine(routine);
    }

    /// <summary>
    /// ���ַ���Ϊ����ֹͣЭ�̺�����
    /// </summary>
    /// <param name="routineName"></param>
    public void StopCoroutine(string routineName)
    {
        controller.StopCoroutine(routineName);
    }


}
