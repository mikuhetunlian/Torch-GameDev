using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 尖刺的脚本
 */
public class Spike : MonoBehaviour
{
    // 射线长度
    public float lineLength = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.queriesStartInColliders = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 左射线检测
        RaycastHit2D hitInfoLeft =  DebugHelper.RaycastAndDrawLine(this.transform.position, Vector2.left, lineLength, LayerMgr.PlayerLayerMask);
        GetLeftRaycast(hitInfoLeft);
    }



    /// <summary>
    /// 受伤的表现
    /// </summary>
    protected virtual void DamageAction()
    {

    }

    /// <summary>
    /// 获取左边的射线检测信息
    /// </summary>
    /// <param name="rhit">传入的结构体</param>
    private void GetLeftRaycast(RaycastHit2D rhit)
    {
        if (rhit.rigidbody.gameObject.name == "player")
        {
            Health_new health_New = new Health_new();
            health_New.Damage(0.5f, null);
        }
    }



}
