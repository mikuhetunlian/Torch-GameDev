using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Death : PlayerAblity
{
    protected CinemachineBrain _barin;
    protected bool _isDeath;


    public override void Initialization()
    {
        base.Initialization();
    }

    /// <summary>
    /// ÿһ֡���һ���Լ���Ѫ����û�У�û�еĻ��ʹ�������
    /// </summary>
    public override void ProcessAbility()
    {
        //base.ProcessAbility();
        //if (_health.CurrentHealth == 0 && !_isDeath)
        //{
        //    DoWhenDeath();
        //}
    }


    protected void DoWhenDeath()
    {
        _isDeath = true;
        DisableAllAbility();
        GameObject blood = ResMgr.GetInstance().LoadRes<GameObject>("Prefab/leafBlood");
        blood.transform.position = this.gameObject.transform.position;
        CameraMgr.GetInstance().SetDefaultBlendType(CinemachineBlendDefinition.Style.Cut);

    }

    protected void DisableAllAbility()
    {
        
    }
}
