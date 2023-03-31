using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : PlayerAblity
{
    private float _horizontalMovement;
    private void Start()
    {
        
        GetComponents();
        Initialization();
    }

    public override void Initialization()
    {
        base.Initialization();       
    }

    public override void GetComponents()
    {
        base.GetComponents();

    }

    public override void HandleInput()
    {
        _horizontalMovement = _horizontalInput;   
    }

    public override void ProcessAbility()
    {
        base.ProcessAbility();
        FlipDo();
    }


    /// <summary>
    /// 实现翻转的地方，根据 _horizontalMovement 参数来实现翻转
    /// </summary>
    private void FlipDo()
    {

        if (_horizontalInput == 0)
        {
            return;
        }

        if (_horizontalInput > 0 )
        {
            if (_playerController.ControlAbleObject != null)
            {
                //箱子在player左边
                if (_playerController.ControlAbleObject.transform.position.x < transform.position.x)
                {
                    //面朝左 拉
                    this.transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y));
                    return;
                }
            }

            this.transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) ,Mathf.Abs(transform.localScale.y));
        }


        if(_horizontalInput < 0)
        {
            if (_playerController.ControlAbleObject != null)
            {
                //箱子在player右
                if (_playerController.ControlAbleObject.transform.position.x > transform.position.x)
                {
                    //面朝右 拉
                    this.transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y));
                    return;
                }
            }

            this.transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y));
        }
    }

  
}
