using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PathMovementElement
{
    public Vector3 PathElementPotion;
    ///在该点延时的时间
    public float Delay;
}


public class PathMovement : MonoBehaviour
{
    public enum PossibleAccelerationType
    {
        ConstantSpeed,
        EaseOut,
        AnimationCurve
    }

    public enum CycleOptions
    {
        ///路径点不连起来，从起点走到终点再从终点走到起点，如此反复
        BackAndForth,
        ///路径点连成一个回路，一直绕着回路走
        Loop,
        ///路径不连起来，从起点走到终点，只走一次
        OnlyOnce
    }

    public enum MovementDirection
    {
        ///路径点下标 为 1,2,3...
        Ascending,
        ///路径点下标 为-1，-2，-3 ...
        Descending,
    }

    [Header("Path")]
    public CycleOptions CycleOption;
    public MovementDirection LoopInitiaMovementDirection = MovementDirection.Ascending;
    public List<PathMovementElement> PathElements;
    ///游戏开始时是否自动移动
    public bool isMoveAtStart;
    ///每个节点的移动判断都都有对应的情况判断
    public bool isConditionMove;
    ///当 距离差小于这个范围时，判断为达到了路径点
    public float MinDistanceToGoal = 0.1f;
    ///要移动的物体的初始transformposion
    public Vector3 _originalTransformPostion;
    ///是否拿到了初始transformposion
    protected bool _originalTransformPostionStatus = false;
    ///物体是否能够沿着路径移动
    public bool CanMove { get; set; }


    public float MovementSpeed = 1;
    ///返回当前平台的移动速度 主要提供给PlayerController使用
    public Vector3 CurrentSpeed { get; protected set; }
    public float DeltaTime;
    public bool EndReached { get { return _endReached; } }
    public PossibleAccelerationType AcclerationType = PossibleAccelerationType.ConstantSpeed;

    protected bool _active = false;
    protected IEnumerator<Vector3> _currnetPoint;
    protected int _direction = 1;
    protected Vector3 _initialPostion;
    protected Vector3 _initialPostionThisFrame;
    protected Vector3 _finalPostion;
    protected Vector3 _previousPoint = Vector3.zero;
    protected int _currentIndex;
    protected float _distanceToNextPoint;
    protected bool _endReached = false;
    //到达一个节点后要回调的函数
    protected UnityAction afterReachPointCallBack;


    protected virtual void Start()
    {
        Initialization();
        EventMgr.GetInstance().AddLinstener<bool>("ConditionMove", SetCondition);
    }

    protected virtual void Initialization()
    {
        _active = true;
        _endReached = false;

        if (this.gameObject.name.Equals("左门"))
        {
            Debug.Log("desu");
        }

        if (isMoveAtStart)
        {
            CanMove = true;
        }
        else 
        {
            CanMove = false;
        }

        if (PathElements == null || PathElements.Count < 1)
        {
            return;
        }

        if (LoopInitiaMovementDirection == MovementDirection.Ascending)
        {
            _direction = 1;
        }
        else
        {
            _direction = 1;
        }

        _initialPostion = this.transform.position;
        _currnetPoint = GetPathEnumerator();
        _previousPoint = _currnetPoint.Current;
        _currnetPoint.MoveNext();


        //初始化 要移动的物品的初始位置
        if (!_originalTransformPostionStatus)
        {
            _originalTransformPostionStatus = true;
            _originalTransformPostion = transform.position;
        }

        //定位到第一个点
        transform.position = _originalTransformPostion + _currnetPoint.Current;
    }

    /// <summary>
    /// 重置移动物体在path的开始点
    /// </summary>
    public virtual void ResetPath()
    {
        Initialization();
        
        transform.position = _originalTransformPostion + PathElements[0].PathElementPotion;
    }

    protected virtual void FixedUpdate()
    {
        if (PathElements == null || PathElements.Count < 1 ||  _endReached || !CanMove)
        {
            CurrentSpeed = Vector2.zero;
            return;
        }

        Move();
    }



    public void Move()
    {

            
            _initialPostion = transform.position;

            MoveAlongThePath();

            float distance = (transform.position - (_originalTransformPostion + _currnetPoint.Current)).magnitude;

            //如果到达了一个节点
            if (distance < MinDistanceToGoal)
            {
                _previousPoint = _currnetPoint.Current;

            if (afterReachPointCallBack != null)
            {
                afterReachPointCallBack();
            }
                
            //这里没写好，写写成这个样子，需要移动的就把isConditionMove设置为true好了
            if (isConditionMove)
                {
                  _currnetPoint.MoveNext();
                }
             
            }

            _finalPostion = transform.position;

            // v = x / t
            if (Time.deltaTime != 0)
            {

                CurrentSpeed = (_finalPostion - _initialPostion) / Time.deltaTime;
            }

        if (_endReached)
        {
            CurrentSpeed = Vector3.zero;
        }
        
    }

    public void SetCondition(bool state)
    {
        StartCoroutine(SetConditionCoroutine(state));
    }

    protected IEnumerator SetConditionCoroutine(bool state)
    {
        isConditionMove = state;
        yield return new  WaitForSeconds(0.5f);
        isConditionMove = !state;
    }


    /// <summary>
    /// 平台真正移动的地方
    /// 暂时没有写关于 AnimationCurve 的情况
    /// </summary>
    public virtual void MoveAlongThePath()
    {
        switch (AcclerationType)
        {
            case PossibleAccelerationType.ConstantSpeed:
                {
                    transform.position =  Vector3.MoveTowards(transform.position, _originalTransformPostion + _currnetPoint.Current, MovementSpeed * Time.deltaTime);
                    break;
                }
            case PossibleAccelerationType.EaseOut:
                {
                    transform.position = Vector3.Lerp(transform.position, _originalTransformPostion + _currnetPoint.Current, MovementSpeed * Time.deltaTime);
                    break;
                }
        }
        Physics2D.SyncTransforms();
    }

    /// <summary>
    /// 返回 能够不断更新当前路径点坐标的 迭代器
    /// </summary>
    /// <returns></returns>
    public IEnumerator<Vector3> GetPathEnumerator()
    { 

        if (PathElements == null || PathElements.Count < 1)
        {
            yield break;
        }

        int index = 0;
        _currentIndex = index;

        while (true)
        {
            _currentIndex = index;
            if (index < 0 || index > PathElements.Count - 1)
            {
                Debug.Log("yuejie");
            }
            yield return PathElements[index].PathElementPotion;

            if (PathElements.Count <= 1)
            {
                continue;
            }


            if (CycleOption == CycleOptions.BackAndForth)
            {
                if (index <= 0)
                {
                    _direction = 1;
                }
                else if (index >= PathElements.Count - 1)
                {
                    _direction = -1;
                }
                index += _direction;
            }

            if (CycleOption == CycleOptions.Loop)
            {
                index += _direction;
                if (index < 0)
                {
                    index = PathElements.Count - 1;
                }
                else if (index > PathElements.Count - 1)
                {
                    index = 0;
                }
            }

            if (CycleOption == CycleOptions.OnlyOnce)
            {
                if (index <= 0)
                {
                    _direction = 1;
                }
                else if (index >= PathElements.Count - 1)
                {
                    _direction = 0;
                    CurrentSpeed = Vector3.zero;
                    _endReached = true;
                }

                index += _direction;
            }
        }

    }


    /// <summary>
    /// 设置到达下一个节点后要做的事情
    /// </summary>
    /// <param name="callback"></param>
    public void SetReachPointCallBack(UnityAction callback)
    {
        afterReachPointCallBack = callback;
    }


    /// <summary>
    /// 给每一个点画gizmos
    /// </summary>
    private void OnDrawGizmos()
    {
        if (PathElements == null || PathElements.Count == 0)
        {
            return;
        }

        if (_originalTransformPostionStatus == false)
        {
            _originalTransformPostionStatus = true;
            _originalTransformPostion = transform.position;
        }

        if (transform.hasChanged && !_active)
        {
            _originalTransformPostion = transform.position;
        }

        for(int i =0;i<PathElements.Count;i++)
        {
            DebugHelper.DrawGizmoPoint(PathElements[i].PathElementPotion, 0.2f, Color.green);

            if (i >= 1 )
            {
                DebugHelper.DrawGizmoLine(_originalTransformPostion + PathElements[i-1].PathElementPotion, _originalTransformPostion + PathElements[i].PathElementPotion, Color.white);
            }

            if (i == PathElements.Count-1 && CycleOption == CycleOptions.Loop)
            {
                DebugHelper.DrawGizmoLine(_originalTransformPostion + PathElements[0].PathElementPotion, _originalTransformPostion + PathElements[i].PathElementPotion, Color.white);
            }
        }

    }


    public Vector3 GetOriginalTransformPostion()
    {
        return _originalTransformPostion;
    }

    public bool GetOriginalTransformPotionStatus()
    {
        return _originalTransformPostionStatus; 
    }

}
