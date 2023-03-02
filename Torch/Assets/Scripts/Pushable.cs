using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour
{
    ///player松手后滑行到停止的时间
    public float SlideTime;
    ///是否跟随movingPlatform移动
    public bool IsFollowWithMovingPlatform;
    ///pushable的底部有没有全面接触到MovingPlatform
    public bool IsFullyTouchMovingPlatform { get; protected set; }
    ///是否可以被操控 总开关
    public bool CanBeControl;

    protected BoxCollider2D _collider;
    protected Rigidbody2D _rbody;
    protected PathMovement _movingPlatform;
    protected float _speed;
    protected int _rayCaseNum = 8;
    protected bool _isTouchMovingPlatform;
    protected bool _isFullyTouchMovingPlatform;


    void Start()
    {
        Initilization();
    }


    protected void Initilization()
    {
        _collider = GetComponent<BoxCollider2D>();
        _rbody = GetComponent<Rigidbody2D>();
        Physics2D.queriesStartInColliders = false;
    }


    /// <summary>
    /// 提供给player使用设置速度
    /// </summary>
    /// <param name="xSpeed"></param>
    public void SetSpeed(float xSpeed)
    {
        _speed = xSpeed;
    }


    private void FixedUpdate()
    {
        if (!CanBeControl)
        {
            return;
        }

        DetectMovingPlatform();

        Vector2 _newPostion = new Vector2(_speed * Time.deltaTime, 0);

        if (IsFollowWithMovingPlatform)
        {
            _rbody.bodyType = RigidbodyType2D.Kinematic;
            //保持和movingplatform一样的移动
            if (_isFullyTouchMovingPlatform && _movingPlatform != null && !_movingPlatform.EndReached)
            {
                _newPostion = (_movingPlatform.CurrentSpeed + new Vector3(_speed,0)) * Time.deltaTime;
            }
     
        }

        this.transform.Translate(_newPostion);
    }



    /// <summary>
    /// 检查下方是否有movingplatform
    /// </summary>
    /// <returns></returns>
    protected void DetectMovingPlatform()
    {
   
        Vector2 leftOriginPos = _collider.bounds.center - new Vector3(_collider.bounds.extents.x, 0);
        Vector2 rightOriginPos = _collider.bounds.center + new Vector3(_collider.bounds.extents.x, 0);
        leftOriginPos -= new Vector2(0, _collider.bounds.extents.y);
        rightOriginPos -= new  Vector2(0, _collider.bounds.extents.y);

        _isFullyTouchMovingPlatform = true;
        _isTouchMovingPlatform = false;
        _movingPlatform = null;
        for (int i = 0; i < _rayCaseNum; i++)
        {
            Vector2 originPos = Vector2.Lerp(leftOriginPos, rightOriginPos, (float)i / (_rayCaseNum - 1));
            RaycastHit2D hitInfo = DebugHelper.RaycastAndDrawLine(originPos, Vector2.down, 0.2f, LayerMgr.MovingPlatformLayerMask);
            if (hitInfo.collider != null)
            {
                _movingPlatform = hitInfo.collider.GetComponent<PathMovement>();
                _isTouchMovingPlatform = true;
            }
            else
            {
                _isFullyTouchMovingPlatform = false;
            }
        }

        IsFullyTouchMovingPlatform = _isFullyTouchMovingPlatform;
    }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMgr.MovingPlatformLayer)
        {
            Debug.Log("碰到movingPlatform");
        }

    }
}
