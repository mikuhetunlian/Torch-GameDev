using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : SingeltonAutoManager<Timer>
    
{
    // ʱ�䣬0����ʱ��δ����
    public float time = 0;

    protected float endTime;
    protected UnityAction actionDelegate;
    protected bool IsTime = false;


  
    private void FixedUpdate()
    {
        // ����ʱ��
        if (IsTime)
        {
            time += Time.fixedDeltaTime;
        }
        
        if (endTime <= time)
        {
            actionDelegate?.Invoke();
            Debug.Log( "zhi xing shi"+ time);
            time = 0;
            IsTime = false;
        }
    }


    /// <summary>
    /// ���ö�ʱ����ʱ��
    /// </summary>
    /// <param name="needTime">��Ҫ��ʱ��ִ�ж೤��ʱ��</param>
    /// <param name="action">��ʱ��ʱ�䵽����֮����Ҫִ��ʲô����</param>
    public void SetTimer(float needTime, UnityAction action)     
    {
        this.IsTime = true;
        this.endTime = needTime;
        this.actionDelegate = action;

        time = 0;



    }    





}
