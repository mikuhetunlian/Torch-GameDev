using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Manger : MonoBehaviour
{

    // 水滴预制体
    public GameObject beadPrefab;


    void Start()
    {
        // 开启延时函数,延时六秒执行，每次执行时间间隔五秒
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
    /// 用于重复创建对象
    /// </summary>
    public void createObject()
    {
        // 创建一个物体对象，坐标为（52.4，-1，0），旋转为0
        GameObject.Instantiate(beadPrefab, new Vector3(52.4f, -1, 0), Quaternion.identity);
        // 创建一个物体对象，坐标为（63.6，-1，0），旋转为0
        GameObject.Instantiate(beadPrefab,new Vector3(63.6f, -1, 0), Quaternion.identity);
        

    }
}
