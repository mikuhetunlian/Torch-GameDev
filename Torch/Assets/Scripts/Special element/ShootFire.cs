using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootFire : MonoBehaviour
{

    public float shootSpeed;
    public float orginShootSpeed;

    // rock��ת�ٶȵľ���ֵ��fire��һ�η�����ٶ�Ӱ��ϵ����0����û��Ӱ�죬1����Ӱ�����
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
    /// �������ݵĳ�ʼ��
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
                // �ı� camera x�����������Ϊ0
                CameraMgr.GetInstance().SetDamping(new Vector2(0, 1));
            }

            isShooting = true;
            dir = (_transform.position - _parent.transform.position).normalized;
            // shoot ǰ��parent��Ϊnull
            _transform.parent = null;
            // ÿ��shootǰ��camera�������û��Լ�
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

        // ����fire���ŵ�rock�ϵ�Ϊλ��
        Vector3 dirFromRockToFire = (_transform.position - _parent.position).normalized;
        float parentRaduis = newParent.GetComponent<CircleCollider2D>().bounds.extents.x;
        float myRaduis = _circleCollider.bounds.extents.x;
        _transform.position = _parent.position + dirFromRockToFire * (parentRaduis + myRaduis);

        // ����rock����ת�ٶȵ�����һ��fire������ٶ�
        AutoRotate autoRotate = newParent.GetComponent<AutoRotate>();
        shootSpeed = orginShootSpeed +  Mathf.Abs(autoRotate.RotateSpeed.z) * _speedEffectFactor;
        
    }

    public void Stop(Transform newParent)
    {
        isShooting = false;
        _transform.parent = newParent;
        _parent = _transform.parent;
        Debug.Log("ֹͣ");
    }

    /// <summary>
    /// ����Ϊ���һ�����
    /// </summary>
    public void SetLastShoot()
    {
        if (!isLastShoot)
        {
            isLastShoot = true;
        }
    }

    
}
