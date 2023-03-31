using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateByPoint : MonoBehaviour
{

    public float Speed;
    public float radius;
    public GameObject RotatePointObj;
    protected Vector2 PathPoint;

    void Start()
    {
        StartCoroutine(GetPathPoint());
    }

  
    void Update()
    {
        transform.position = PathPoint;
    }



    protected IEnumerator GetPathPoint()
    {
        float t = 0;
        while (true)
        {
            Vector3 origin = RotatePointObj.transform.position;
            if (t >= 360)
            {
                t = 0;
            }
            float x = origin.x + radius * Mathf.Cos(Speed * t * Mathf.Deg2Rad);
            float y = origin.y + radius * Mathf.Sin(Speed * t * Mathf.Deg2Rad);
            PathPoint.x = x;
            PathPoint.y = y;
            t += 1;
            yield return new WaitForFixedUpdate();
        }

      
    }
}
