using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushAndPull : PlayerAblity
{

    protected bool _controlAble;
    //����һ�²ٿ��е�����
    protected GameObject controllingObj;
    protected HorizontalMove _horizontalMove;
    protected bool _canPush;
    public override void Initialization()
    {
        base.Initialization();
    }

    public override void GetComponents()
    {
        base.GetComponents();
        _horizontalMove = _player.gameObject.GetComponent<HorizontalMove>();
    }

    /// <summary>
    /// �ж��ܲ����ƺ���
    /// </summary>
    public override void HandleInput()
    {
        if (_inputManager.ControlButton.State.CurrentState == InputHelper.ButtonState.ButtonPressed)
        {
            _controlAble = true;

        }
        if (_inputManager.ControlButton.State.CurrentState == InputHelper.ButtonState.ButtonUp)
        {
            _controlAble = false;
            StopPushableSpeed(); 
        }
    }

    public override void ProcessAbility()
    {
        PushOrPull();
    }

    /// <summary>
    /// ����Player.FacingDirections��_horizontalInput��ʵ���ƺ���
    /// �Ƶ�ʱ���뻮��̫Զ���Ƶ��ٶ�Ҫ���� ��д����4
    /// ����ʱ��Ҫ������player�������ٶ�Ҫ��player�ƶ����ٶ�һ�� ��д����16
    /// </summary>
    public void PushOrPull()
    {
        //�����⵽�˿��Բٿص����� �� �����˲ٿصİ�ť
        if (_controlAble && _playerController.State.isDetectControlableObject)
        {
            if (_playerController.ControlAbleObject == null)
            {
                return;
            }

            if (_horizontalInput == 0)
            {
                _playerController.State.IsControlingRight = false;
                _playerController.State.IsControlingLeft = false;
                return;
            }

            controllingObj = _playerController.ControlAbleObject;
            //���� ����
            if (_horizontalInput > 0 && _player.CurrentFaceingDir == Player.FacingDirections.Right )
            {
                _playerController.ControlAbleObject.GetComponent<Pushable>().SetSpeed( _playerController.Parameters.Physic2DPushOrPullForce);
            }

            //���� ����
            if (_horizontalInput > 0 && _player.CurrentFaceingDir == Player.FacingDirections.Left)
            {
                _playerController.ControlAbleObject.GetComponent<Pushable>().SetSpeed(_playerController.Parameters.Physic2DPushOrPullForce);
            }

            //���� ����
            if (_horizontalInput < 0 && _player.CurrentFaceingDir == Player.FacingDirections.Left)
            {
                _playerController.ControlAbleObject.GetComponent<Pushable>().SetSpeed(-_playerController.Parameters.Physic2DPushOrPullForce);
            }

            //���� ����
            if (_horizontalInput < 0 && _player.CurrentFaceingDir == Player.FacingDirections.Right)
            {
                _playerController.ControlAbleObject.GetComponent<Pushable>().SetSpeed(-_playerController.Parameters.Physic2DPushOrPullForce);
            }

      
            float distance = controllingObj.transform.position.x - transform.position.x;
            if (distance > 0)
            {
                _playerController.State.IsControlingLeft = true;
            }
            else if (distance < 0)
            {
                _playerController.State.IsControlingRight = true;
            }
        }
        else
        {
            _playerController.State.IsControlingRight = false;
            _playerController.State.IsControlingLeft = false;
        }

      
    }

    /// <summary>
    /// ͣ�����ڿ��Ƶ�����
    /// </summary>
    public void StopPushableSpeed()
    {
        if (controllingObj != null)
        {
            controllingObj.GetComponent<Pushable>().SetSpeed(0);
        }
       
    }

}
