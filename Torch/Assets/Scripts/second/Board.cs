using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public float currentHp = 20f;
    // 射线长度
    public float lineLength = 0.1f;
    // 左上角
    public Vector2 leftTop;
    // 右上角
    public Vector2 rightTop;
    // 射线个数
    public float rayCastNum = 64;
    protected BoxCollider2D boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.queriesStartInColliders = false;
        // 获取该碰撞体的碰撞器
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RayCastToTop();
        isDestory();
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
        for (float i = 0; i < rayCastNum; i++)
        {
            Vector2 origin = Vector2.Lerp(leftTop, rightTop, i / (rayCastNum - 1));
            // 发射射线
            RaycastHit2D hitInfoTop = DebugHelper.RaycastAndDrawLine(origin, Vector2.up, lineLength, LayerMgr.PlayerLayerMask);
            touchPlayer(hitInfoTop);
        }

    }

    /// <summary>
    /// 如果玩家带的时间过长，将会自动销毁
    /// </summary>
    /// <param name="hitInfoTop">传入的射线检测结构体</param>
   public void touchPlayer(RaycastHit2D hitInfoTop)
    {
        if(hitInfoTop.collider != null && hitInfoTop.collider.gameObject.tag.Equals("Player"))
        {
            currentHp -= 0.25f;
            Debug.Log("踏板的生命值" + currentHp);
        }
    }

    /// <summary>
    /// 销毁踏板
    /// </summary>
    public void isDestory()
    {
        if(currentHp <= 0f) {
            Destroy(this.gameObject);
        }
    }
    
}
