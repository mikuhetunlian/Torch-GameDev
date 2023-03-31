using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicVolumnMgr : BaseManager<MusicVolumnMgr>
{
    /// <summary>
    /// ȫ�������� 0-10֮�䣬ֻΪ����
    /// </summary>
    public int VolumnValue;


     public void AddVolumn()
     {
        VolumnValue = VolumnValue + 1 > 10 ? 10 : VolumnValue + 1;
     }


    public void DeleteVolumn()
    {
        VolumnValue = VolumnValue - 1 < 0 ? 0 : VolumnValue - 1;
    }

    public void SetVolumn(int volumn)
    {
        VolumnValue = (int)Mathf.Ceil(volumn);
    }
}
