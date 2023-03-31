using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    //ladder上方的平台
    public GameObject LadderPlatform;
    //在梯子上是否保持在中间位置
    public bool CenterOnLadder;
    protected Collider2D _collider;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            PlayerLadder playerLadder = collision.gameObject.GetComponent<PlayerLadder>();
            playerLadder.AddColliderLadder(_collider);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Player"))
        {
            PlayerLadder playerLadder = collision.gameObject.GetComponent<PlayerLadder>();
            playerLadder.RemoveColliderLadder(_collider);
        }
    }

}
