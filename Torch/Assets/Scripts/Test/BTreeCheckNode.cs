using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class BTreeCheckNode : Conditional
{

    public bool condition;

    public override TaskStatus OnUpdate()
    {
        return condition ? TaskStatus.Success : TaskStatus.Failure;
    }
}
