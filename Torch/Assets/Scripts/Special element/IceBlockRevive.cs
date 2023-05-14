using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlockRevive : MonoBehaviour
{
    public GameObject IceBlockPrefab;
    public GameObject IceBlockParent;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMgr.PlatformsLayer)
        {

            for (int i = 0; i < IceBlockParent.transform.childCount; i++)
            {
                GameObject childIceBlock = IceBlockParent.transform.GetChild(i).gameObject;
                GameObject.Destroy(childIceBlock);
            }

            GameObject iceBlock = GameObject.Instantiate(IceBlockPrefab);
            iceBlock.transform.position = new Vector3(158.6f, 28.3f, 0);
            iceBlock.transform.parent = IceBlockParent.transform;
        }
    }
}
