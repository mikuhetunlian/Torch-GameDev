using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : SingeltonAutoManager<Timer>
    
{
    // 时间，0代表定时器未开启
    public float time = 0;

    protected float endTime;
    protected UnityAction actionDelegate;
    protected bool IsTime = false;


  
    private void FixedUpdate()
    {
        // 更新时间
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
    /// 设置定时器的时间
    /// </summary>
    /// <param name="needTime">需要定时器执行多长的时间</param>
    /// <param name="action">定时器时间到到了之后需要执行什么动作</param>
    public void SetTimer(float needTime, UnityAction action)     
    {
        this.IsTime = true;
        this.endTime = needTime;
        this.actionDelegate = action;

        time = 0;



    }    





}
