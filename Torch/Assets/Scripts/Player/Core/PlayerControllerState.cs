using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerState
{
  
    public bool isCollidingRight { get; set; }
    public bool isCollidingLeft { get; set; }
    public bool isCollidingAbove { get; set; }
    public bool isCollidingBelow { get; set; }
    ///�Ƿ񿿽����ܹ��ٿ�����ĳ̶�
    public bool isDetectControlableObject { get; set; }

    ///��û������ʲô
    public bool HasCollision 
    {
        get
        {
            return isCollidingAbove || isCollidingBelow || isCollidingLeft || isCollidingRight;
        }
    }

    ///left��ײ���Ķ����ľ���,-1����leftû����ײ������
    public float DistanceToLeftCollider;
    ///right��ײ���Ķ����ľ���,-1����rightû����ײ������
    public float DistanceToRightCollider;

    ///���ϵ�б�µĽǶ�
    public float LateralSlopeAngle { get; set; }
    ///�����ߵ�б�µĽǶ�
    public float BelowSlopeAngle { get; set; }
    ///б���ܲ�����
    public bool SlopeAngleOK { get; set; }

    ///�Ƿ��ڵ�����
    public bool IsGrounded { get { return isCollidingBelow; } }
    ///�Ƿ�������Ծ
    public bool IsJumping { get; set; }
    ///�Ƿ�����falling
    public bool IsFalling { get; set; }
    ///�Ƿ���movingPlatform��
    public bool IsOnMovingPlatform { get; set; }
    ///��һ֡�ǲ����ڵ�����
    public bool WasGroundedLastFrame { get; set; }
    ///��һ֡�ǲ���ײ�����컨����
    public bool WasTouchingTheCeilLastFrame { get; set; }
    ///�ǲ��Ǹո���������
    public bool JustGotGround { get; set; }
    ///��û��������ײ����С����ӦС�ռ�
    public bool ColliderResized { get; set; }
    ///�Ƿ����ұ߲ٿ�����
    public bool IsControlingRight { get; set; }
    ///�Ƿ�����߲ٿ�����
    public bool IsControlingLeft { get; set; }

    /// <summary>
    /// reset all collsion states to false
    /// </summary>
    public virtual void Reset()
    {
        isCollidingAbove = false;
        isCollidingLeft = false;
        isCollidingRight = false;
        DistanceToLeftCollider = -1;
        DistanceToRightCollider = -1;
        SlopeAngleOK = false;
        JustGotGround = false;
        //��Ϊһ��ʼmei�����µ���
        IsFalling = true;
        IsJumping = false;
        LateralSlopeAngle = 0;
    }


}
