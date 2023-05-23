using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 这个类是用来对水滴逻辑进行控制
 */

public class BeadObejct : MonoBehaviour
{


    public void Start()
    { 
     
        
    }

    public void FixedUpdate()
    {
        // 开启延时函数

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag.Equals("Player") || collision.gameObject.tag.Equals("Ground"))
        {
            Destroy(this.gameObject);
            if (collision.gameObject.tag.Equals("Player")) {
                // 获得主角的对象
                Player player = collision.gameObject.GetComponent<Player>();
                // 更改主角的状态为冰冻
                player.Condition.ChangeState(PlayerStates.PlayerConditions.Forzen);
                // 获取主角的控制器
                PlayerController controller = collision.gameObject.GetComponent<PlayerController>();
                // 关闭物体的运动状态
                controller.SetForce(Vector2.zero);
                // 禁止用户的输入，以此达到冰冻的效果
                InputManager.GetInstance().InputDetectionActive = false;
                // 创建定时器的实例，调用SetTimer方法
                Timer.GetInstance().SetTimer(2, () =>
                {
                    // 更改主角的状态为正常
                    player.Condition.ChangeState(PlayerStates.PlayerConditions.Normal);
                    // 允许用户输入
                    InputManager.GetInstance().InputDetectionActive = true;

                });
            }


           
            
        }
    }

  
}
