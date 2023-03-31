using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class BTreeNode2 : Action
{


    public SharedTransform BTreeNode2Target;


    public override TaskStatus OnUpdate()
    {

        if (BTreeNode2Target != null)
        {
            Debug.Log("Node2" + BTreeNode2Target.Value.gameObject.name.ToString());
        }
 
        return base.OnUpdate();
    }

}
