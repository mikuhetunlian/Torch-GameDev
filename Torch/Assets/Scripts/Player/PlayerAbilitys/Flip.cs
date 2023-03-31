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
    /// ʵ�ַ�ת�ĵط������� _horizontalMovement ������ʵ�ַ�ת
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
                //������player���
                if (_playerController.ControlAbleObject.transform.position.x < transform.position.x)
                {
                    //�泯�� ��
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
                //������player��
                if (_playerController.ControlAbleObject.transform.position.x > transform.position.x)
                {
                    //�泯�� ��
                    this.transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y));
                    return;
                }
            }

            this.transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y));
        }
    }

  
}
