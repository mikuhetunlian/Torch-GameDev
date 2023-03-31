using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;


public class Check: Conditional
{

    public SharedTransform target;

    protected RaycastHit2D hitInfo;

    public override void OnAwake()
    {
        Physics2D.queriesStartInColliders = false;
  
    }

    public override TaskStatus OnUpdate()
    {
        return RayCast()? TaskStatus.Success :TaskStatus.Running;
    }

    protected bool RayCast()
    {
        hitInfo = DebugHelper.RaycastAndDrawLine(this.transform.position, Vector2.left, 8, LayerMgr.PlayerLayerMask);

        if (hitInfo.transform != null)
        {
            Debug.Log("¼ì²âµ½player");
            target.SetValue(hitInfo.transform);
        }
        else 
        {
            target.SetValue(null);
        }

        return hitInfo.transform ? true : false;
    }




}
