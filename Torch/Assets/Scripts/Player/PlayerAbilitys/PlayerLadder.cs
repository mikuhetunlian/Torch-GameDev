using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLadder : PlayerAblity
{

    ///�����ӵ��ٶ�
    public float LadderClimbingSpeed;
    public bool LadderCollider { get { return _colliders.Count > 0; } }
    ///���������Ϸ�ʱ�Ƿ��Զ���λ���Ϸ�
    public bool ForceAnchorToTopOnExit = true;
    ///װĿǰ������������
    protected List<Collider2D> _colliders;
    ///Ŀǰ�����ڽӴ�������
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
    /// ���㵱ǰ����������������һ��
    /// ����ɶ������ƴ�ӳ�һ�����ӣ�������жϻ������
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
    /// �������� ��ladder����Ϊ�ĵط�
    /// </summary>
    protected void HandleLadderClimbing()
    {
        if (!AbilityPermitted)
        {
            return;
        }

        //������ڽӴ�ladder
        if (LadderCollider)
        {
            //����������� �� �պ���������Ļ�
            if (_movement.CurrentState == PlayerStates.MovementStates.LadderClimbing
                && _playerController.State.IsGrounded
                && !_playerController.IsGravityActive)
            {
                GetOffTheLadder();
            }

            //������������ӵ�״̬����������ȥ
            if (_movement.CurrentState != PlayerStates.MovementStates.LadderClimbing
                && !AboveLadderPlatform())
            {
                if (_verticalInput > 0)
                {
                    StartClimbing();
                }
            }

            //����������ӵ�״̬
            if (_movement.CurrentState == PlayerStates.MovementStates.LadderClimbing)
            {
                Climbing();
            }

            //����������Ҹոյ�ladderPlatformʱ���Ƿ�������λ��
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
            
            //���������ƽ̨�ϣ���������ȥ
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
    /// ��ʼ����������
    /// </summary>
    protected void StartClimbing()
    {
        _movement.ChangeState(PlayerStates.MovementStates.LadderClimbing);
        //�ر�����
        _playerController.GravityActive(false);

        //����������������м�λ�ã�������һ������
        if (_currentLadder.CenterOnLadder)
        { 
            _transform.position = new Vector3(_currentLadder.transform.position.x, _transform.position.y, _transform.position.z);
        }
    }

    /// <summary>
    /// ��ʼ����������
    /// </summary>
    protected void StartClimbingDown()
    {
        _movement.ChangeState(PlayerStates.MovementStates.LadderClimbing);
        _playerController.GravityActive(false);
        _playerController.CollisionOff();

        //����������������м�λ�ã�������һ������
        if (_currentLadder.CenterOnLadder)
        {
            _transform.position = new Vector3(_currentLadder.transform.position.x, _transform.position.y, _transform.position.z);
        }

    }

    /// <summary>
    /// ����������ʱ���߼�
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
    /// �뿪ladderҪ������ 
    /// ����������ability����
    /// </summary>
    public void GetOffTheLadder()
    {
        _movement.ChangeState(PlayerStates.MovementStates.Idle);
        _playerController.GravityActive(true);
        _playerController.CollisonOn();
    }


    /// <summary>
    /// �����ж�player�Ƿ�վ����ladderPlatform��
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
    ///  �����ж�player�Ƿ�����ladderPlatform�·�
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
    /// ����������� ladder �� collider2D
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
    /// �Ƴ��뿪ʱ�� ladder �� collider2D
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
