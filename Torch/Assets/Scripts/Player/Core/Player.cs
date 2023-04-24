using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;




/// <summary>
/// Player类 的主要作用是用来管理playerAblity们
/// 玩家角色 面朝向的功能写在了Player中，HorizontalMovement 不包含改变面朝向的功能
/// </summary>
public class Player : MonoBehaviour
{

    //设定初始的面朝向
    public enum FacingDirections { Left, Right }
    //重生后的面朝向，或许某次重生后的面朝向需要改变
    public enum SpawnFacingDirections { Left, Right }
    public FacingDirections CurrentFaceingDir = FacingDirections.Right;
    public FacingDirections InitialFacingDir = FacingDirections.Right;
    public SpawnFacingDirections SpawnDir = SpawnFacingDirections.Right;
    ///所有abilitys的总开关
    public bool DisableAllAbility;
    //player的abilitys
    protected List<PlayerAblity> abilitysList;
    //包括player的状态（正常，冰冻，眩晕等） 和 player的移动状态
    public PlayerStates State;
    public StateMachine<PlayerStates.MovementStates> Movement;
    public StateMachine<PlayerStates.PlayerConditions> Condition;

    public Animator _animator;
    //用来存储animator中的 参数名 的int哈希值
    public HashSet<int> _animatorParameters;

    protected PlayerAblity[] _playerAbilities;

    protected Rigidbody2D _rbody;
    public InputManager LinkedInputManager { get; protected set; }
    protected virtual void Awake()
    {
        Initialization();
        GetComponents();
    }

    //初始化
    public void Initialization()
    {
        //初始化两个状态机
        DisableAllAbility = false;
        Movement = new StateMachine<PlayerStates.MovementStates>(this.gameObject, false);
        Condition = new StateMachine<PlayerStates.PlayerConditions>(this.gameObject, false);

    }

    public void GetComponents()
    {
        GetAnimator();
        SetInputManager();
        _playerAbilities = GetComponents<PlayerAblity>();
        _rbody = GetComponent<Rigidbody2D>();
        abilitysList = new List<PlayerAblity>();
        _animatorParameters = new HashSet<int>();
        PlayerAblity[] abilitys = this.gameObject.GetComponents<PlayerAblity>();
        for (int i = 0; i < abilitys.Length; i++)
        {
            abilitysList.Add(abilitys[i]);
        }

    }



    public void Update()
    {
        UpdateFaceDirection();
        EveryFrame();
    }

    // 在FixUpdate中执行，相对独立一些
    public void EveryFrame()
    {
        EarlyProcessAbilitys();
        ProcessAbiblitys();
        LateProcessAbilitys();
    }

    /// <summary>
    /// 遍历 abilitysList 来不断地运行ability中的 EarlyProcessAbilitys
    /// <summary>
    protected void EarlyProcessAbilitys()
    {
        foreach (PlayerAblity ability in abilitysList)
        {
            if (ability.AbilityPermitted && !DisableAllAbility)
            {
                ability.EarlyProcessAblity();
            }

        }
    }


    /// <summary>
    /// 遍历 abilitysList 来不断地运行ability中的ProcessAbility
    /// </summary>
    protected void ProcessAbiblitys()
    {
        foreach (PlayerAblity ability in abilitysList)
        {
            if (ability.AbilityPermitted && !DisableAllAbility)
            {
                ability.ProcessAbility();
                ability.UpdateAnimator();
            }

        }
    }

    /// <summary>
    /// 遍历 abilitysList 来不断地运行ability中的ProcessAbility
    /// </summary>
    protected void LateProcessAbilitys()
    {
        foreach (PlayerAblity ability in abilitysList)
        {
            if (ability.AbilityPermitted && !DisableAllAbility)
            {
                ability.LateProcessAbility();
            }
        }
    }



    public virtual void SetInputManager()
    {

        LinkedInputManager = InputManager.GetInstance();
        //UpDateInputManagerInAbilities();
    }

    /// <summary>
    /// 更新 InputManager 到所有的 abilities中
    /// </summary>
    public virtual void UpDateInputManagerInAbilities()
    {
        if (_playerAbilities == null)
        {
            return;
        }
        for (int i = 0; i < _playerAbilities.Length; i++)
        {
            _playerAbilities[i].SetInputManager(LinkedInputManager);
        }
    }


    public virtual void GetAnimator()
    {
        _animator = GetComponent<Animator>();
        //如果找到了aniamtor,就初始化animator里的参数到_animatorParametrs中
        if (_animator != null)
        {
            InitializeAnimatorParameters();
        }
    }


    protected virtual void InitializeAnimatorParameters()
    {
        //if (_animator == null)
        //{
        //    return;
        //}

        //_animatorParameters = new HashSet<int>();

        //AnimatorHelper.AddAnimatorParamaterIfExists(_animator, "isWalk", AnimatorControllerParameterType.Bool, );
    }


    /// <summary>
    /// 使 player 在指定的地点和朝向重生
    /// </summary>
    /// <param name="spawnPoint"></param>
    /// <param name="facingDirections"></param>
    public void RespawnAt(Transform spawnPoint, FacingDirections facingDirections)
    {
        transform.position = spawnPoint.position;
        Debug.Log("重生设置了位置");
        SetFace(facingDirections);
    }



    /// <summary>
    /// 设置转向和状态
    /// </summary>
    public void UpdateFaceDirection()
    {

        if (LinkedInputManager.PrimaryMovement.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            CurrentFaceingDir = FacingDirections.Right;
        }
        if (LinkedInputManager.PrimaryMovement.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            CurrentFaceingDir = FacingDirections.Left;
        }

    }

    /// <summary>
    /// 改变player的面朝向
    /// </summary>
    /// <param name="facingDirections"></param>
    public void SetFace(FacingDirections facingDirections)
    {
        short face;
        if (facingDirections == FacingDirections.Right)
        {
            face = 1;
        }
        else
        {
            face = -1;
        }
        transform.localScale = new Vector3(face * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }



  

    /// <summary>
    /// 变透明
    /// </summary>
    public void ToTransparency()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    /// <summary>
    /// 变看得见
    /// </summary>
    public void ToVisiable()
    {
        GetComponent<MeshRenderer>().enabled = true;
    }

}
