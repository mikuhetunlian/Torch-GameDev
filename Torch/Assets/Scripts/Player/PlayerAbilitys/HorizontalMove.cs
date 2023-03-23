using UnityEngine;

public class HorizontalMove : PlayerAblity
{


    public Player.FacingDirections facingDir = Player.FacingDirections.Right;
    public float speed;
    public bool move;
    private Rigidbody2D rbody;
    [SerializeField]
    private Vector2 _newPostion;

    // ˮƽ�ƶ��ķ���ÿһ֡�� _horizontalInput ��ȡ
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
    /// ����input������ �ı� newPostion �� x����ֵ
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
                && _movement.CurrentState != PlayerStates.MovementStates.LadderClimbing //����������
                && _movement.CurrentState != PlayerStates.MovementStates.Jumping //������Ծ
               )
            {
                // ��������ˮƽ�ƶ����ı� PlayerStates
                if (_horizontalMovement != 0)
                {
                    _movement.ChangeState(PlayerStates.MovementStates.Walking);
                }
                else // ���ˮƽ�ٶ�Ϊ0���ı� PlayerStates
                {
                    _movement.ChangeState(PlayerStates.MovementStates.Idle);
                       
                }
            }

            //��  �� �� �� ��ʱ���Լ����ƶ��ٶ�Ҫ���ͺ�pushable����ƥ��
            if (_playerController.State.IsControlingRight ||_playerController.State.IsControlingLeft)
            {
                _playerController.SetHorizontalForce(_horizontalMovement * _playerController.Parameters.Physic2DPushOrPullForce);
            }
            else
            {
                Debug.Log("ˮƽ�ƶ��ٶ�" + _horizontalMovement);
                // ���� �� �� �� ��ʱ���Լ����ƶ��ٶȲ��ı�
                _playerController.SetHorizontalForce(_horizontalMovement * speed);
            }

        }
       
    }


    // �����Ƕ��������ĳ�ʼ���Ϳ���

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
