using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
using DG.Tweening;

public class CameraMgr : BaseManager<CameraMgr>
{
    ///跟随玩家的虚拟摄像机
    public CinemachineVirtualCamera PlayerVirtualCamera { get;  set; }
    public Vector3 DefaultOffset = new Vector2(0, 7.6f);
    private CinemachineBrain _brain;
    protected CinemachineVirtualCamera _currentActiveCamera;
    protected CinemachineVirtualCamera _previousActiveCamera;

    public CameraMgr()
    {
        _brain = Camera.main.gameObject.GetComponent<CinemachineBrain>();
        //PlayerVirtualCamera = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        //_previousActiveCamera = _brain.ActiveVirtualCamera as CinemachineVirtualCamera;



    }

    /// <summary>
    /// 或许当前被激活的VirtualCamera
    /// </summary>
    /// <returns></returns>
    public CinemachineVirtualCamera GetCurrentActiveCamera()
    {
        CinemachineVirtualCamera currentCamera = (CinemachineVirtualCamera)_brain.ActiveVirtualCamera;
        return currentCamera;
    }

    /// <summary>
    /// 设置cinemachine的 OrthoSize
    /// </summary>
    /// <param name="newOrthoSize">想要设置的新的值</param>
    /// <param name="durationTime">在多少秒之内设置完成</param>
    public void SetOrthoSize(float newOrthoSize,float durationTime)
    {
        CinemachineVirtualCamera currentCamera = GetCurrentActiveCamera();
        if (currentCamera != null)
        {
            DOVirtual.Float(currentCamera.m_Lens.OrthographicSize, newOrthoSize, durationTime, (changedOrthoSize) =>
            {
                currentCamera.m_Lens.OrthographicSize = changedOrthoSize;
            });
        }
    }

    /// <summary>
    /// 设置cinemachine的默认混合时间
    /// </summary>
    public void SetDefaultBlednTime(float blendTime)
    {
        if (_brain == null)
        {
            return;
        }

        _brain.m_DefaultBlend.m_Time = blendTime;
    }

    /// <summary>
    /// 设置cinemachine的followTarget
    /// </summary>
    /// <param name="followTarget">想要设置的followTarget</param>
    public void ChangeFollow(Transform followTarget)
    {
        CinemachineVirtualCamera currentCamera = GetCurrentActiveCamera();
        currentCamera.Follow = followTarget;
    }



    /// <summary>
    /// 重置当前镜头的 offset 为 默认镜头
    /// </summary>
    public void ResetCurrnteCameraOffset()
    {
        CinemachineVirtualCamera currentCamera = _brain.ActiveVirtualCamera as CinemachineVirtualCamera;
        CinemachineFramingTransposer transporser = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        MonoManager.GetInstance().StartCoroutine(DoSetCurrentCameraOffset(transporser, DefaultOffset));

    }

    /// <summary>
    /// 设置当前镜头的 offsetX
    /// </summary>
    /// <param name="y"></param>
    public void SetCurrentCameraOffsetX(float x)
    {
        CinemachineVirtualCamera currentCamera = _brain.ActiveVirtualCamera as CinemachineVirtualCamera;
        CinemachineFramingTransposer transporser = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        MonoManager.GetInstance().StartCoroutine(DoSetCurrentCameraOffset(transporser, new Vector2(x, transporser.m_TrackedObjectOffset.y)));
    }

    /// <summary>
    /// 设置当前镜头的 offsetY
    /// 设置当前镜头的 offsetY
    /// </summary>
    /// <param name="y"></param>
    public void SetCurrentCameraOffsetY(float y)
    {
        CinemachineVirtualCamera currentCamera = _brain.ActiveVirtualCamera as CinemachineVirtualCamera;
        CinemachineFramingTransposer transporser = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        MonoManager.GetInstance().StartCoroutine(DoSetCurrentCameraOffset(transporser, new Vector2(transporser.m_TrackedObjectOffset.x, y)));
    }

    /// <summary>
    /// 实现镜头偏移的协程
    /// </summary>
    /// <param name="transporser"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    IEnumerator DoSetCurrentCameraOffset(CinemachineFramingTransposer transporser, Vector2 offset)
    {
        float t = 0;
        Vector3 originPoint = transporser.m_TrackedObjectOffset;
        while (t <= 1)
        {
            transporser.m_TrackedObjectOffset = Vector3.Lerp(originPoint, offset, t);
            t += 0.05f;
            yield return null;
        }

    }


    /// <summary>
    /// 切换摄像机
    /// </summary>
    /// <param name="targetCamera">要切换到的目标摄像机</param>
    /// <param name="blendTime">过度时间，-1为默认不修改过度时间</param>
    /// <param name="action">后续行为</param>
    public void ChangeCamera(CinemachineVirtualCamera targetCamera, float blendTime = -1, UnityAction action = null)
    {
        if (blendTime != -1 && blendTime >= 0)
        {
            SetDefaultBlednTime(blendTime);
        }
        _previousActiveCamera = _brain.ActiveVirtualCamera as CinemachineVirtualCamera;
        _previousActiveCamera.enabled = false;
        targetCamera.enabled = true;

        action?.Invoke();
    }

    /// <summary>
    ///设置默认的镜头切换模式
    /// </summary>
    public void SetDefaultBlendType(CinemachineBlendDefinition.Style style)
    {
        CinemachineBlendDefinition blendDefinition = _brain.m_DefaultBlend;
        blendDefinition.m_Style = style;
        _brain.m_DefaultBlend = blendDefinition;
    }

    /// <summary>
    /// 设置当前摄像机的跟随阻尼
    /// </summary>
    /// <param name="damping"></param>
    public void SetDamping(Vector2 damping)
    {
        CinemachineVirtualCamera camera  = _brain.ActiveVirtualCamera as CinemachineVirtualCamera;
        CinemachineFramingTransposer ft =  camera.GetCinemachineComponent<CinemachineFramingTransposer>();
        ft.m_XDamping = damping.x;
        ft.m_YDamping = damping.y;
    }


    /// <summary>
    /// 设置 DeadZone 的尺寸
    /// </summary>
    /// <param name="deathZoneSize"></param>
    public void SetDeathZone(float width,float height)
    {
        CinemachineVirtualCamera camera = _brain.ActiveVirtualCamera as CinemachineVirtualCamera;
        CinemachineFramingTransposer ft = camera.GetCinemachineComponent<CinemachineFramingTransposer>();
        ft.m_DeadZoneHeight = height;
        ft.m_DeadZoneWidth = width;
    }
}
