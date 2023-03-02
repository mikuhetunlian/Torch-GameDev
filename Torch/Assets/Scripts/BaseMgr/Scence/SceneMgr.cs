using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneMgr:BaseManager<SceneMgr>
{
    /// <summary>
    /// ͬ�����س���
    /// </summary>
    /// <param name="sceneName">������</param>
    /// <param name="action">������ɺ��callback</param>
    public void LoadScene(string sceneName, UnityAction action = null)
    {
        SceneManager.LoadScene(sceneName);

        if (action != null)
        {
            action();
        }
    }


    /// <summary>
    /// �첽���س���
    /// </summary>
    /// <param name="sceneName">������</param>
    /// <param name="action">������ɺ��callback</param>
    public void LoadSceneAsync(string sceneName, UnityAction action = null,float delayTime = 0)
    {
        MonoManager.GetInstance().StartCoroutine(DoLoadSceneAsync(sceneName, action , delayTime));
    }



    /// <summary>
    /// ����ִ���첽���س�����Э�̺���
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    private IEnumerator DoLoadSceneAsync(string sceneName, UnityAction action,float delayTime)
    {

      

        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
        while (!ao.isDone)
        {
            EventMgr.GetInstance().EventTrigger("loadsecneIng", ao.progress);
            yield return ao.progress;
        }


        Debug.Log("�ȴ�" + delayTime);
        yield return new WaitForSeconds(delayTime);

        Debug.Log("��ʼ����");

        if (action != null)
        {
            action();
        }
       
    }



}
