using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> where T : struct, IComparable, IConvertible, IFormattable
{
    /// <summary>
    /// 是否需要触发event
    /// </summary>
    public bool isTriggerEvent { get; set; }

    public GameObject Target;

    public T CurrentState { get; set; }

    public T PerviousState { get; set; }


    public StateMachine(GameObject target, bool isTriggerEvent)
    {
        this.Target = target;
        this.isTriggerEvent = isTriggerEvent;
        CurrentState = default(T);
        PerviousState = default(T);
    }


    /// <summary>
    /// 更新 CurrentState 和 PerviousState
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(T newState)
    {
        if (CurrentState.Equals(newState))
        {
            return;
        }

        PerviousState = CurrentState;
        CurrentState = newState;

        if (isTriggerEvent)
        {
            //这里用evnetManager来传递状态改变需要触发的event
        }
    }
}
