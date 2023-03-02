using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;




/// <summary>
/// �������������������gameplay�ĵط�����ţ�
/// </summary>
public class Player : MonoBehaviour
{

    //�趨��ʼ���泯��
    public enum FacingDirections { Left, Right }
    //��������泯�򣬻���ĳ����������泯����Ҫ�ı�
    public enum SpawnFacingDirections { Left, Right }
    public FacingDirections CurrentFaceingDir = FacingDirections.Right;
    public FacingDirections InitialFacingDir = FacingDirections.Right;
    public SpawnFacingDirections SpawnDir = SpawnFacingDirections.Right;
    ///����abilitys���ܿ���
    public bool DisableAllAbility;
    //player��abilitys
    protected List<PlayerAblity> abilitysList;
    //����player��״̬��������������ѣ�εȣ� �� player���ƶ�״̬
    public PlayerStates State;
    public StateMachine<PlayerStates.MovementStates> Movement;
    public StateMachine<PlayerStates.PlayerConditions> Condition;

    public Animator _animator;
    //�����洢animator�е� ������ ��int��ϣֵ
    public HashSet<int> _animatorParameters;

    protected PlayerAblity[] _playerAbilities;

    protected Rigidbody2D _rbody;
    public InputManager LinkedInputManager { get; protected set; }
    protected virtual void Awake()
    {
        Initialization();
        GetComponents();
    }

    //��ʼ��
    public void Initialization()
    {
        //��ʼ������״̬��
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

    //��FixUpdate��ִ�У���Զ���һЩ
    public void EveryFrame()
    {
        EarlyProcessAbilitys();
        ProcessAbiblitys();
        LateProcessAbilitys();
    }

    /// <summary>
    /// ���� abilitysList �����ϵ�����ability�е� EarlyProcessAbilitys
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
    /// ���� abilitysList �����ϵ�����ability�е�ProcessAbility
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
    /// ���� abilitysList �����ϵ�����ability�е�ProcessAbility
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
    /// ���� InputManager �����е� abilities��
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
        //����ҵ���aniamtor,�ͳ�ʼ��animator��Ĳ�����_animatorParametrs��
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
    /// ʹ player ��ָ���ĵص�ͳ�������
    /// </summary>
    /// <param name="spawnPoint"></param>
    /// <param name="facingDirections"></param>
    public void RespawnAt(Transform spawnPoint, FacingDirections facingDirections)
    {
        transform.position = spawnPoint.position;
        Debug.Log("����������λ��");
        SetFace(facingDirections);
    }


    /// <summary>
    /// ���� transform.localScale.x ������������ CurrentFaceingDir
    /// </summary>
    public void UpdateFaceDirection()
    {
        if (transform.localScale.x > 0)
        {
            CurrentFaceingDir = FacingDirections.Right;
        }
        else
        {
            CurrentFaceingDir = FacingDirections.Left;
        }
    }

    /// <summary>
    /// �ı�player���泯��
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
    /// ��͸��
    /// </summary>
    public void ToTransparency()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    /// <summary>
    /// �俴�ü�
    /// </summary>
    public void ToVisiable()
    {
        GetComponent<MeshRenderer>().enabled = true;
    }

}
