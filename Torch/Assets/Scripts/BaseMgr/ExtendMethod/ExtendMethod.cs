using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����������Ҫ��ӵ���չ����
/// </summary>
public static class ExtendMethod 
{

    /// <summary>
    /// �ж�animator����û�ж�Ӧ���͵Ĳ���
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
