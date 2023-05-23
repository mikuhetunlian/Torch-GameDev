using PathCreation.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 尖刺的脚本
 */
public class Spike : MonoBehaviour
{
    // player的游戏对象
    public Player player;
    // 射线长度
    public float lineLength = 0.1f;
   // 左上角
    public Vector2 leftTop;
    // 左下角
    public Vector2 leftDown;
    // 右上角
    public Vector2 rightTop;
    // 右下角
    public Vector2 rightDown;
    // 射线个数
    public float rayCastNum = 8;
    protected BoxCollider2D boxCollider;
    
    
  


    void Start()
    {
        Physics2D.queriesStartInColliders = false;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // 左射线检测
        RayCastToLeft();
        // 右射线检测
        RayCastToRight();
        // 上射线检测
        RayCastToTop();
    }

    /// <summary>
    /// 向左边发出射线
    /// </summary>
    public void RayCastToLeft()
    {
        // 获得左上角的向量
        leftTop = boxCollider.bounds.center + new Vector3(-boxCollider.bounds.extents.x, boxCollider.bounds.extents.y);
        // 获得左下角的向量
        leftDown = boxCollider.bounds.center + new Vector3(-boxCollider.bounds.extents.x, -boxCollider.bounds.extents.y);
        for (float i = 0; i < rayCastNum; i++)
        {
            // 使用插值函数，求到我们要射出射线的位置
            Vector2 origin = Vector2.Lerp(leftTop, leftDown, i /(rayCastNum-1));
            // 进行射线
            RaycastHit2D hitInfoLeft = DebugHelper.RaycastAndDrawLine(origin, Vector2.left, lineLength, LayerMgr.PlayerLayerMask);
            // 获取射线检测信息
            GetRaycast(hitInfoLeft);
        }
    }

    /// <summary>
    /// 右边的射线检测
    /// </summary>
    public void RayCastToRight()
    {
        // 获得右上角的坐标
        rightTop = boxCollider.bounds.center + new Vector3(boxCollider.bounds.extents.x, boxCollider.bounds.extents.y);
        // 获得右下角的坐标
        rightDown = boxCollider.bounds.center + new Vector3(boxCollider.bounds.extents.x, -boxCollider.bounds.extents.y);
        for(float i = 0; i < rayCastNum; i++)
        {
            Vector2 origin = Vector2.Lerp(rightTop, rightDown, i /(rayCastNum-1));
            // 进行射线
            RaycastHit2D hitInfoRight = DebugHelper.RaycastAndDrawLine(origin, Vector2.right, lineLength, LayerMgr.PlayerLayerMask);
            // 获取射线检测信息
            GetRaycast(hitInfoRight);
        }
        
    }

    /// <summary>
    /// 上方的射线检测
    /// </summary>
    public void RayCastToTop()
    {
        // 获得右上角的坐标
        rightTop = boxCollider.bounds.center + new Vector3(boxCollider.bounds.extents.x, boxCollider.bounds.extents.y);
        // 获得左上角的向量
        leftTop = boxCollider.bounds.center + new Vector3(-boxCollider.bounds.extents.x, boxCollider.bounds.extents.y);
        for(float i = 0; i < rayCastNum; i++)
        {
            Vector2 origin = Vector2.Lerp(leftTop, rightTop, i /(rayCastNum-1));
            // 发射射线
            RaycastHit2D hitInfoTop = DebugHelper.RaycastAndDrawLine(origin, Vector2.up, lineLength, LayerMgr.PlayerLayerMask);
            // 获取射线检测信息
            GetRaycast(hitInfoTop);
        }

    }

    /// <summary>
    /// 修改玩家的状态
    /// </summary>
    protected virtual void DamageAction()
    {
        // 修改玩家的状态为无敌状态
        player.Condition.ChangeState(PlayerStates.PlayerConditions.Invincibility);
        // 获取主角的控制器
        PlayerController controller = player.GetComponent<PlayerController>();
        // 关闭物体的运动状态
        controller.SetForce(Vector2.zero);
        Transform transform = player.transform;
        // 玩家被刺到后有一个向后的移动
        transform.Translate(new Vector2(transform.forward.x * (-0.5f),0));
        // 设置无敌帧的时间为0.8s
        Timer.GetInstance().SetTimer(0.8f, () => {
            // 修改主角状态为正常
            player.Condition.ChangeState(PlayerStates.PlayerConditions.Normal);
        });


    }

    /// <summary>
    /// 获取射线检测信息
    /// </summary>
    /// <param name="rhit">传入的结构体</param>
    private void GetRaycast(RaycastHit2D rhit)
    {
        // 玩家碰到尖刺
        if (rhit.collider != null && rhit.collider.gameObject.tag.Equals("Player"))
        {
             player = rhit.collider.gameObject.GetComponent<Player>();
            // 要准确获得player身上的脚本
            Health_new health_new = player.GetComponent<Health_new>();
            // 玩家的当前状态如果不是无敌的话
            if(player.Condition.CurrentState != PlayerStates.PlayerConditions.Invincibility)
            {
                health_new.Damage(0.5f, DamageAction);
            }
        }
    }



}
