using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * ѩ���Ľű�
 */

public class Metrorrhagia : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // ��ʼ��������ٶ�Ϊ10
        GetComponent<Rigidbody2D>().velocity = new Vector2(10, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ������Ҿ��������
        if(collision.gameObject.tag.Equals("Player"))
        {
            Destroy(collision.gameObject);
        }
    }
}
