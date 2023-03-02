using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> where T : struct, IComparable, IConvertible, IFormattable
{
    /// <summary>
    /// �Ƿ���Ҫ����event
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
    /// ���� CurrentState �� PerviousState
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
            //������evnetManager������״̬�ı���Ҫ������event
        }
    }
}
