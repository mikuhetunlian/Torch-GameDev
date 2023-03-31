using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootFire : MonoBehaviour
{

    public float shootSpeed;
    public float orginShootSpeed;

    // rock旋转速度的绝对值对fire下一次发射的速度影响系数，0代表没有影响，1代表影响最大
    protected float _speedEffectFactor;
    protected Transform _transform;
    protected Transform _parent;
    protected CircleCollider2D _circleCollider;
    protected Vector2 dir;
    protected bool isShooting;
    protected bool isLastShoot;



    void Start()
    {
        Initialization();
    }

    /// <summary>
    /// 进行数据的初始化
    /// </summary>
    protected void Initialization()
    {
        EventMgr.GetInstance().AddLinstener<Transform>("StopAndAttach", StopAndAttach);
        EventMgr.GetInstance().AddLinstener<Transform>("Stop", Stop);
        _transform = GetComponent<Transform>();
        _parent = _transform.parent;
        _circleCollider = GetComponent<CircleCollider2D>();
        isShooting = false;
        isLastShoot = false;
        if (shootSpeed == 0)
        {
            shootSpeed = 5.0f;
        }
        orginShootSpeed = shootSpeed;
        _speedEffectFactor = 0.8f;
    }


   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

        Shooting();
    }

    public void Shoot()
    {
        if (!isShooting)
        {
            if (isLastShoot)
            {
                // 改变 camera x方向跟随阻尼为0
                CameraMgr.GetInstance().SetDamping(new Vector2(0, 1));
            }

            isShooting = true;
            dir = (_transform.position - _parent.transform.position).normalized;
            // shoot 前将parent设为null
            _transform.parent = null;
            // 每次shoot前将camera跟随设置回自己
            CameraMgr.GetInstance().ChangeFollow(_transform);
        }
    }


    protected void Shooting()
    {
        if (isShooting)
        {
            _transform.Translate(dir * shootSpeed * Time.deltaTime, Space.World);
        }
    }


    public void StopAndAttach(Transform newParent)
    {

        Stop(newParent);

        // 调整fire附着到rock上的为位置
        Vector3 dirFromRockToFire = (_transform.position - _parent.position).normalized;
        float parentRaduis = newParent.GetComponent<CircleCollider2D>().bounds.extents.x;
        float myRaduis = _circleCollider.bounds.extents.x;
        _transform.position = _parent.position + dirFromRockToFire * (parentRaduis + myRaduis);

        // 根据rock的旋转速度调整下一次fire射出的速度
        AutoRotate autoRotate = newParent.GetComponent<AutoRotate>();
        shootSpeed = orginShootSpeed +  Mathf.Abs(autoRotate.RotateSpeed.z) * _speedEffectFactor;
        
    }

    public void Stop(Transform newParent)
    {
        isShooting = false;
        _transform.parent = newParent;
        _parent = _transform.parent;
        Debug.Log("停止");
    }

    /// <summary>
    /// 设置为最后一次射击
    /// </summary>
    public void SetLastShoot()
    {
        if (!isLastShoot)
        {
            isLastShoot = true;
        }
    }

    
}
