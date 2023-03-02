using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    ///不同的死的形式
    public enum DeathStyle {DeathWithFlower,DeathwithoutFlower }
    ///目前的血量
    [ReadOnly]
    public int CurrentHealth;

    ///初始血量
    [Header("Health")]
    public int InitiaHeath;
    ///最大血量
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
    /// 当被机关触碰到时，调用这个函数来减少player的血量
    /// <typeparam name="T">复活时的事件回调的参数类型</typeparam>
    /// <param name="damge">当被机关触碰到时，调用这个函数来减少player的血量</param>
    /// <param name="respawnCallbackName">复活时的事件回调</param>
    /// <param name="deathStyle">受到伤害如果死亡了，选择死亡的风格</param>
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
    /// 当血量<0时，执行DeathWithFlower
    /// </summary>
    public IEnumerator DeathWithFlower(UnityAction respawnCallback)
    {
        Debug.Log("player kill");

        GameObject blood = GameObject.Instantiate(Resources.Load("Prefab/leafBlood")) as GameObject;
            /*ResMgr.GetInstance().LoadRes<GameObject>("Prefab/leafBlood")*/;
        blood.transform.position = this.gameObject.transform.position;
        //获得blood的动画长度
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
    /// 重置血量
    /// </summary>
    public void ResetHealth()
    {
        CurrentHealth = MaximumHealth;
    }

}
