using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    ///��ͬ��������ʽ
    public enum DeathStyle {DeathWithFlower,DeathwithoutFlower }
    ///Ŀǰ��Ѫ��
    [ReadOnly]
    public int CurrentHealth;

    ///��ʼѪ��
    [Header("Health")]
    public int InitiaHeath;
    ///���Ѫ��
    public int MaximumHealth;
    protected Player _player;


    private void Start()
    {
        Initialization();
    }

    public void Initialization()
    {
        CurrentHealth = 10;
        InitiaHeath = 10;
        MaximumHealth = 10;
        _player = GetComponent<Player>();
    }

    /// <summary>
    /// <summary>
    /// �������ش�����ʱ�������������������player��Ѫ��
    /// <typeparam name="T">����ʱ���¼��ص��Ĳ�������</typeparam>
    /// <param name="damge">�������ش�����ʱ�������������������player��Ѫ��</param>
    /// <param name="respawnCallbackName">����ʱ���¼��ص�</param>
    /// <param name="deathStyle">�ܵ��˺���������ˣ�ѡ�������ķ��</param>
    /// <param name="delayTime"></param>
    public void Damage(int damge,DeathStyle deathStyle = DeathStyle.DeathWithFlower,UnityAction respawnCallback = null,float delayTime = 1)
    {
        if (CurrentHealth < 0)
        {
            return;
        }
        CurrentHealth -= damge;

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            if (deathStyle == DeathStyle.DeathWithFlower)
            {
                StartCoroutine(DeathWithFlower(respawnCallback));
            }
            else if (deathStyle == DeathStyle.DeathwithoutFlower)
            {
                StartCoroutine(DeathWithoutFlower(respawnCallback,delayTime));
            }

        }
    }


    /// <summary>
    /// ��Ѫ��<0ʱ��ִ��DeathWithFlower
    /// </summary>
    public IEnumerator DeathWithFlower(UnityAction respawnCallback)
    {
        Debug.Log("player kill");

        GameObject blood = GameObject.Instantiate(Resources.Load("Prefab/leafBlood")) as GameObject;
            /*ResMgr.GetInstance().LoadRes<GameObject>("Prefab/leafBlood")*/;
        blood.transform.position = this.gameObject.transform.position;
        //���blood�Ķ�������
        Animator  animator = blood.GetComponent<Animator>();
        AnimatorClipInfo[] infos  = animator.GetCurrentAnimatorClipInfo(0);
        float delayTime = 0;
        for (int i = 0; i < infos.Length; i++)
        {
            delayTime = infos[i].clip.length;
        }
        delayTime += 0.5f;

        CameraMgr.GetInstance().SetDefaultBlendType(CinemachineBlendDefinition.Style.Cut);
        
        InputManager.GetInstance().InputDetectionActive = false;
        _player.ToTransparency();
        yield return new WaitForSeconds(delayTime);
        LevelManager.GetInstance().RespawnPlayer();
        ResetHealth();
        _player.ToVisiable();
        CameraMgr.GetInstance().SetDefaultBlendType(CinemachineBlendDefinition.Style.EaseInOut);
        InputManager.GetInstance().InputDetectionActive = true;
        respawnCallback?.Invoke();
    }

    public IEnumerator DeathWithoutFlower(UnityAction respawnCallback,float delay)
    {
        Debug.Log("player kill");
        float delayTime = delay;
        CameraMgr.GetInstance().SetDefaultBlendType(CinemachineBlendDefinition.Style.Cut);
        InputManager.GetInstance().InputDetectionActive = false;
        _player.ToTransparency();
        yield return new WaitForSeconds(delayTime);
        LevelManager.GetInstance().RespawnPlayer();
        ResetHealth();
        _player.ToVisiable();
        CameraMgr.GetInstance().SetDefaultBlendType(CinemachineBlendDefinition.Style.EaseInOut);
        InputManager.GetInstance().InputDetectionActive = true;
        respawnCallback?.Invoke();
    }

    /// <summary>
    /// ����Ѫ��
    /// </summary>
    public void ResetHealth()
    {
        CurrentHealth = MaximumHealth;
    }

}
