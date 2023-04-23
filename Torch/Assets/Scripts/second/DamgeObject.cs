using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamgeObject : MonoBehaviour
{
 

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            Health_new health = collision.gameObject.GetComponent<Health_new>();
            health.Damage(10, DamgeAction);
        }
    }

    protected virtual void DamgeAction( )
    {
        



    }
}
