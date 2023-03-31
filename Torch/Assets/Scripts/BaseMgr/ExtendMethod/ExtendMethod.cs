using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 包含所有想要添加的拓展方法
/// </summary>
public static class ExtendMethod 
{

    /// <summary>
    /// 判断animator中有没有对应类型的参数
    /// </summary>
    /// <param name="self"></param>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool HasParameterOfType(this Animator self, string name, AnimatorControllerParameterType type)
    {
        if (name == null || name.Equals(""))
        {
            return false;
        }

        AnimatorControllerParameter[] parameters = self.parameters;
        foreach (AnimatorControllerParameter parameter in parameters)
        {
            if (parameter.type == type && parameter.name.Equals(name))
            {
                return true;
            }
        }

        return false;
    }
    
}
