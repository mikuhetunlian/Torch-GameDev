using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingeltonAutoManager<LevelManager>
{
    /// <summary>
    /// �����һ�������ĵص�,�ڽ�����Ϸ��ʱ�򣬻����¶�ȡһ�����checkPoint
    /// </summary>
  
    public CheckPointData currentCheckPointData;
    protected CheckPoint currentCheckPoint;  
    /// ͨ�� GameObject.Find ���ҵ�Player
    protected Player _player;
    protected string KEY_NAME = "currentCheckPointData";



    private void Start()
    {
        //Initialization();
    }

    protected void Initialization()
    {

        _player = GameObject.FindObjectOfType<Player>();
        //�ڳ�ʼ����ʱ����ȡ��һ��checkPoint
        currentCheckPointData = DataMgr.Instance.Load(typeof(CheckPointData), KEY_NAME) as CheckPointData;
        if (currentCheckPointData == null)
        {
            currentCheckPointData.checkPointName = "CheckPoint1";
        }
        GameObject obj  = GameObject.Find(currentCheckPointData.checkPointName).gameObject;
        currentCheckPoint = obj.GetComponent<CheckPoint>();


        //������Ϊrainhandle�����������Ҳ����Ļ�������һ�Σ��������Ż���Ū����
        if (currentCheckPoint == null)
        {
            currentCheckPoint = GameObject.Find(currentCheckPointData.checkPointName).gameObject.GetComponent<RainHandleCheckPoint>();
        }
        Debug.Log("Ŀǰ��checkPoint��" + (DataMgr.Instance.Load(typeof(CheckPointData), KEY_NAME) as CheckPointData).checkPointName);
    }

    /// <summary>
    /// ����ɫ��Ҫ��������ʱ������������
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
    /// �޸�Ŀǰ�� checkPoint������DataMgr�洢
    /// </summary>
    /// <param name="checkPoint"></param>
    public void SetCurrentCheckPoint(CheckPoint checkPoint)
    {
        currentCheckPoint = checkPoint;
        currentCheckPointData = currentCheckPoint.checkPointData;
        //��DataMgr�洢 currentCheckPoint �е� checkPointData
        DataMgr.Instance.Save(currentCheckPointData, KEY_NAME);
        Debug.Log("Ŀǰ��checkPoint��" + (DataMgr.Instance.Load(typeof(CheckPointData), KEY_NAME) as CheckPointData).checkPointName);
    }

}
