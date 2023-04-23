using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * ��̵Ľű�
 */
public class Spike : MonoBehaviour
{
    // ���߳���
    public float lineLength = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.queriesStartInColliders = false;
    }

    // Update is called once per frame
    void Update()
    {
        // �����߼��
        RaycastHit2D hitInfoLeft =  DebugHelper.RaycastAndDrawLine(this.transform.position, Vector2.left, lineLength, LayerMgr.PlayerLayerMask);
        GetLeftRaycast(hitInfoLeft);
    }



    /// <summary>
    /// ���˵ı���
    /// </summary>
    protected virtual void DamageAction()
    {

    }

    /// <summary>
    /// ��ȡ��ߵ����߼����Ϣ
    /// </summary>
    /// <param name="rhit">����Ľṹ��</param>
    private void GetLeftRaycast(RaycastHit2D rhit)
    {
        if (rhit.rigidbody.gameObject.name == "player")
        {
            Health_new health_New = new Health_new();
            health_New.Damage(0.5f, null);
        }
    }



}
