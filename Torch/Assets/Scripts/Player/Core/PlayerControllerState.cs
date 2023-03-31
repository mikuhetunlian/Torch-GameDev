using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerState
{
  
    public bool isCollidingRight { get; set; }
    public bool isCollidingLeft { get; set; }
    public bool isCollidingAbove { get; set; }
    public bool isCollidingBelow { get; set; }
    ///是否靠近到能够操控物体的程度
    public bool isDetectControlableObject { get; set; }

    ///有没有碰到什么
    public bool HasCollision 
    {
        get
        {
            return isCollidingAbove || isCollidingBelow || isCollidingLeft || isCollidingRight;
        }
    }

    ///left碰撞到的东西的距离,-1代表left没有碰撞到东西
    public float DistanceToLeftCollider;
    ///right碰撞到的东西的距离,-1代表right没有碰撞到东西
    public float DistanceToRightCollider;

    ///边上的斜坡的角度
    public float LateralSlopeAngle { get; set; }
    ///正在走的斜坡的角度
    public float BelowSlopeAngle { get; set; }
    ///斜坡能不能走
    public bool SlopeAngleOK { get; set; }

    ///是否在地面上
    public bool IsGrounded { get { return isCollidingBelow; } }
    ///是否正在跳跃
    public bool IsJumping { get; set; }
    ///是否正在falling
    public bool IsFalling { get; set; }
    ///是否在movingPlatform上
    public bool IsOnMovingPlatform { get; set; }
    ///上一帧是不是在地面上
    public bool WasGroundedLastFrame { get; set; }
    ///上一帧是不是撞到了天花板上
    public bool WasTouchingTheCeilLastFrame { get; set; }
    ///是不是刚刚碰到地面
    public bool JustGotGround { get; set; }
    ///有没有缩放碰撞器大小来适应小空间
    public bool ColliderResized { get; set; }
    ///是否在右边操控物体
    public bool IsControlingRight { get; set; }
    ///是否在左边操控物体
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
        //因为一开始mei是往下掉的
        IsFalling = true;
        IsJumping = false;
        LateralSlopeAngle = 0;
    }


}
