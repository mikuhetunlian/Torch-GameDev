using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ability�Ļ���
/// </summary>
public class PlayerAblity :MonoBehaviour
{

    ///�����Ƿ�����ʹ��
    public bool AbilityPermitted = true;


    protected PlayerController _playerController;
    protected Player _player;
    protected Animator _animator;
    protected StateMachine<PlayerStates.MovementStates> _movement;
    protected StateMachine<PlayerStates.PlayerConditions> _condition;
    protected InputManager _inputManager;
    protected Transform _transform;
    protected Touch _touch;
    protected Health _health;
    protected float _verticalInput;
    protected float _horizontalInput;


    protected virtual void Start()
    {
        GetComponents();
        Initialization();
    }
    /// <summary>
    /// ability ��ʼ�����ݺ�����һЩ����
    /// </summary>
    public virtual void Initialization()
    {
        
    }
    /// <summary>
    /// ability �����Ҫ��õ����
    /// </summary>
    public virtual void GetComponents()
    {
        _playerController = GetComponent<PlayerController>();
        _player = GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _transform = GetComponent<Transform>();
        _touch = GetComponent<Touch>();
        _health = GetComponent<Health>();
        _movement = _player.Movement;
        _condition = _player.Condition;
        _inputManager = _player.LinkedInputManager;
        if (_animator != null)
        {
            InitializeAnimatorParameter();
        }
    }


    /// <summary>
    /// abilityʹ�õĿ���
    /// </summary>
    /// <param name="abilityPermitted"></param>
    public virtual void PermitAbility(bool abilityPermitted)
    {
        AbilityPermitted = abilityPermitted;

    }



    public virtual void InternalHandleInput()
    {
        if (_inputManager == null) { return; }

        _horizontalInput = _inputManager.PrimaryMovement.x;
        _verticalInput = _inputManager.PrimaryMovement.y;
        HandleInput();
    }

    /// <summary>
    /// ��д�������������˶���ֵ�򰴼�״̬ �Ӷ������߼�
    /// </summary>
    public virtual void HandleInput()
    {
        
    }

    //ability�ĵ�һ��
    public virtual void EarlyProcessAblity()
    {
        //ÿ��ability�Ȼ�ȡinput��Ϣ�ٽ����߼�����
        InternalHandleInput();
    }

    //ability�ĵڶ���
    public virtual void ProcessAbility()
    {
        
    }

    //abilitty�ĵ�����
    public virtual void LateProcessAbility()
    {

    }


    /// <summary>
    /// �����µ� InputManager
    /// </summary>
    /// <param name="inputManager"></param>
    public virtual void SetInputManager(InputManager inputManager)
    {
        _inputManager = inputManager;
    }





    /// <summary>
    /// ��д�������������Լ�ability��Ҫ�����animator�Ĳ�����Player��_animatorParameters��
    /// �� awake �� GetComponents �л�����������
    /// </summary>
    protected virtual void InitializeAnimatorParameter()
    {
        
    }

    /// <summary>
    /// ���animator�д�������������Ͱ�����ӵ�Player�� _animatorParameters ��
    /// </summary>
    /// <param name="parameterName"></param>
    /// <param name="type"></param>
    protected virtual void RegisterAnimatorParameter(string parameterName, AnimatorControllerParameterType type,out int parameter)
    {
        parameter = Animator.StringToHash(parameterName);

        if (_animator == null)
        {
            return;
        }
        if (_animator.HasParameterOfType(parameterName, type))
        {
            _player._animatorParameters.Add(parameter);
        }
    }

    /// <summary>
    /// abilitys ��д������� ���ı�player���ϵ�animator�Ĳ�����ÿһִ֡��һ�Σ�����ÿһ��lateUpdate�����
    /// </summary>
    public virtual void UpdateAnimator()
    {
      
    }



}
