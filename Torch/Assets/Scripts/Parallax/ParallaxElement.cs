using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class ParallaxElement : MonoBehaviour
{


    public bool MoveInOppositeDirection;
    public float HorizontalSpeed;
    public float VerticalSpeed;


    protected Camera _camera;
    protected PrallaxCamera _parallaxCamera;
    protected Transform _cameraTransform;
    protected Vector3 _speed;
    protected Vector3 _previousCameraPostion;
    protected bool _previousMoveParallax;


    private void OnEnable()
    {
        if (Camera.main == null)
        {
            return;
        }

        _camera = Camera.main;
        _parallaxCamera = _camera.GetComponent<PrallaxCamera>();
        _cameraTransform = _parallaxCamera.transform;

    }




    void LateUpdate()
    {
        MovePrallax();
    }

    /// <summary>
    /// 带有该脚本的元素 根据 摄像机的位置 进行视差移动 的地方上
    /// </summary>
    protected void MovePrallax()
    {
        if (_camera == null)
        {
            return;
        }

        if (!_parallaxCamera.MovePrallax)
        {
            return;
        }


        if (_parallaxCamera.MovePrallax && !_previousMoveParallax)
        {
            _previousCameraPostion = _cameraTransform.position;
        }

        _previousMoveParallax = _parallaxCamera.MovePrallax;

        _speed.x = HorizontalSpeed;
        _speed.y = VerticalSpeed;
        _speed.z = 0;


        Vector2 disffernet = _cameraTransform.position - _previousCameraPostion;

        float direction = (MoveInOppositeDirection) ? -1 : 1;

        transform.position += Vector3.Scale(disffernet, _speed) * direction;

        //transform.DOMove(newPos,HorizontalSpeed);
        //transform.Translate(Vector3.Scale(disffernet, _speed) * direction);

        _previousCameraPostion = _cameraTransform.position;

    }

}
