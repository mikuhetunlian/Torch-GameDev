using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wolf : MonoBehaviour
{
    public Vector3 origin;
    public float height;
    public float width;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void attack_wolf()
    {
        Debug.Log("attack_wolf");
    }

    public void attack()
    {
        Debug.Log("attack");
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + origin, new Vector3(width, height, 0),0, LayerMgr.PlayerLayerMask);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag.Equals("Player"))
            {
                Debug.Log("jiancedao wanjia");
            }
        }
    }


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(this.transform.position + origin, new Vector3(width, height, 0));
    }

    


}
