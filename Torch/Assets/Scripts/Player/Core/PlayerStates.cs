using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����������ǰplayer������״̬��ʲô
/// </summary>
public class PlayerStates
{
    public enum PlayerConditions
    {
        Normal,//�޲���
        Move,//�ƶ���
        Forzen,//������
        Dead,//����
    }


    public enum MovementStates
    {
        Null,
        Idle,
        Walking,
        Jumping,
        Falling,
        TouchGround,
        Runnig,
        InBubble,
        LadderClimbing,

    }


}
