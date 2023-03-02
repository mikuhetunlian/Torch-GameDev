using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���߳���ͬʱҲ��hitInfos��key
public enum RaycastDirection { Up, Down, Left, Right }


/// <summary>
/// ������������������߼������������С���Ƶģ�Ŀǰ��û������������صĹ��ܣ�
/// </summary>
public class PlayerController : MonoBehaviour
{


    //ÿ�������������
    public int RaycastNum;

    public Player _player;
    public PlayerControllerState State;
    //����player��һЩ����
    public PlayerControllerParameters Parameters;
    //boxCollider2D
    public BoxCollider2D Collider;

    public Dictionary<RaycastDirection, List<RaycastHit2D>> hitInfos = new Dictionary<RaycastDirection, List<RaycastHit2D>>();
    public RaycastHit2D fallHitInfo;


    public Player.FacingDirections facingDir = Player.FacingDirections.Right;
    public Vector2 Speed { get; protected set; }
    public bool IsGravityActive { get { return _gravityActive; } }

    [Header("CollisonLayerMask")]
    public LayerMask PlatformMask;
    public LayerMask MovingPlatformMask;
    public LayerMask OneWayPlatformMask;



    [Header("Ray")]
    ///�Ƿ񻭳�����
    public bool isDrawRay;
    public short NumberOfHorizontalRays = 8;
    public short NumberOfVerticalRays = 8;
    ///����߽�һС�ε�offset
    public float rayOffset;

    public float RayOffsetVertical;
    public float RayOffsetHorizontal;

    public GameObject StandingOn;
    public Collider2D StandingOnCollider;
    //����push��pull������
    public GameObject ControlAbleObject;
    public RaycastHit2D LeftHitInfo;
    public RaycastHit2D RighthHitInfo;

    ///ʱ�̷�Ӧ�����·�����������˭
    public GameObject BelowColliderGameobject;

    ///boxCollider��x��һ��
    private float _extentX;
    ///boxCollider��y��һ��
    private float _extentY;

    ///�����Ϊ����㼶������
    private string _groundCheckLayerName;


    /// speed �� private ����
    protected Vector2 _speed;
    protected Vector2 _externalForce;
    protected float _currentGravity;
    protected float _movingPlatformCurrnetGravity;
    protected const float _movingPlatformGrayvity = -500;
    protected bool _gravityActive = true;
    public Vector2 _newPostion;
    protected Transform _transform;
    protected RaycastHit2D[] _rightHitStorage;
    protected RaycastHit2D[] _leftHitStorage;
    protected RaycastHit2D[] _aboveHitStorage;
    protected RaycastHit2D[] _belowHitStorage;
    protected Vector2 _verticalRayCastFromLeft;
    protected Vector2 _verticalRayCastToRight;
    protected Vector2 _horizontalRayCastFromDown;
    protected Vector2 _horizontalRayCastToUp;
    protected InputManager _inputManager;
    protected LayerMask _raysBelowLayerMaskPlatforms;
    protected LayerMask _rayBelowLayerMaskPlatformWithoutOneWay;
    protected LayerMask _platformMaskSave;
    protected PathMovement _movingPlatformTest;
    protected PathMovement _movingPlatform;


    protected const float _smallValue = 0.0001f;
    protected const float _movingPlatformGraviy = -500;   // ??? ���û��



    private void Awake()
    {
        Physics2D.queriesStartInColliders = false;
        GameObject.DontDestroyOnLoad(this.gameObject);
        SetRaycastParameter();

    }


    private void Start()
    {
        GetComponents();
        Initialization();
    }
    //��ʼ������
    public void Initialization()
    {
        //��Inputģ��
        InputMgr.GetInstance().InputEnable(true);
        //��ʼlevelMgr
        LevelManager.GetInstance();
        Parameters = new PlayerControllerParameters();
        State = new PlayerControllerState();
        State.Reset();
        rayOffset = 0.1f;

        RayOffsetVertical = 0.05f;
        RayOffsetHorizontal = 0.1f;

        RaycastNum = 5;
        isDrawRay = true;
        _extentX = Collider.bounds.extents.x;
        _extentY = Collider.bounds.extents.y;
        _groundCheckLayerName = "mid1";
        _aboveHitStorage = new RaycastHit2D[NumberOfVerticalRays];
        _belowHitStorage = new RaycastHit2D[NumberOfVerticalRays];
        _leftHitStorage = new RaycastHit2D[NumberOfHorizontalRays];
        _rightHitStorage = new RaycastHit2D[NumberOfHorizontalRays];
        _inputManager = _player.LinkedInputManager;
        _platformMaskSave = PlatformMask;

        CachePlatformMask();
    }

    public void GetComponents()
    {
        Collider = GetComponent<BoxCollider2D>();
        _transform = GetComponent<Transform>();
        _player = GetComponent<Player>();
    }



    public void AddForce(Vector2 force)
    {
        _speed += force;
        _externalForce += force;
    }

    public void AddHorizontalForce(float x)
    {
        _speed.x += x;
        _externalForce.x += x;
    }

    public void AddVertivalForce(float y)
    {
        _speed.y += y;
        _externalForce.y += y;
    }

    public void SetForce(Vector2 force)
    {
        _speed = force;
        _externalForce = force;
    }

    public void SetHorizontalForce(float x)
    {
        _speed.x = x;
        _externalForce.x = x;
    }

    public void SetVerticalForce(float y)
    {
        _speed.y = y;
        _externalForce.y = y;
    }


    private void FixedUpdate()
    {
        EveryFrame();


    }


    public void EveryFrame()
    {
        if (Time.timeScale == 0f)
        {
            return;
        }


     

        ApplyGravity();
        FrameInitialization();

        HandleMovingPlatforms();

        DetectControlAble();

        CastRayToLeft();
       
        CastRayToRight();
       
        CastRayAbove();
        
        CastRayBelow();

        ComputeNewSpeed();


        if (State.IsOnMovingPlatform)
        {
            _newPostion.y = 0;
            _newPostion.y = 0;
        }

        _transform.Translate(_newPostion, Space.Self);

        _externalForce.x = 0;
        _externalForce.y = 0;
        
        
        SetSate(); 
    }

    public void ApplyGravity()
    {
        _currentGravity = Parameters.Gravity;
        if (_gravityActive)
        {
            _speed.y += (_currentGravity + _movingPlatformCurrnetGravity) * Time.deltaTime;
        }
    }


    /// <summary>
    /// ���վ���� onewayPlatfrom �� ����player ���ٶ�
    /// </summary>
    public void HandleMovingPlatforms()
    {
        if (_movingPlatform != null)
        {

            //��һ��ע�Ͳ�Ҫɾ
            if (!float.IsNaN(_movingPlatform.CurrentSpeed.x) && !float.IsNaN(_movingPlatform.CurrentSpeed.y) && !float.IsNaN(_movingPlatform.CurrentSpeed.z))
            {
                _transform.Translate(_movingPlatform.CurrentSpeed * Time.deltaTime);
            }



            if ((Time.timeScale == 0) || float.IsNaN(_movingPlatform.CurrentSpeed.x) || float.IsNaN(_movingPlatform.CurrentSpeed.y) || float.IsNaN(_movingPlatform.CurrentSpeed.z))
            {
                return;
            }

            if ((Time.deltaTime <= 0))
            {
                return;
            }

 
            GravityActive(false);

            State.IsOnMovingPlatform = true;

            _movingPlatformCurrnetGravity = _movingPlatformGraviy;

            _newPostion.y = _movingPlatform.CurrentSpeed.y * Time.deltaTime;
            _speed = -_newPostion / Time.deltaTime;
            _speed.x = -_speed.x;


        }
    }


    public void DetachFromMovinPlatform()
    {
        if (_movingPlatform == null)
        {
            return;
        }

        this.transform.SetParent(null);
        GravityActive(true);
        State.IsOnMovingPlatform = false;
        _movingPlatform = null;
        _movingPlatformCurrnetGravity = 0;
    }

    public void CastRayToLeft()
    {
        float leftRayLength = Mathf.Abs(_speed.x * Time.deltaTime) + Collider.bounds.extents.x  + RayOffsetHorizontal*2;

        _horizontalRayCastFromDown = Collider.bounds.center - new Vector3(0, _extentY);
        _horizontalRayCastToUp = Collider.bounds.center + new Vector3(0, _extentY);
        _horizontalRayCastFromDown = _horizontalRayCastFromDown + (Vector2)transform.up * 0.05f;
        _horizontalRayCastToUp = _horizontalRayCastToUp - (Vector2)transform.up * 0.05f;

        float leftSamllestDistance = float.MaxValue;
        int leftSmallestIndex = 0;
        bool leftHitConnect = false;
        Vector2 originPoint = Vector2.zero;
        for (int i = 0; i < NumberOfHorizontalRays; i++)
        {
            originPoint = Vector2.Lerp(_horizontalRayCastFromDown, _horizontalRayCastToUp, (float)i / (NumberOfHorizontalRays - 1));

            _leftHitStorage[i] = DebugHelper.RaycastAndDrawLine(originPoint, Vector2.left, leftRayLength, PlatformMask);
            if (_leftHitStorage[i].collider != null)
            {
                float distance = Mathf.Abs(_leftHitStorage[i].point.x - _horizontalRayCastFromDown.x);
                if (distance < leftSamllestDistance)
                {
                    leftSmallestIndex = i;
                    leftSamllestDistance = distance;
                }
                leftHitConnect = true;
            }

        }



        if (leftHitConnect)
        {

            State.isCollidingLeft = true;
            LeftHitInfo = _leftHitStorage[leftSmallestIndex];
            if (_inputManager.PrimaryMovement.x < 0)
            {
                float distance = Mathf.Abs(transform.position.x - _extentX - _leftHitStorage[leftSmallestIndex].point.x);

                //float distance = Mathf.Abs(Collider.bounds.center.x - _leftHitStorage[leftSmallestIndex].point.x);

                //float distance = Mathf.Abs(_leftHitStorage[leftSmallestIndex].point.x - _horizontalRayCastFromDown.x);


                //��� ���� ������ǽ�ڼ��
                if (State.IsControlingRight &&
                    _inputManager.PrimaryMovement.x < 0 &&
                    _inputManager.ControlButton.State.CurrentState == InputHelper.ButtonState.ButtonPressed
                    && ControlAbleObject.GetComponent<Rigidbody2D>().velocity.x > 0.5f)
                {
                    return;
                }

                _newPostion.x = RayOffsetHorizontal - distance;
     
            }

        }
        else
        {
            State.isCollidingLeft = false;
        }

    }

    public void CastRayToRight()
    {

        float rightRayLength = Mathf.Abs(_speed.x * Time.deltaTime) + Collider.bounds.extents.x + +RayOffsetHorizontal;


        _horizontalRayCastFromDown = Collider.bounds.center - new Vector3(0, _extentY);
        _horizontalRayCastToUp = Collider.bounds.center + new Vector3(0, _extentY);
        _horizontalRayCastFromDown = _horizontalRayCastFromDown + (Vector2)transform.up *0.05f;
        _horizontalRayCastToUp = _horizontalRayCastToUp - (Vector2)transform.up * 0.05f;

        float rightSmallestDistance = float.MaxValue;
        int rightSmallestIndex = 0;
        bool rightHitConnect = false;
        Vector2 originPoint = Vector2.zero;
        for (int i = 0; i < NumberOfHorizontalRays; i++)
        {
            originPoint = Vector2.Lerp(_horizontalRayCastFromDown, _horizontalRayCastToUp, (float)i / (NumberOfHorizontalRays - 1));

            _rightHitStorage[i] = DebugHelper.RaycastAndDrawLine(originPoint, Vector2.right, rightRayLength, PlatformMask);
            if (_rightHitStorage[i].collider != null)
            {
                float distance = Mathf.Abs(_rightHitStorage[i].point.x - _horizontalRayCastFromDown.x);
                if (distance < rightSmallestDistance)
                {
                    rightSmallestIndex = i;
                    rightSmallestDistance = distance;
                }
                rightHitConnect = true;

            }
        }



        if (rightHitConnect)
        {
            State.isCollidingRight = true;
            RighthHitInfo = _rightHitStorage[rightSmallestIndex];
            if (_inputManager.PrimaryMovement.x > 0)
            {
                float distance = Mathf.Abs(_rightHitStorage[rightSmallestIndex].point.x - (transform.position.x + _extentX));

                //float distance = Mathf.Abs(_rightHitStorage[rightSmallestIndex].point.x - Collider.bounds.center.x);

                //float distance = Mathf.Abs(_rightHitStorage[rightSmallestIndex].point.x - _horizontalRayCastFromDown.x);

                //��� ���� ������ǽ�ڼ��
                if (State.IsControlingLeft &&
                    _inputManager.PrimaryMovement.x > 0 &&
                    _inputManager.ControlButton.State.CurrentState == InputHelper.ButtonState.ButtonPressed &&
                     ControlAbleObject.GetComponent<Rigidbody2D>().velocity.x <  -0.5f)
                {
                    return;
                }

             
                _newPostion.x = -(RayOffsetHorizontal - distance);
            }
        }
        else
        {
            State.isCollidingRight = false;
        }

    }

    public void CastRayBelow()
    {
        if (_newPostion.y < 0)
        {
            State.IsFalling = true;
        }
        else
        {
            State.IsFalling = false;
        }

        float rayLength = _extentY + RayOffsetVertical;

        if (State.IsOnMovingPlatform)
        {
            rayLength *= 2;
        }

        //����������䣬�ӳ����¼������߳���
        if (_newPostion.y < 0)
        {
            rayLength += Mathf.Abs(_newPostion.y);
        }

        if (_belowHitStorage.Length != NumberOfVerticalRays)
        {
            _belowHitStorage = new RaycastHit2D[NumberOfVerticalRays];
        }

        if (State.IsOnMovingPlatform && (_newPostion.y > 0))
        {
            _raysBelowLayerMaskPlatforms = _raysBelowLayerMaskPlatforms & ~LayerMgr.OneWayPlatformLayerMask;
        }



        float smallestDistance = float.MaxValue;
        int smallestDistanceIndex = 0;
        bool hitConnected = false;

        _verticalRayCastFromLeft = Collider.bounds.center - new Vector3(_extentX, 0);
        _verticalRayCastToRight = Collider.bounds.center + new Vector3(_extentX, 0);
        _verticalRayCastFromLeft += (Vector2)transform.up * RayOffsetVertical;
        _verticalRayCastToRight += (Vector2)transform.up * RayOffsetVertical;
        _verticalRayCastFromLeft += (Vector2)transform.right * _newPostion.x;
        _verticalRayCastToRight += (Vector2)transform.right * _newPostion.x;


        Vector3 _boundBottom = Collider.bounds.center - new Vector3(0, _extentY);

        StandingOn = null;
        StandingOnCollider = null;
        for (int i = 0; i < NumberOfVerticalRays; i++)
        {
            Vector2 originPoint = Vector2.Lerp(_verticalRayCastFromLeft, _verticalRayCastToRight, ((float)i / (NumberOfHorizontalRays - 1)));

            _raysBelowLayerMaskPlatforms = PlatformMask;
            _rayBelowLayerMaskPlatformWithoutOneWay = PlatformMask & ~(OneWayPlatformMask);

          
           
            _belowHitStorage[i] = DebugHelper.RaycastAndDrawLine(originPoint, Vector2.down, rayLength, _raysBelowLayerMaskPlatforms);
            
           
            float distance = Mathf.Abs(_verticalRayCastFromLeft.y - _belowHitStorage[i].point.y);

            if (distance < 0.05f)
            {
                break;
            }

            if (_belowHitStorage[i].collider != null)
            {
                hitConnected = true;

                if (distance < smallestDistance)
                {
                    smallestDistance = distance;
                    smallestDistanceIndex = i;
                }
            }
        }

        if (hitConnected)
        {
            StandingOn = _belowHitStorage[smallestDistanceIndex].collider.gameObject;
            StandingOnCollider = _belowHitStorage[smallestDistanceIndex].collider;

            if (smallestDistance < 1f && StandingOn.layer == LayerMgr.OneWayPlatformLayer && _speed.y >0)
            {
                StandingOn = null;
                StandingOnCollider = null;
                State.isCollidingBelow = false;
                return;
            }



            State.IsFalling = false;
            State.isCollidingBelow = true;
         

            //������������� ���� ��Ծ ���Ǿ�ֱ��Ӧ����������������ٶ�
            if (_externalForce.y > 0 && _speed.y > 0)
            {
                _newPostion.y = _speed.y * Time.deltaTime;
                State.isCollidingBelow = false;
            }
            else
            {
                //float distance = Mathf.Abs(_belowHitStorage[smallestDistanceIndex].point.y - transform.position.y);
                //float distance = Mathf.Abs(Collider.bounds.center.y - _belowHitStorage[smallestDistanceIndex].point.y);

                float distance = Mathf.Abs(_verticalRayCastFromLeft.y - _belowHitStorage[smallestDistanceIndex].point.y);

                _newPostion.y = -distance + Collider.bounds.extents.y + RayOffsetVertical;

           
            }


            if (Mathf.Abs(_newPostion.y) < _smallValue)
            {
                _newPostion.y = 0;
            }



            ////����Ƿ�վ���� oneWayMovingPlatform ��
            PathMovement _movingPlatformTest = _belowHitStorage[smallestDistanceIndex].collider.GetComponent<PathMovement>();
            if (_movingPlatformTest != null && State.IsGrounded)
            {
                _movingPlatform = _movingPlatformTest.GetComponent<PathMovement>();
            }
            else
            {
                
                DetachFromMovinPlatform();
            }

        }
        else
        {
            State.isCollidingBelow = false;
            if (State.IsOnMovingPlatform)
            {
                DetachFromMovinPlatform();
            }
        }


    }

    public void CastRayAbove()
    {


        _verticalRayCastFromLeft = Collider.bounds.center - new Vector3(_extentX, 0);
        _verticalRayCastToRight = Collider.bounds.center + new Vector3(_extentX, 0);

        float rayLength = _extentY + RayOffsetVertical;


        float smallestDistance = float.MaxValue;
        float smallIndex = 0;
        bool hitConnected = false;

        for (int i = 0; i < NumberOfVerticalRays; i++)
        {
            Vector2 originPoint = Vector2.Lerp(_verticalRayCastFromLeft, _verticalRayCastToRight, (float)i / (NumberOfHorizontalRays - 1));
            _aboveHitStorage[i] = DebugHelper.RaycastAndDrawLine(originPoint, Vector2.up, rayLength, PlatformMask & ~(OneWayPlatformMask));

            if (_aboveHitStorage[i].collider != null)
            {
                hitConnected = true;
                float distance = Mathf.Abs(_aboveHitStorage[i].point.y - _verticalRayCastFromLeft.y);
                if (Mathf.Abs(_aboveHitStorage[i].point.y - _verticalRayCastFromLeft.y) < smallestDistance)
                {
                    smallestDistance = distance;
                    smallIndex = i;
                }
            }

        }


        if (hitConnected)
        {
            State.isCollidingAbove = true;

            if (_newPostion.y > 0)
            {
                _newPostion.y = 0;
            }

        }
    }

    //�������һ����û�нӴ���controlable������
    public void DetectControlAble()
    {
        float rayLength = Collider.bounds.extents.x + RayOffsetHorizontal;

        Vector2 origin = Collider.bounds.center;
        RaycastHit2D hitInfoleft = DebugHelper.RaycastAndDrawLine(origin, Vector2.left, rayLength * 2f, 1 << LayerMask.NameToLayer("Pushables"));
      
        RaycastHit2D hitInfoRight = DebugHelper.RaycastAndDrawLine(origin, Vector2.right, rayLength * 2f, 1 << LayerMask.NameToLayer("Pushables"));

        if (hitInfoleft.collider != null || hitInfoRight.collider != null)
        {
            State.isDetectControlableObject = true;

            ControlAbleObject = hitInfoRight.collider != null ? hitInfoRight.collider.gameObject : hitInfoleft.collider.gameObject;
        }
        else
        {
            State.isDetectControlableObject = false;
            ControlAbleObject = null;
        }
    }
    public void SetSate()
    {
        //if (State.IsGrounded  && _inputManager.PrimaryMovement.x == 0)
        //{
        //    _player.Movement.ChangeState(PlayerStates.MovementStates.Idle);
        //}
    }

    /// <summary>
    /// �������� �� �ر����� �Ŀ���
    /// </summary>
    /// <param name="state"></param>
    public void GravityActive(bool state)
    {
        _gravityActive = state;
    }

    /// <summary>
    /// �ڳ�ʼ����ʱ�򱣴�һ���ʼ��platformMask
    /// </summary>
    public void CachePlatformMask()
    {
        _platformMaskSave = PlatformMask;
    }

    /// <summary>
    /// ������������ײ���
    /// </summary>
    public void CollisonOn()
    {
        PlatformMask = _platformMaskSave;
        PlatformMask |= OneWayPlatformMask;

    }

    /// <summary>
    /// �ر�������ײ���
    /// </summary>
    public void CollisionOff()
    {
        PlatformMask = 0;
    }

    public void FrameInitialization()
    {
        
        //����һ֡�Ŀ�ʼ ���� ����ability���úõ� _speed ������_newPostion
        _newPostion = _speed * Time.deltaTime;
        State.WasGroundedLastFrame = State.isCollidingBelow;
        
    }


    public void ComputeNewSpeed()
    {
        if(Time.deltaTime > 0)
        {
            _speed = _newPostion / Time.deltaTime;
        }
    }

    /// <summary>
    /// �������߼������Ҫ�Ķ���
    /// </summary>
    private void SetRaycastParameter()
    {
        hitInfos.Add(RaycastDirection.Up, new List<RaycastHit2D>());
        hitInfos.Add(RaycastDirection.Down, new List<RaycastHit2D>());
        hitInfos.Add(RaycastDirection.Left, new List<RaycastHit2D>());
        hitInfos.Add(RaycastDirection.Right, new List<RaycastHit2D>());

        for (int i = 1; i <= RaycastNum; i++)
        {
            hitInfos[RaycastDirection.Up].Add(new RaycastHit2D());
            hitInfos[RaycastDirection.Right].Add(new RaycastHit2D());
            hitInfos[RaycastDirection.Left].Add(new RaycastHit2D());
            hitInfos[RaycastDirection.Down].Add(new RaycastHit2D());
        }
    }

    /// <summary>
    /// ��λ��˲�����ϵ�������
    /// </summary>
    public void AnchorToGround()
    {
        
            float distanceToGround = this.transform.position.y - _belowHitStorage[0].point.y;
            if (distanceToGround > 0 && distanceToGround <1)
            {
                SetForce(Vector2.zero);
                this.transform.position = new Vector3(transform.position.x, _belowHitStorage[0].point.y, transform.position.z);
                Debug.Log("˲��������");
            }
    }

    /// <summary>
    /// ���ص�����ľ���
    /// </summary>
    /// <returns>���û�м�⵽������-1</returns>
    public float DistanceToGround()
    {
        RaycastHit2D hitInfo =  Physics2D.Raycast(_transform.position, Vector2.down,1,PlatformMask);

        if (hitInfo.collider != null)
        {
            float distance = _transform.position.y - hitInfo.point.y;
            return distance;
        }
        else
        {
            return -1;
        }
    }


    /// <summary>
    /// ������ת
    /// </summary>
    public void ResetRotation()
    {
        transform.rotation = Quaternion.Euler(0,0,0);
    }
}
