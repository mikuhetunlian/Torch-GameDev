using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimate : MonoBehaviour
{
    public Vector3 origin;
    // 攻击高度
    public float height;
    // 攻击宽度
    public float width;
    // 动画机
    Animator animator;
    // 进攻模式
    public int attackMode = 0;
    // 是否结束动画
    bool isOver = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        playerAttack();
      
        
    }

    public void playerAttack()
    {
        
       if(Input.GetKey(KeyCode.K) && isOver) {
            switch (attackMode)
            {
                case 0:
                    animator.SetBool("attack1", true);
                    
                    
                    break;
                case 1:
                    animator.SetBool("attack2", true);
                    break;
            }
            isOver = false;

        }


    }

    /// <summary>
    /// 攻击1的攻击事件
    /// </summary>
    public void attack1_touchFire_attack()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + origin, new Vector3(width * 2.5f, height * 2.5f, 0), 0, LayerMgr.EnemyMask);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag.Equals("Wolf"))
            {
                Debug.Log("检测到了");
                wolf animal = collider.gameObject.GetComponent<wolf>();
                WolfHealth wolfHealth = animal.GetComponent<WolfHealth>();
                wolfHealth.Damage(15f, null);
            }
        }
    }

    /// <summary>
    /// 攻击1的结束事件
    /// </summary>
    public void attack1_touchFire_over()
    {
        animator.SetBool("attack1", false);
        attackMode = 1;
        isOver = true;
    }


    /// <summary>
    /// 攻击2的攻击事件
    /// </summary>
    public void attack2_touchFire_attack()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + origin, new Vector3(width * 2.5f, height, 0), 0, LayerMgr.EnemyMask);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag.Equals("Wolf"))
            {
                Debug.Log("检测到了");
                wolf animal = collider.gameObject.GetComponent<wolf>();
                WolfHealth wolfHealth = animal.GetComponent<WolfHealth>();
                wolfHealth.Damage(10f, null);
            }
        }
    }

    /// <summary>
    /// 攻击2的结束事件
    /// </summary>
    public void attack2_touchFire_over()
    {
        animator.SetBool("attack2", false);
        attackMode = 0;
        isOver = true;
    }





    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(this.transform.position + origin, new Vector3(width * 2.5f, height * 2.5f, 0));
    }




}
