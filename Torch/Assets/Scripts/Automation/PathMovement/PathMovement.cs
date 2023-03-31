using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PathMovementElement
{
    public Vector3 PathElementPotion;
    ///�ڸõ���ʱ��ʱ��
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
        ///·���㲻��������������ߵ��յ��ٴ��յ��ߵ���㣬��˷���
        BackAndForth,
        ///·��������һ����·��һֱ���Ż�·��
        Loop,
        ///·������������������ߵ��յ㣬ֻ��һ��
        OnlyOnce
    }

    public enum MovementDirection
    {
        ///·�����±� Ϊ 1,2,3...
        Ascending,
        ///·�����±� Ϊ-1��-2��-3 ...
        Descending,
    }

    [Header("Path")]
    public CycleOptions CycleOption;
    public MovementDirection LoopInitiaMovementDirection = MovementDirection.Ascending;
    public List<PathMovementElement> PathElements;
    ///��Ϸ��ʼʱ�Ƿ��Զ��ƶ�
    public bool isMoveAtStart;
    ///ÿ���ڵ���ƶ��ж϶����ж�Ӧ������ж�
    public bool isConditionMove;
    ///�� �����С�������Χʱ���ж�Ϊ�ﵽ��·����
    public float MinDistanceToGoal = 0.1f;
    ///Ҫ�ƶ�������ĳ�ʼtransformposion
    public Vector3 _originalTransformPostion;
    ///�Ƿ��õ��˳�ʼtransformposion
    protected bool _originalTransformPostionStatus = false;
    ///�����Ƿ��ܹ�����·���ƶ�
    public bool CanMove { get; set; }


    public float MovementSpeed = 1;
    ///���ص�ǰƽ̨���ƶ��ٶ� ��Ҫ�ṩ��PlayerControllerʹ��
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
    //����һ���ڵ��Ҫ�ص��ĺ���
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

        if (this.gameObject.name.Equals("����"))
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


        //��ʼ�� Ҫ�ƶ�����Ʒ�ĳ�ʼλ��
        if (!_originalTransformPostionStatus)
        {
            _originalTransformPostionStatus = true;
            _originalTransformPostion = transform.position;
        }

        //��λ����һ����
        transform.position = _originalTransformPostion + _currnetPoint.Current;
    }

    /// <summary>
    /// �����ƶ�������path�Ŀ�ʼ��
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

            //���������һ���ڵ�
            if (distance < MinDistanceToGoal)
            {
                _previousPoint = _currnetPoint.Current;

            if (afterReachPointCallBack != null)
            {
                afterReachPointCallBack();
            }
                
            //����ûд�ã�дд��������ӣ���Ҫ�ƶ��ľͰ�isConditionMove����Ϊtrue����
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
    /// ƽ̨�����ƶ��ĵط�
    /// ��ʱû��д���� AnimationCurve �����
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
    /// ���� �ܹ����ϸ��µ�ǰ·��������� ������
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
    /// ���õ�����һ���ڵ��Ҫ��������
    /// </summary>
    /// <param name="callback"></param>
    public void SetReachPointCallBack(UnityAction callback)
    {
        afterReachPointCallBack = callback;
    }


    /// <summary>
    /// ��ÿһ���㻭gizmos
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
