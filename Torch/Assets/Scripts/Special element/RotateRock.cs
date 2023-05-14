using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class RotateRock : MonoBehaviour
{
    public static float _shootFireVcOrginSize;
    protected CircleCollider2D _circleCollider;
    protected Rigidbody2D _rbody;
    protected AutoRotate _autoRotate;

    protected bool hasGetCameraSize =false; 

    void Start()
    {
        Initialization();
    }

    protected void Initialization()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
        _autoRotate = GetComponent<AutoRotate>();
        _rbody = this.gameObject.AddComponent<Rigidbody2D>();
        _rbody.bodyType = RigidbodyType2D.Kinematic;

        CinemachineVirtualCamera camera = CameraMgr.GetInstance().GetCurrentActiveCamera();
        if (camera != null)
        {
            _shootFireVcOrginSize = camera.m_Lens.OrthographicSize;
            hasGetCameraSize = true;
        }


    }


    private void Update()
    {
        CinemachineVirtualCamera camera = CameraMgr.GetInstance().GetCurrentActiveCamera();
        if (camera != null && !hasGetCameraSize)
        {
            _shootFireVcOrginSize = camera.m_Lens.OrthographicSize;
            hasGetCameraSize = true;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("ShootFire"))
        {
            // 触发 fire 执行 StopAndAttach 
            EventMgr.GetInstance().EventTrigger<Transform>("StopAndAttach",this.transform);
            _autoRotate.SetRotate(true);

            // 附着时，让 camera 跟随 rock
            CameraMgr.GetInstance().ChangeFollow(this.transform);
            // 改变 Camera 的镜头大小
            float newOriginSize = _shootFireVcOrginSize + (transform.localScale.x - 1) * 1;
            CameraMgr.GetInstance().SetOrthoSize(newOriginSize , 2);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("ShootFire"))
        {
            _autoRotate.RotateAtStart = false;
        }
    }


}
