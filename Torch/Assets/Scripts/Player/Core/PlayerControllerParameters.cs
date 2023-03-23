using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerControllerParameters类 的主要作用是统一储存 PlayerController类会用到的参数，比如说重力大小，下降倍率等。
/// </summary>
public class PlayerControllerParameters 
{
    [Header("Gravity")]
    public float Gravity = -50f;
    ///下降倍增器
    public float FallMultiplier = 1f;
    ///上升倍增器
    public float AscentMultiplier = 1f;

    [Header("Speed")]
    ///最大速度，防止某些情况下移动过快
    public Vector2 MaxVelocity = new Vector2(100f, 100f);
    ///地面上的加速度
    public float SpeedAccelerationOnGround = 20f;
    ///空气中的加速度
    public float SpeedAccelerrationInAir = 5f;
    ///暂时不知道是什么的影响因素
    public float SpeedFactor = 1;

    [Header("Slopes")]
    ///最大可以行走的斜坡的角度
    [Range(0,90)]
    public float MaximumSlopeAngel = 30f;
    ///在斜坡上行走的速度调曲线
    public AnimationCurve SlopeAngleSpeedFactor = new AnimationCurve(new Keyframe(-90, 1f), new Keyframe(0, 1), new Keyframe(90f, 1f));

    [Header("Physics2D Interaction ")]
    ///如果为ture，mei碰撞到物体会给它施加力
    public bool Physics2DInteraction = true;
    ///碰撞到物体施加的力
    public float Physic2DPushOrPullForce = 10f;

    [Header("Gizmos")]
    ///是否画出射线的示意图
    public bool DrawRayCastsGizmos = true;
    
}
