using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimate : MonoBehaviour
{
    // 武器耐久度
    public float weaponHealth = 100;
    Animator animator;
    // 进攻模式
    public int attackMode = 0;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        playerAttack();
        getL();
        
    }

    public void playerAttack()
    {
        
       if(Input.GetKey(KeyCode.K)) {
            switch (attackMode)
            {
                case 0:
                    animator.SetBool("attack1", true);
                    attackMode = 1;
                    
                    break;
                case 1:
                    animator.SetBool("attack2", true);
                    attackMode = 0;
                    
                    break;
            }

        }


    }

    public void getL()
    {
        Debug.Log("播放完idle");

    }

  
}
