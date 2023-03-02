using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainHandleCheckPoint : CheckPoint
{
    protected bool canbeTrigger;
    
    protected override void Awake()
    {
        base.Awake();
        EventMgr.GetInstance().AddLinstener<string>("RainHandleCheckPointTrigger", RainHandleCheckPointTrigger);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") && canbeTrigger)
        {
            LevelManager.GetInstance().SetCurrentCheckPoint(this);
        }
    }

    public void RainHandleCheckPointTrigger(string info)
    {
        canbeTrigger = true;
    }
}
