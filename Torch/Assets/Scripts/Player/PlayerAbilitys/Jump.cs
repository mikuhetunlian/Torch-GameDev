using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : PlayerAblity
{

    public float JumpHeight;
    //上升的速度
    public float upSpeed;
    //下降的速度
    public float fallSpeed;
    //上升时间倍增器
    public float upTimeMutilper;
    //下降时间倍增器
    public float fallTimeMutilper;
    public float fallAddSpeed;
    protected PlayerLadder _playerLadder;


    private PlayerControllerParameters parameter;
    private PlayerControllerState State;


    //是否在上升
    public bool isUping;
    //是否在下降
    public bool isFalling;
    //是否在跳跃 （包括上升和下降）
    public bool isJumping;
    //是否自然掉落
    public bool startNatureFall;
    public bool isTouchGround;
    //是否能再次跳跃
    public bool canJump;


    public override void Initialization()
    {
        base.Initialization();
        JumpHeight = 8;
        upSpeed = 15f;
        fallSpeed = 12f;
        fallTimeMutilper = 8f;
        upTimeMutilper = 5.5f;
        fallAddSpeed = 30;
        isFalling = true;
    }

    

    public override void GetComponents()
    {
        base.GetComponents();
        State = _playerController.State;
        parameter = _playerController.Parameters;
        _playerLadder = GetComponent<PlayerLadder>();
    }

    public override void ProcessAbility()
    {
        SetState();
    }


    public override void HandleInput()
    {
        if(_inputManager.JumpButton.State.CurrentState == InputHelper.ButtonState.ButtonDown)
        {
            JumpStart();
        }
        if (_inputManager.JumpButton.State.CurrentState == InputHelper.ButtonState.ButtonUp)
        {
            JumpGetKeyUp();
        }
    }

    /// <summary>
    /// 给 player 一个向上的瞬时速度 V² = 2as 
    /// </summary>
    public void JumpStart()
    {

        if (_playerController.State.isCollidingBelow
            || _movement.CurrentState == PlayerStates.MovementStates.LadderClimbing)
        {
            if (_movement.CurrentState == PlayerStates.MovementStates.LadderClimbing)
            {
                _playerLadder.GetOffTheLadder();
            }

            if (_playerController.State.IsOnMovingPlatform)
            {
                _playerController.DetachFromMovinPlatform();
            }

            //如果已经跳跃状态就不能在跳跃
            if (_playerController.Speed.y > 0 || !_playerController.State.isCollidingBelow || _movement.CurrentState == PlayerStates.MovementStates.Jumping)
            {
                return;
            }

            _movement.ChangeState(PlayerStates.MovementStates.Jumping);
            _playerController.SetVerticalForce(Mathf.Sqrt(2f * Mathf.Abs(_playerController.Parameters.Gravity) * JumpHeight));
        }

    }

    public void JumpGetKeyUp()
    {
        if (!_playerController.State.IsGrounded && !_playerController.State.IsFalling)
        {

            _playerController.SetVerticalForce(0);
        }
    }

    public void SetState()
    {

        if(!_playerController.State.isCollidingBelow
            && _movement.CurrentState != PlayerStates.MovementStates.LadderClimbing)
        {
            _movement.ChangeState(PlayerStates.MovementStates.Jumping);
        }

        if( _movement.CurrentState == PlayerStates.MovementStates.Jumping 
            && _playerController.State.isCollidingBelow )
        {
            _movement.ChangeState(PlayerStates.MovementStates.Idle);
        }
    }


    /// <summary>
    /// 设置上升速度
    /// </summary>
    public void SetUpSpeed(float speed)
    {
        upSpeed = speed;
    }


    public void ResetJumpPrameters()
    {
        isUping = false;
        isFalling = false;
        isJumping = false;
        startNatureFall = false;
        isTouchGround = false;
        canJump = true;
    }



    /// <summary>
    /// 重置IsTouchGround参数
    /// </summary>
    private void ResetIsTouchGround()
    {
        isTouchGround = false;
    }

    protected override void InitializeAnimatorParameter()
    {
     
    }
    public override void UpdateAnimator()
    {
        
    }


}
