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
    /// 改变角色的spine skin
    /// </summary>
    /// <param name="skinName">皮肤名，如果皮肤在spine里创建的时候有文件夹的话，皮肤名是 文件夹名+皮肤名 </param>
    /// <param name="callback">改变完皮肤后的回调，默认为空</param>
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
