using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionInBubble : PlayerAblity
{

    public float jumpOutTime;
    public bool canJumpOutOfBubble;
    private Transform _bubbleTransform;
    private Rigidbody2D _rbody;
    private StateMachine<PlayerStates.MovementStates> _movement;
    private Jump _jump;
    private HorizontalMove _horizontalMove;

    public override void Initialization()
    {
        base.Initialization();
        AbilityPermitted = false;
        jumpOutTime = 1f;
        //EventMgr.GetInstance().AddLinstener<KeyCode>("GetKey", GetKey);
    }

    public override void GetComponents()
    {
        base.GetComponents();
        _rbody = GetComponent<Rigidbody2D>();
        _movement = _player.Movement;
        _jump = _player.gameObject.GetComponent<Jump>();
        _horizontalMove = _player.gameObject.GetComponent<HorizontalMove>();
    }

    public override void ProcessAbility()
    {
            KeepPostionInBubble();
            this.transform.eulerAngles = Vector3.zero;
    }

    public override void HandleInput()
    {
        if (_inputManager.JumpButton.State.CurrentState == InputHelper.ButtonState.ButtonDown)
        {
            GetOutBubble();
        }
    }


    private void GetKey(KeyCode key)
    {
        if (key == KeyCode.Space && _movement.CurrentState == PlayerStates.MovementStates.InBubble && canJumpOutOfBubble)
        {
            Debug.Log("bubble 按下了空格键");
            GetOutBubble();
        }
    }
    public void SetBubbleTransform(Transform transform)
    {
        if (transform != null)
        {
            _bubbleTransform = transform;
        }
    }

    private void KeepPostionInBubble()
    {
        _rbody.velocity = Vector2.zero;
        
    }


    /// <summary>
    /// 当MeI进来的时候计时1秒后才允许跳出去
    /// </summary>
    /// <returns></returns>
    private IEnumerator TimerJumpOut()
    {
        yield return new WaitForSeconds(jumpOutTime);
        canJumpOutOfBubble = true;
    }

    /// <summary>
    /// 进入bubble 要设置的一些参数
    /// </summary>
    public void GetInBubble(Transform bubble,Transform ferris)
    {
        StartCoroutine(TimerJumpOut());
        _horizontalMove.PermitAbility(false);
        _jump.PermitAbility(false);
        _bubbleTransform = bubble;
        this.transform.position = new Vector3(bubble.position.x, bubble.position.y - 0.15f,bubble.position.z);
        this.transform.SetParent(ferris,true);
        _player.Movement.ChangeState(PlayerStates.MovementStates.InBubble);
    }

    /// <summary>
    /// 离开bubble
    /// </summary>
    private void GetOutBubble()
    {
        this.transform.parent = null;
        _playerController.ResetRotation();
        _movement.ChangeState(PlayerStates.MovementStates.InBubble);
        _jump.PermitAbility(true);
        _horizontalMove.PermitAbility(true);
        PermitAbility(false);
    }
}
