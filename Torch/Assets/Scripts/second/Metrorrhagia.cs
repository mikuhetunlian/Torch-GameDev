using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 雪崩的脚本
 */

public class Metrorrhagia : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 初始化物体的速度为10
        GetComponent<Rigidbody2D>().velocity = new Vector2(10, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 碰到玩家就销毁玩家
        if(collision.gameObject.tag.Equals("Player"))
        {
            Destroy(collision.gameObject);
        }
    }
}
