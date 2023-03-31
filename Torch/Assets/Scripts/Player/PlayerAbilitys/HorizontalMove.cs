using UnityEngine;

public class HorizontalMove : PlayerAblity
{


    public Player.FacingDirections facingDir = Player.FacingDirections.Right;
    public float speed;
    public bool move;
    private Rigidbody2D rbody;
    [SerializeField]
    private Vector2 _newPostion;

    // 水平移动的方向，每一帧从 _horizontalInput 获取
    protected float _horizontalMovement;
    protected float _horizontalMovementForce;


    /// aniamtor parameters
    protected string _idleAniamtorParameterName = "idle";
    protected string _walkAniamtorParameterName = "walk";
    protected string _touchGroundAnimatorParameterName = "touchGround";
    protected string _tryToTouchAnimatorParameterName = "tryToTouch";
    protected int _idleAnimatorParameter;
    protected int _walkAniamtorParameter;
    protected int _touchGroundAnimatorParameter;
    protected int _tryToTouchAnimatorParameter;


    public override void Initialization()
    {
        base.Initialization();
        speed = 16f;
    }

    public override void GetComponents()
    {
        base.GetComponents();
        rbody = GetComponent<Rigidbody2D>();
        _player = GetComponent<Player>();
    }


    public override void PermitAbility(bool abilityPermitted)
    {
        base.PermitAbility(abilityPermitted);
    }

    public override void HandleInput()
    {
        _horizontalMovement = _horizontalInput;
    }


    public override void ProcessAbility()
    {      
        HorizontalMovement();
    }


    /// <summary>
    /// 根据input输入来 改变 newPostion 的 x分量值
    /// </summary>
    public virtual void HorizontalMovement()
    {
        if (AbilityPermitted)
        {
            if (_playerController.State == null)
            {
                Debug.Log(this.gameObject);
            }
            if (_playerController.State.isCollidingBelow
                && _movement.CurrentState != PlayerStates.MovementStates.LadderClimbing //不在爬梯子
                && _movement.CurrentState != PlayerStates.MovementStates.Jumping //不在跳跃
               )
            {
                // 如果玩家在水平移动，改变 PlayerStates
                if (_horizontalMovement != 0)
                {
                    _movement.ChangeState(PlayerStates.MovementStates.Walking);
                }
                else // 如果水平速度为0，改变 PlayerStates
                {
                    _movement.ChangeState(PlayerStates.MovementStates.Idle);
                       
                }
            }

            //在  推 或 拉 的时候自己的移动速度要降低和pushable物体匹配
            if (_playerController.State.IsControlingRight ||_playerController.State.IsControlingLeft)
            {
                _playerController.SetHorizontalForce(_horizontalMovement * _playerController.Parameters.Physic2DPushOrPullForce);
            }
            else
            {
                Debug.Log("水平移动速度" + _horizontalMovement);
                // 不在 推 或 拉 的时候，自己的移动速度不改变
                _playerController.SetHorizontalForce(_horizontalMovement * speed);
            }

        }
       
    }


    // 下面是动画参数的初始化和控制

    protected override void InitializeAnimatorParameter()
    {
        RegisterAnimatorParameter(_idleAniamtorParameterName, AnimatorControllerParameterType.Bool, out _idleAnimatorParameter);
        RegisterAnimatorParameter(_walkAniamtorParameterName, AnimatorControllerParameterType.Bool, out _walkAniamtorParameter);
        RegisterAnimatorParameter(_touchGroundAnimatorParameterName, AnimatorControllerParameterType.Bool,out _touchGroundAnimatorParameter);
        RegisterAnimatorParameter(_tryToTouchAnimatorParameterName, AnimatorControllerParameterType.Bool, out _tryToTouchAnimatorParameter);
    }

    public override void UpdateAnimator()
    {

        if (_movement.CurrentState == PlayerStates.MovementStates.Walking)
        {
            AnimatorHelper.UpdateAnimatorBool(_animator, _walkAniamtorParameter, true, _player._animatorParameters);
        }
        else
        {
            AnimatorHelper.UpdateAnimatorBool(_animator, _walkAniamtorParameter, false, _player._animatorParameters);
        }


        if (!_touch.CanTouch)
        {
            if (_movement.CurrentState == PlayerStates.MovementStates.Idle)
            {
                AnimatorHelper.UpdateAnimatorBool(_animator, _idleAnimatorParameter, true, _player._animatorParameters);
            }
            else
            {
                AnimatorHelper.UpdateAnimatorBool(_animator, _idleAnimatorParameter, false, _player._animatorParameters);
            }
        }
        
    }

}
