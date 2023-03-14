using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZone : MonoBehaviour
{

    public CinemachineVirtualCamera zoneVirturalCamera;

    [Header("Gizmos")]
    public bool isDrawGizmos;
    public Color GizmosColor;
    public BoxCollider2D _boxCollider;

    protected Vector3 _gizmoSize;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            zoneVirturalCamera.enabled = true;
        }
    }


    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            zoneVirturalCamera.enabled = false;
        }
    }



    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (isDrawGizmos)
        {
            Gizmos.color = GizmosColor;

            _gizmoSize.x = _boxCollider.bounds.size.x;
            _gizmoSize.y = _boxCollider.bounds.size.y;
            _gizmoSize.z = 1.0f;

            Gizmos.DrawCube(_boxCollider.bounds.center, _gizmoSize);
        }
    }
    #endif
}
