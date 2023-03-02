using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ����Animator��helper��
/// </summary>
public class AnimatorHelper 
{

    //���animator����������͵Ĳ����Ļ�������ӵ�paramaterList��
    public static void AddAnimatorParamaterIfExists(Animator animator,string paramaterName,AnimatorControllerParameterType type,HashSet<int> paramaters)
    {
        if (animator.HasParameterOfType(paramaterName, type))
        {
            paramaters.Add(Animator.StringToHash(paramaterName));
        }
    }
    

    /// <summary>
    /// ���parameterList�Ƿ��� parameterName �������������У�����ÿһ��loop��animator�еĲ�����Ϊvalue
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
