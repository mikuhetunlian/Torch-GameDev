using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushAndPull : PlayerAblity
{

    protected bool _controlAble;
    //储存一下操控中的物体
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
    /// 判断能不能推和拉
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
    /// 根据Player.FacingDirections和_horizontalInput来实现推和拉
    /// 推的时候不想划的太远，推的速度要降低 先写死了4
    /// 拉的时候要跟得上player，拉的速度要和player移动的速度一样 先写死了16
    /// </summary>
    public void PushOrPull()
    {
        //如果检测到了可以操控的物体 且 按下了操控的按钮
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
            //左推 向右
            if (_horizontalInput > 0 && _player.CurrentFaceingDir == Player.FacingDirections.Right )
            {
                _playerController.ControlAbleObject.GetComponent<Pushable>().SetSpeed( _playerController.Parameters.Physic2DPushOrPullForce);
            }

            //右拉 向右
            if (_horizontalInput > 0 && _player.CurrentFaceingDir == Player.FacingDirections.Left)
            {
                _playerController.ControlAbleObject.GetComponent<Pushable>().SetSpeed(_playerController.Parameters.Physic2DPushOrPullForce);
            }

            //右推 向左
            if (_horizontalInput < 0 && _player.CurrentFaceingDir == Player.FacingDirections.Left)
            {
                _playerController.ControlAbleObject.GetComponent<Pushable>().SetSpeed(-_playerController.Parameters.Physic2DPushOrPullForce);
            }

            //左拉 向左
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
    /// 停下正在控制的物体
    /// </summary>
    public void StopPushableSpeed()
    {
        if (controllingObj != null)
        {
            controllingObj.GetComponent<Pushable>().SetSpeed(0);
        }
       
    }

}
