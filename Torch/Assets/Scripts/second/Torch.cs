using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Torch : MonoBehaviour
{
  
    // Start is called before the first frame update
    void Start()
    {
   

    }

    // Update is called once per frame
    void Update()
    {
        //RatcastToLeft();
  

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.tag.Equals("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if(player.Condition.CurrentState != PlayerStates.PlayerConditions.Fight)
            {
                Animator animator = collision.gameObject.GetComponent<Animator>();
                // 切换主角持有火把的动画
                animator.SetBool("stateWithTorch", true);
                // 获得主角的对象
                player.Condition.ChangeState(PlayerStates.PlayerConditions.Fight);
            }
          

        }
    }


}
