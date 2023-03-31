using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handle : MonoBehaviour
{

    public Transform slicePoint;
    public float maxHeight;
    public float minHeight;
    public float speed;

    protected BoxCollider2D _boxCollider;
    

    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();    
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            Debug.Log("Stay");
            if(Input.GetKey(KeyCode.W))
            {
                MoveSlicePoint(Vector2.up, speed);
            }

            if (Input.GetKey(KeyCode.S))
            {
                MoveSlicePoint(Vector2.down, speed);
            }

            if (InputManager.GetInstance().ControlButton.State.CurrentState == InputHelper.ButtonState.ButtonDown)
            {
                EventMgr.GetInstance().EventTrigger<bool>("Slice", true);
            }
        }
    }

    protected void MoveSlicePoint(Vector2 dir,float speed)
    {
        
        slicePoint.Translate(dir * speed * Time.deltaTime);
        if (slicePoint.position.y > maxHeight)
        {
            slicePoint.position = new Vector3(slicePoint.position.x,maxHeight, slicePoint.position.z);
        }
        if (slicePoint.position.y < minHeight)
        {
            slicePoint.position = new Vector3(slicePoint.position.x, minHeight, slicePoint.position.z);
        }
    }

    
}
