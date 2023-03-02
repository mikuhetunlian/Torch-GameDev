using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerParameters 
{
    [Header("Gravity")]
    public float Gravity = -50f;
    ///�½�������
    public float FallMultiplier = 1f;
    ///����������
    public float AscentMultiplier = 1f;

    [Header("Speed")]
    ///����ٶȣ���ֹĳЩ������ƶ�����
    public Vector2 MaxVelocity = new Vector2(100f, 100f);
    ///�����ϵļ��ٶ�
    public float SpeedAccelerationOnGround = 20f;
    ///�����еļ��ٶ�
    public float SpeedAccelerrationInAir = 5f;
    ///��ʱ��֪����ʲô��Ӱ������
    public float SpeedFactor = 1;

    [Header("Slopes")]
    ///���������ߵ�б�µĽǶ�
    [Range(0,90)]
    public float MaximumSlopeAngel = 30f;
    ///��б�������ߵ��ٶȵ�����
    public AnimationCurve SlopeAngleSpeedFactor = new AnimationCurve(new Keyframe(-90, 1f), new Keyframe(0, 1), new Keyframe(90f, 1f));

    [Header("Physics2D Interaction ")]
    ///���Ϊture��mei��ײ����������ʩ����
    public bool Physics2DInteraction = true;
    ///��ײ������ʩ�ӵ���
    public float Physic2DPushOrPullForce = 10f;

    [Header("Gizmos")]
    ///�Ƿ񻭳����ߵ�ʾ��ͼ
    public bool DrawRayCastsGizmos = true;
    
}
