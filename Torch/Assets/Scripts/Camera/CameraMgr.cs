using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
using DG.Tweening;

public class CameraMgr : BaseManager<CameraMgr>
{
    ///������ҵ����������
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
    /// ����ǰ�������VirtualCamera
    /// </summary>
    /// <returns></returns>
    public CinemachineVirtualCamera GetCurrentActiveCamera()
    {
        CinemachineVirtualCamera currentCamera = (CinemachineVirtualCamera)_brain.ActiveVirtualCamera;
        return currentCamera;
    }

    /// <summary>
    /// ����cinemachine�� OrthoSize
    /// </summary>
    /// <param name="newOrthoSize">��Ҫ���õ��µ�ֵ</param>
    /// <param name="durationTime">�ڶ�����֮���������</param>
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
    /// ����cinemachine��Ĭ�ϻ��ʱ��
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
    /// ����cinemachine��followTarget
    /// </summary>
    /// <param name="followTarget">��Ҫ���õ�followTarget</param>
    public void ChangeFollow(Transform followTarget)
    {
        CinemachineVirtualCamera currentCamera = GetCurrentActiveCamera();
        currentCamera.Follow = followTarget;
    }



    /// <summary>
    /// ���õ�ǰ��ͷ�� offset Ϊ Ĭ�Ͼ�ͷ
    /// </summary>
    public void ResetCurrnteCameraOffset()
    {
        CinemachineVirtualCamera currentCamera = _brain.ActiveVirtualCamera as CinemachineVirtualCamera;
        CinemachineFramingTransposer transporser = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        MonoManager.GetInstance().StartCoroutine(DoSetCurrentCameraOffset(transporser, DefaultOffset));

    }

    /// <summary>
    /// ���õ�ǰ��ͷ�� offsetX
    /// </summary>
    /// <param name="y"></param>
    public void SetCurrentCameraOffsetX(float x)
    {
        CinemachineVirtualCamera currentCamera = _brain.ActiveVirtualCamera as CinemachineVirtualCamera;
        CinemachineFramingTransposer transporser = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        MonoManager.GetInstance().StartCoroutine(DoSetCurrentCameraOffset(transporser, new Vector2(x, transporser.m_TrackedObjectOffset.y)));
    }

    /// <summary>
    /// ���õ�ǰ��ͷ�� offsetY
    /// ���õ�ǰ��ͷ�� offsetY
    /// </summary>
    /// <param name="y"></param>
    public void SetCurrentCameraOffsetY(float y)
    {
        CinemachineVirtualCamera currentCamera = _brain.ActiveVirtualCamera as CinemachineVirtualCamera;
        CinemachineFramingTransposer transporser = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        MonoManager.GetInstance().StartCoroutine(DoSetCurrentCameraOffset(transporser, new Vector2(transporser.m_TrackedObjectOffset.x, y)));
    }

    /// <summary>
    /// ʵ�־�ͷƫ�Ƶ�Э��
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
    /// �л������
    /// </summary>
    /// <param name="targetCamera">Ҫ�л�����Ŀ�������</param>
    /// <param name="blendTime">����ʱ�䣬-1ΪĬ�ϲ��޸Ĺ���ʱ��</param>
    /// <param name="action">������Ϊ</param>
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
    ///����Ĭ�ϵľ�ͷ�л�ģʽ
    /// </summary>
    public void SetDefaultBlendType(CinemachineBlendDefinition.Style style)
    {
        CinemachineBlendDefinition blendDefinition = _brain.m_DefaultBlend;
        blendDefinition.m_Style = style;
        _brain.m_DefaultBlend = blendDefinition;
    }

    /// <summary>
    /// ���õ�ǰ������ĸ�������
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
    /// ���� DeadZone �ĳߴ�
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
