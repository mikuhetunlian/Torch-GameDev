using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WolfHealth : MonoBehaviour
{
    public float health = 500f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float damage, UnityAction action)
    {

        health -= damage;
        action?.Invoke();
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
