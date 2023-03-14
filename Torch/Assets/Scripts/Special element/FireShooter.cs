using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShooter : MonoBehaviour
{
    public float cameraMagnification;
    public float shooterShootSpeed;

    protected CircleCollider2D _circleCollider;
    protected float _shootFireVcOrginOrthoSize;


    void Start()
    {
        Initialization();
    }

    protected void Initialization()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
        _shootFireVcOrginOrthoSize = CameraMgr.GetInstance().GetCurrentActiveCamera().m_Lens.OrthographicSize;
        if (cameraMagnification == 0)
        {
            cameraMagnification = 1;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("ShootFire"))
        {
            EventMgr.GetInstance().EventTrigger<Transform>("Stop", this.transform);
            collision.gameObject.transform.position = this.transform.position + Vector3.right * (_circleCollider.bounds.extents.x + collision.bounds.extents.x);



            // 设置新的 shootSpeed
            ShootFire shootFire = collision.gameObject.GetComponent<ShootFire>();
            shootFire.shootSpeed = shooterShootSpeed;
            // 设置为最后一次射击
            shootFire.SetLastShoot();


            // 改变camera OrthoSize
            float newOrthoSize = _shootFireVcOrginOrthoSize + (transform.localScale.x - 1) * cameraMagnification;
            CameraMgr.GetInstance().SetOrthoSize(newOrthoSize, 2);


        }
    }
}
