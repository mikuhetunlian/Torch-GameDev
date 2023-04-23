using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health_new : MonoBehaviour
{

   public float currentHp = 5f;
   public int maxHp;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float damage, UnityAction action)
    {
        
        currentHp -= damage;
        Debug.Log("当前的HP值：" + currentHp);
        /*if (currentHp <= 0)
        {
            Destroy(gameObject);
        }*/
    }
}
