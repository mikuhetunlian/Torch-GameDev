using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;


public class Chase : Action
{
    public float speed;
    public SharedTransform target;

    protected Transform targetValue;

    public override TaskStatus OnUpdate()
    {

        targetValue = target.Value;

        Vector2 dir = (targetValue.position - transform.position).normalized;

        //if (Vector2.Distance(targetValue.position, this.transform.position) < 0.01f)
        //{
        //    return TaskStatus.Success;
        //}   
        
        transform.Translate(dir * speed * Time.deltaTime, Space.World);
        return TaskStatus.Running;


    }
}
