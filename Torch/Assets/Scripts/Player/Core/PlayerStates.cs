using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用来表明当前player所处的状态是什么
/// </summary>
public class PlayerStates
{
    public enum PlayerConditions
    {
        Normal,// 无操作
        Move,// 移动中
        Forzen,// 被冰冻
        Dead,// 死亡
        Invincibility,// 无敌状态
        Fight,// 战斗状态





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
