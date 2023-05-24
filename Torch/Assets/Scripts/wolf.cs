using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wolf : MonoBehaviour
{
    public Vector3 origin;
    // 攻击高度
    public float height = 2f;
    // 攻击宽度
    public float width = 3.6f;
    // 攻击欲望
    public bool attackDesire = false;
    // 攻击模式,有三种攻击模式，分别对应代码0，1，2
    public int attackMode = 0;
    // 原来的位置
    Vector2 postion;
    // 玩家的坐标
    public GameObject player;
    // 爪击次数
    int catchTimes = 0;
    // 狼的动画机
    Animator animator;
    // 是否结束动画
    bool isOver = true;

    

    // Start is called before the first frame update
    void Start()
    {
        EventMgr.GetInstance().AddLinstener<bool>("SetAttackDesire", SetAttackDesire);
        postion = transform.position;
        animator = GetComponent<Animator>();       
    }

    // Update is called once per frame
    void Update()
    {
        StartAttack();
        Debug.Log(attackMode);
        
    }


   

   public void attack_pull_attack()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + origin, new Vector3(width * 2.5f, height, 0), 0, LayerMgr.PlayerLayerMask);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag.Equals("Player"))
            {
                Player player = collider.gameObject.GetComponent<Player>();
                Health_new health_new = player.GetComponent<Health_new>();
                health_new.Damage(1.5f, null);
            }
        }
    }


    public void jump()
    {
        Debug.Log(player.transform.position.x);
        Vector2 vector2 = player.transform.position;
        transform.Translate(new Vector2(vector2.x - postion.x, postion.y));
    }


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(this.transform.position + origin, new Vector3(width, height, 0));
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(this.transform.position + origin, new Vector3(width*2.5f, height, 0));
    }

    /// <summary>
    /// 爪击攻击
    /// </summary>
    public void attack_catch_attack()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + origin, new Vector3(width, height, 0),0, LayerMgr.PlayerLayerMask);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag.Equals("Player"))
            {
                Player player = collider.gameObject.GetComponent<Player>();
                Health_new health_new = player.GetComponent<Health_new>();
                health_new.Damage(1f, null);
            }
        }
    }

    /// <summary>
    /// 爪击动画结束的触发事件
    /// </summary>
    public void attack_catch_over()
    {
        catchTimes++;
        if (catchTimes >= 3)
        {
            animator.SetBool("onlyCatch", false);
            attackMode = Random.Range(0, 2);
            isOver = true;
        }
       
    }


    public void SetAttackDesire(bool desire)
    {
        attackDesire = desire;
    }

    public void StartAttack()
    {
        if(attackDesire && isOver)
        {
            switch (attackMode)
            {
                // 连续三次爪击
                case 0:
                    // 只是爪击
                    animator.SetBool("onlyPull", false);
                    animator.SetBool("onlyCatch", true);                   
                    break;
                
                // 冲击爪击
                case 1:
                    // 扑击
                    animator.SetBool("onlyCatch", false);
                    animator.SetBool("onlyPull", true);
                    
                    attackMode = Random.Range(0, 2);
                    break;
            }
            isOver = false;
        }
    }

    public void attack_pull_over()
    {
        // 回归原位
        transform.position = postion;
        // 关闭扑击
        animator.SetBool("onlyPull", false);
        attackMode = Random.Range(0, 2);
        isOver = true;
    }
    


}
