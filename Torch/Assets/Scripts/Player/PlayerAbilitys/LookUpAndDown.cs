using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LookUpAndDown : PlayerAblity
{

    private enum LookDirection
    {
        Up,
        Down
    }

    private CinemachineVirtualCamera _mainVCamera;
    private CinemachineFramingTransposer _transposer;
    public bool isLookUp;
    public bool isLookDown;
    ///触发所需要按下的时间
    public float pressTime;
    ///距离中心点的偏移位置
    public float offset;
    ///是否在计时中
    private bool _isTiming;

    public override void Initialization()
    {
        base.Initialization();
        pressTime = 1;
        offset = 0.2f;
        EventMgr.GetInstance().AddLinstener<KeyCode>("GetKey", GetKey);
        EventMgr.GetInstance().AddLinstener<KeyCode>("GetKeyUp", GetKeyUp);
    }



    public override void GetComponents()
    {
        base.GetComponents();
        _mainVCamera = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        _transposer = _mainVCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    public override void ProcessAbility()
    {
        base.ProcessAbility();
    }


    private void GetKey(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.W:
                if (!_isTiming && _mainVCamera.enabled
                    && _movement.CurrentState != PlayerStates.MovementStates.LadderClimbing)
                {
                    StartCoroutine(LookUpOrDown(LookDirection.Up));
                }
                break;
            case KeyCode.S:
                if (!_isTiming && _mainVCamera.enabled
                    && _movement.CurrentState != PlayerStates.MovementStates.LadderClimbing)
                {
                    StartCoroutine(LookUpOrDown(LookDirection.Down));
                }  
                break;
        }
    }


    private void GetKeyUp(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.W:
                if (_isTiming)
                {
                    _isTiming = false;
                }
                if (isLookUp)
                {
                    RecoverSight();
                }

                break;
            case KeyCode.S:
                if (_isTiming)
                {
                    _isTiming = false;
                }
                if (isLookDown)
                {
                    RecoverSight();
                }
                break;
        }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    private IEnumerator LookUpOrDown(LookDirection direction)
    {
       
        _isTiming = true;
        float t = 0;
        while (t <= pressTime)
        {
            if (!_isTiming)
            {
                yield break;
            }
            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }


        if (!_mainVCamera.enabled)
        {
            yield break;
        }

        if (direction == LookDirection.Up)
        {
            _transposer.m_ScreenY += offset;
            isLookUp = true;
        }
        else
        {
            _transposer.m_ScreenY -= offset;
            isLookDown = true;
        }
    }

    private void RecoverSight()
    {
        _transposer.m_ScreenY = 0.5f;
        isLookUp = false;
    }
}
