using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * �������������ˮ���߼����п���
 */

public class BeadObejct : MonoBehaviour
{


    public void Start()
    { 
     
        Debug.Log("kaishi jishi");
    }

    public void FixedUpdate()
    {
        // ������ʱ����

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag.Equals("Player") || collision.gameObject.tag.Equals("Ground"))
        {
            Destroy(this.gameObject);
            if (collision.gameObject.tag.Equals("Player")) {
                // ������ǵĶ���
                Player player = collision.gameObject.GetComponent<Player>();
                // �������ǵ�״̬Ϊ����
                player.Condition.ChangeState(PlayerStates.PlayerConditions.Forzen);
                // ��ȡ���ǵĿ�����
                PlayerController controller = collision.gameObject.GetComponent<PlayerController>();
                // �ر�������˶�״̬
                controller.SetForce(Vector2.zero);

                // ��ֹ�û������룬�Դ˴ﵽ������Ч��
                InputManager.GetInstance().InputDetectionActive = false;
                // ������ʱ����ʵ��������SetTimer����
                Timer.GetInstance().SetTimer(2, () =>
                {
                    // �������ǵ�״̬Ϊ����
                    player.Condition.ChangeState(PlayerStates.PlayerConditions.Normal);
                    // �����û�����
                    InputManager.GetInstance().InputDetectionActive = true;

                });
            }


           
            
        }
    }

  
}
