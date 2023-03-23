using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class BTreeNode : Action
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("this is a speed")]
    public float speed;
    public string name;
    public GameObject target;


    public override void OnAwake()
    {
        gameObject = this.gameObject;
        
    }

    public override TaskStatus OnUpdate()
    {
        Debug.Log("自定义的Node DesuYo");
        Debug.Log(gameObject.name);
        Debug.Log(target.name);
        return TaskStatus.Success;
    }
}
