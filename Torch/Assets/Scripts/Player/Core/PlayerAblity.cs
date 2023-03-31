using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ability的基类
/// </summary>
public class PlayerAblity :MonoBehaviour
{

    ///能力是否被允许使用
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
    /// ability 初始化数据和其他一些开关
    /// </summary>
    public virtual void Initialization()
    {
        
    }

    /// <summary>
    /// ability 获得想要获得的组件
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
    /// 控制Ablity是否启用
    /// </summary>
    /// <param name="abilityPermitted"></param>
    public virtual void PermitAbility(bool abilityPermitted)
    {
        AbilityPermitted = abilityPermitted;
    }


    /// <summary>
    /// 在EarlyProcessAblity调用
    /// _horizontalInput代表水平键的值，_verticalInput代表竖直键的值
    /// _horizontalInput 和 _verticalInput 都从 _inputManager 中获取
    /// </summary>
    public virtual void InternalHandleInput()
    {
        if (_inputManager == null) { return; }

        _horizontalInput = _inputManager.PrimaryMovement.x;
        _verticalInput = _inputManager.PrimaryMovement.y;
        HandleInput();
    }

    /// <summary>
    /// 重写这个方法来获得运动轴值或按键状态 从而处理逻辑
    /// </summary>
    public virtual void HandleInput()
    {
        
    }

    /// <summary>
    /// 逻辑上的Ablity的第一段，可以类比为EarlyUpdate（虽然EarlyUpdate是不存在的）
    /// </summary>
    public virtual void EarlyProcessAblity()
    {
        //每个ability先获取input信息再进行逻辑处理
        InternalHandleInput();
    }

    /// <summary>
    /// 逻辑上的Ablity的第二段，可以类比为Update
    /// ablity逻辑要在 InternalHandleInput 后调用
    /// </summary>
    public virtual void ProcessAbility()
    {
        
    }

    /// <summary>
    /// 逻辑上的Ablity的第三段，可以类比为LateUpdate
    /// ablity逻辑要在 InternalHandleInput 后调用
    /// </summary>
    public virtual void LateProcessAbility()
    {

    }


    /// <summary>
    /// 设置新的 InputManager
    /// </summary>
    /// <param name="inputManager"></param>
    public virtual void SetInputManager(InputManager inputManager)
    {
        _inputManager = inputManager;
    }


    /// <summary>
    /// 重写这个方法，添加自己ability需要处理的animator的参数到Player的_animatorParameters中
    /// 在 awake 的 GetComponents 中会调用这个方法
    /// </summary>
    protected virtual void InitializeAnimatorParameter()
    {
        
    }




    /// <summary>
    /// 如果animator中存在这个参数，就把它添加到Player的 _animatorParameters 中
    /// </summary>
    /// <param name="parameterName">要更新的状态参数名</param>
    /// <param name="type">要更新的状态参数类型</param>
    /// <param name="parameter">传进一个参数，这个参数代表的是parameterName的哈希值，
    /// 目的是把string的参数转化为int参数，在动画状态机很大的情况下也能减少存储空间</param>
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
    /// abilitys 重写这个方法 来改变player身上的animator的参数，每一帧执行一次，会在每一次lateUpdate后调用
    /// </summary>
    public virtual void UpdateAnimator()
    {
      
    }



}
