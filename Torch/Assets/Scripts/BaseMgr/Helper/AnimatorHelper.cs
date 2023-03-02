using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 设置Animator的helper类
/// </summary>
public class AnimatorHelper 
{

    //如果animator中有这个类型的参数的话，就添加到paramaterList中
    public static void AddAnimatorParamaterIfExists(Animator animator,string paramaterName,AnimatorControllerParameterType type,HashSet<int> paramaters)
    {
        if (animator.HasParameterOfType(paramaterName, type))
        {
            paramaters.Add(Animator.StringToHash(paramaterName));
        }
    }
    

    /// <summary>
    /// 检查parameterList是否含有 parameterName 这个参数，如果有，就在每一个loop将animator中的参数设为value
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="parameterName"></param>
    /// <param name="value"></param>
    /// <param name="parameterList"></param>
    public static void UpdateAnimatorBool(Animator animator, int parameter, bool value, HashSet<int> parameters)
    {
        if (animator == null)
        {
            return;
        }

        if (parameters.Contains(parameter))
        {
            animator.SetBool(parameter, value);
        }

    }





    
}
