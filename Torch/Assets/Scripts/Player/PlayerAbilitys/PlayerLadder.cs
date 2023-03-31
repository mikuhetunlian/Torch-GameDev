using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLadder : PlayerAblity
{

    ///爬梯子的速度
    public float LadderClimbingSpeed;
    public bool LadderCollider { get { return _colliders.Count > 0; } }
    ///到达梯子上方时是否自定定位到上方
    public bool ForceAnchorToTopOnExit = true;
    ///装目前碰到的梯子们
    protected List<Collider2D> _colliders;
    ///目前的正在接触的梯子
    protected Ladder _currentLadder;


    /// animator parameters
    protected string _ladderIdleAnimatorParameterName = "ladder_idle";
    protected string _ladderClimbingAnimatorParameterName = "ladder_climbing";
    protected int _ladderIdleAniamtorParameter;
    protected int _ladderClimbingAnimatorParameter;





    public override void Initialization()
    {
        base.Initialization();
        _colliders = new List<Collider2D>();
    }


    public override void ProcessAbility()
    {
        base.ProcessAbility();
       
        ComputeClosetLadder();
        HandleLadderClimbing();
      
    }



    /// <summary>
    /// 计算当前正在爬的梯子是哪一个
    /// 如果由多个梯子拼接成一个梯子，这里的判断会很有用
    /// </summary>
    public void ComputeClosetLadder()
    {
        if (_colliders == null) { return; }
        if (_colliders.Count < 0) { return; }

        if (_colliders.Count > 0)
        {
            _currentLadder = _colliders[0].gameObject.GetComponent<Ladder>();
        }
        
    }


    /// <summary>
    /// 真正处理 在ladder上行为的地方
    /// </summary>
    protected void HandleLadderClimbing()
    {
        if (!AbilityPermitted)
        {
            return;
        }

        //如果正在接触ladder
        if (LadderCollider)
        {
            //如果在爬梯子 且 刚好碰到地面的话
            if (_movement.CurrentState == PlayerStates.MovementStates.LadderClimbing
                && _playerController.State.IsGrounded
                && !_playerController.IsGravityActive)
            {
                GetOffTheLadder();
            }

            //如果不在爬梯子的状态，可以爬上去
            if (_movement.CurrentState != PlayerStates.MovementStates.LadderClimbing
                && !AboveLadderPlatform())
            {
                if (_verticalInput > 0)
                {
                    StartClimbing();
                }
            }

            //如果在爬梯子的状态
            if (_movement.CurrentState == PlayerStates.MovementStates.LadderClimbing)
            {
                Climbing();
            }

            //如果往上爬且刚刚到ladderPlatform时，是否开启卡主位置
            if (_movement.CurrentState == PlayerStates.MovementStates.LadderClimbing
               && AboveLadderPlatform()
               && _verticalInput > 0)
            {
                if (ForceAnchorToTopOnExit)
                {
                    _playerController.AnchorToGround();
                    GetOffTheLadder();
                }

            }
            
            //如果在梯子平台上，可以爬下去
            if (_movement.CurrentState != PlayerStates.MovementStates.LadderClimbing
                && _movement.CurrentState != PlayerStates.MovementStates.Jumping
                && AboveLadderPlatform())
            {
                if (_verticalInput < 0)
                {
                    StartClimbingDown();
                }
            }

        }
        else
        {
            if (_movement.CurrentState == PlayerStates.MovementStates.LadderClimbing)
            {
                GetOffTheLadder();
            }
        }
       
    }

    /// <summary>
    /// 开始向上爬梯子
    /// </summary>
    protected void StartClimbing()
    {
        _movement.ChangeState(PlayerStates.MovementStates.LadderClimbing);
        //关闭重力
        _playerController.GravityActive(false);

        //如果开启了在梯子中间位置，就设置一次坐标
        if (_currentLadder.CenterOnLadder)
        { 
            _transform.position = new Vector3(_currentLadder.transform.position.x, _transform.position.y, _transform.position.z);
        }
    }

    /// <summary>
    /// 开始向下爬梯子
    /// </summary>
    protected void StartClimbingDown()
    {
        _movement.ChangeState(PlayerStates.MovementStates.LadderClimbing);
        _playerController.GravityActive(false);
        _playerController.CollisionOff();

        //如果开启了在梯子中间位置，就设置一次坐标
        if (_currentLadder.CenterOnLadder)
        {
            _transform.position = new Vector3(_currentLadder.transform.position.x, _transform.position.y, _transform.position.z);
        }

    }

    /// <summary>
    /// 正在爬梯子时的逻辑
    /// </summary>
    protected void Climbing()
    {

        if (_movement.CurrentState == PlayerStates.MovementStates.LadderClimbing
            && BelowLadderPlatform())
        {
            _playerController.CollisonOn();
           
        }

        _playerController.SetVerticalForce(_verticalInput * LadderClimbingSpeed);
    }


    /// <summary>
    /// 离开ladder要做的事 
    /// 可以由其他ability调用
    /// </summary>
    public void GetOffTheLadder()
    {
        _movement.ChangeState(PlayerStates.MovementStates.Idle);
        _playerController.GravityActive(true);
        _playerController.CollisonOn();
    }


    /// <summary>
    /// 用于判断player是否站在了ladderPlatform上
    /// </summary>
    /// <returns></returns>
    public bool AboveLadderPlatform()
    {
        if (!LadderCollider) 
        {
            return false;
        }
    
        float ladderPlatformTopY =   _currentLadder.LadderPlatform.GetComponent<BoxCollider2D>().bounds.center.y + 
                                     _currentLadder.LadderPlatform.GetComponent<BoxCollider2D>().bounds.extents.y;

        float distance = _transform.position.y - ladderPlatformTopY;

        if (distance >= 0 && distance < 1f)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    /// <summary>
    ///  用于判断player是否在了ladderPlatform下方
    /// </summary>
    /// <returns></returns>
    public bool BelowLadderPlatform()
    {
        if (!LadderCollider)
        {
            return false;
        }

        float ladderPlatformButtonY = _currentLadder.LadderPlatform.GetComponent<BoxCollider2D>().bounds.center.y -
                                     _currentLadder.LadderPlatform.GetComponent<BoxCollider2D>().bounds.extents.y;

        float distance = ladderPlatformButtonY - (_playerController.Collider.bounds.center.y + 
                                                 _playerController.Collider.bounds.extents.y);

        if (distance > 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }


    /// <summary>
    /// 添加新碰到的 ladder 的 collider2D
    /// </summary>
    /// <param name="collider"></param>
    public void AddColliderLadder(Collider2D collider)
    {
        if (_colliders == null)
        {
            _colliders = new List<Collider2D>();
        }
        _colliders.Add(collider);
        Debug.Log(_colliders.Count);
    }

    /// <summary>
    /// 移除离开时的 ladder 的 collider2D
    /// </summary>
    /// <param name="collider"></param>
    public void RemoveColliderLadder(Collider2D collider)
    {
        if (_colliders == null)
        {
            return;
        } 
        _colliders.Remove(collider);
    }




    protected override void InitializeAnimatorParameter()
    {
        RegisterAnimatorParameter(_ladderIdleAnimatorParameterName, AnimatorControllerParameterType.Bool, out _ladderIdleAniamtorParameter);
        RegisterAnimatorParameter(_ladderClimbingAnimatorParameterName, AnimatorControllerParameterType.Bool, out _ladderClimbingAnimatorParameter);
    }

    public override void UpdateAnimator()
    {

        if (_movement.CurrentState != PlayerStates.MovementStates.LadderClimbing)
        {
            AnimatorHelper.UpdateAnimatorBool(_animator, _ladderIdleAniamtorParameter, false, _player._animatorParameters);
            AnimatorHelper.UpdateAnimatorBool(_animator, _ladderClimbingAnimatorParameter, false, _player._animatorParameters);
        }

        if (_movement.CurrentState == PlayerStates.MovementStates.LadderClimbing
            && _verticalInput !=0)
        {
            AnimatorHelper.UpdateAnimatorBool(_animator, _ladderIdleAniamtorParameter, false, _player._animatorParameters);
            AnimatorHelper.UpdateAnimatorBool(_animator, _ladderClimbingAnimatorParameter, true, _player._animatorParameters);
          
        }

        if (_movement.CurrentState == PlayerStates.MovementStates.LadderClimbing
            && _verticalInput == 0)
        {
            AnimatorHelper.UpdateAnimatorBool(_animator, _ladderIdleAniamtorParameter, true, _player._animatorParameters);
            AnimatorHelper.UpdateAnimatorBool(_animator, _ladderClimbingAnimatorParameter, false, _player._animatorParameters);

        }
    }

}
