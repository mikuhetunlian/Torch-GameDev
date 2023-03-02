using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingeltonAutoManager<LevelManager>
{
    /// <summary>
    /// 玩家下一次重生的地点,在进入游戏的时候，会重新读取一次这个checkPoint
    /// </summary>
  
    public CheckPointData currentCheckPointData;
    protected CheckPoint currentCheckPoint;  
    /// 通过 GameObject.Find 来找到Player
    protected Player _player;
    protected string KEY_NAME = "currentCheckPointData";



    private void Start()
    {
        //Initialization();
    }

    protected void Initialization()
    {

        _player = GameObject.FindObjectOfType<Player>();
        //在初始加载时，读取上一个checkPoint
        currentCheckPointData = DataMgr.Instance.Load(typeof(CheckPointData), KEY_NAME) as CheckPointData;
        if (currentCheckPointData == null)
        {
            currentCheckPointData.checkPointName = "CheckPoint1";
        }
        GameObject obj  = GameObject.Find(currentCheckPointData.checkPointName).gameObject;
        currentCheckPoint = obj.GetComponent<CheckPoint>();


        //这里因为rainhandle很特殊所以找不到的话单独找一次，后面有优化再弄这里
        if (currentCheckPoint == null)
        {
            currentCheckPoint = GameObject.Find(currentCheckPointData.checkPointName).gameObject.GetComponent<RainHandleCheckPoint>();
        }
        Debug.Log("目前的checkPoint是" + (DataMgr.Instance.Load(typeof(CheckPointData), KEY_NAME) as CheckPointData).checkPointName);
    }

    /// <summary>
    /// 当角色需要从重生的时候调用这个方法
    /// </summary>
    public void RespawnPlayer()
    {
        if (currentCheckPointData == null)
        {
            return;
        }
        currentCheckPoint.SpawnPlayer(_player);
    }


    /// <summary>
    /// 修改目前的 checkPoint，并用DataMgr存储
    /// </summary>
    /// <param name="checkPoint"></param>
    public void SetCurrentCheckPoint(CheckPoint checkPoint)
    {
        currentCheckPoint = checkPoint;
        currentCheckPointData = currentCheckPoint.checkPointData;
        //用DataMgr存储 currentCheckPoint 中的 checkPointData
        DataMgr.Instance.Save(currentCheckPointData, KEY_NAME);
        Debug.Log("目前的checkPoint是" + (DataMgr.Instance.Load(typeof(CheckPointData), KEY_NAME) as CheckPointData).checkPointName);
    }

}
