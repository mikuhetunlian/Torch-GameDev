using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Manger : MonoBehaviour
{

    // ˮ��Ԥ����
    public GameObject beadPrefab;


    void Start()
    {
        // ������ʱ����,��ʱ����ִ�У�ÿ��ִ��ʱ��������
        InvokeRepeating("createObject", 1.5f, 7);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
      
    }

    /// <summary>
    /// �����ظ���������
    /// </summary>
    public void createObject()
    {
        // ����һ�������������Ϊ��52.4��-1��0������תΪ0
        GameObject.Instantiate(beadPrefab, new Vector3(52.4f, -1, 0), Quaternion.identity);
        // ����һ�������������Ϊ��63.6��-1��0������תΪ0
        GameObject.Instantiate(beadPrefab,new Vector3(63.6f, -1, 0), Quaternion.identity);
        

    }
}
