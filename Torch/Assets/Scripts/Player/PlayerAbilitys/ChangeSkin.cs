using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.Events;

public class ChangeSkin : PlayerAblity
{

    private SkeletonMecanim _skeletonMecanim;
    private Skeleton _skeleton;

    public override void GetComponents()
    {
        
        base.GetComponents();
        _skeletonMecanim = GetComponent<SkeletonMecanim>();
        _skeleton = _skeletonMecanim.skeleton;
    }

    public override void Initialization()
    {
        base.Initialization();
    }


    /// <summary>
    /// �ı��ɫ��spine skin
    /// </summary>
    /// <param name="skinName">Ƥ���������Ƥ����spine�ﴴ����ʱ�����ļ��еĻ���Ƥ������ �ļ�����+Ƥ���� </param>
    /// <param name="callback">�ı���Ƥ����Ļص���Ĭ��Ϊ��</param>
    public void ChangeSkeletonSkin(string skinName,UnityAction callback = null)
    {
        if (_skeleton == null)
        {
            return;
        }
        _skeleton.SetSkin(skinName);
        callback?.Invoke();
    }


}
