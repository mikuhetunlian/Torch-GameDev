using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepAtPoint : MonoBehaviour
{

    public enum FollowType
    {
        Fix,
        Lerp
    }

    ///�����״̬
    public FollowType followType = FollowType.Fix;

    public GameObject Target;
    public float  offsetY;
    public float offsetX;
    ///������ٶ� ֻ����FollowTypeΪLerp�²�����
    public float followSpeed;

    protected Vector2 _followPoint;

    // Update is called once per frame
    void Update()
    {
        Follow();
    }

    /// <summary>
    /// ����Ŀ��
    /// </summary>
    protected void Follow()
    {
        if (Target == null) { return; }
        _followPoint = Target.transform.position + Vector3.right * offsetX + Vector3.up * offsetY;

        if (followType == FollowType.Fix)
        {
            transform.position = _followPoint;
        }
        if (followType == FollowType.Lerp)
        {
            transform.position = Vector3.Lerp(transform.position, _followPoint, followSpeed * Time.deltaTime);
        }
        
    }

    /// <summary>
    /// ����ƫ��
    /// </summary>
    /// <param name="offset"></param>
    public void SetOffset(Vector2 offset)
    {
        offsetX = offset.x;
        offsetY = offset.y;
    }

    

}
