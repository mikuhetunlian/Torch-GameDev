using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch : PlayerAblity
{
    public bool CanTouch;
    public bool TryToTouch;
    public bool TryToTouching;

    protected string _tryToTouchAnimatorParameterName = "tryToTouch";
    protected int _tryToTouchAnimatorParameter;
    protected string _tryToTouchingAnimatorParameterName = "tryToTouching";
    protected int _tryToTouchingAnimatorParameter;


    /// <summary>
    ///给 touch trigger 修改这里的状态
    /// </summary>
    public void SetTouchIn()
    {
        CanTouch = true;
        TryToTouch = true;
        TryToTouching = false;
    }

    public void SetTouchOut()
    {
        ResetAnimatorParameter();
    }

    protected void ResetAnimatorParameter()
    {
        TryToTouch = false;
        TryToTouching = false;
        CanTouch = false;
    }

    protected override void InitializeAnimatorParameter()
    {
        RegisterAnimatorParameter(_tryToTouchAnimatorParameterName, AnimatorControllerParameterType.Bool, out _tryToTouchAnimatorParameter);
        RegisterAnimatorParameter(_tryToTouchingAnimatorParameterName, AnimatorControllerParameterType.Bool, out _tryToTouchingAnimatorParameter);
    }

    public override void UpdateAnimator()
    {
        if (_touch.CanTouch && TryToTouch)
        {
            AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);
            if (info.normalizedTime >= 1)
            {
                TryToTouch = false;
                TryToTouching = true;
                AnimatorHelper.UpdateAnimatorBool(_animator, _tryToTouchingAnimatorParameter, true, _player._animatorParameters);
            }
            AnimatorHelper.UpdateAnimatorBool(_animator, _tryToTouchAnimatorParameter, true, _player._animatorParameters);
        }
        else
        {
            AnimatorHelper.UpdateAnimatorBool(_animator, _tryToTouchAnimatorParameter, false, _player._animatorParameters);
        }

        if (!TryToTouching)
        {
            AnimatorHelper.UpdateAnimatorBool(_animator, _tryToTouchingAnimatorParameter, false, _player._animatorParameters);
        }


    }


}
