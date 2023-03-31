using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicMgr : BaseManager<MusicMgr>
{
    //播放背景音乐的AudioSource
    public AudioSource bkMusic;
    //背景音乐的大小 0-1之间
    public float bkValue;

    //在场景中被挂载音效们的GameObject
    private GameObject efMusicObj;
    //音效链
    private List<AudioSource> sourceList;
    //音效的大小 0-1之间
    public float efValue = 10;



    public MusicMgr()
    {
        if (sourceList == null)
        {
            sourceList = new List<AudioSource>();
        }

        //添加到Mono的Update中执行
        MonoManager.GetInstance().AddUpdateLinstener(Update);
    }

    /// <summary>
    /// 每一帧看一下是否有音效播放完毕，如果有，从efMusicObj上移除该AudioSource组件
    /// 由Mono管理模块代理执行
    /// </summary>
    private void Update()
    {
        for (int i = sourceList.Count - 1; i >= 0; i--)
        {
            if (!sourceList[i].isPlaying)
            {
                //如果该特效模仿完毕，从efMusicObj中移除对应的音效AudioSource组件
                GameObject.Destroy(sourceList[i]);
                sourceList.RemoveAt(0);
            }
        }
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="musicName">背景音乐名</param>
    public void PlayBkMusic(string musicName,bool isLoop)
    {
        if (bkMusic == null)
        {
            GameObject obj = new GameObject("bkMusic");
            bkMusic = obj.AddComponent<AudioSource>();
        }

        //异步加载资源
        ResMgr.GetInstance().LoadResAsync<AudioClip>("Music/bkMusic/" + musicName, (clip) =>
         {
             bkMusic.clip = clip;
             bkMusic.volume = bkValue;
             bkMusic.loop = isLoop;
             bkMusic.Play();
         });

    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="clip">背景音乐的clip</param>
    public void PlayBkMusic(AudioClip clip, bool isLoop)
    {
        if (bkMusic == null)
        {
            GameObject obj = new GameObject("bkMusic");
            bkMusic = obj.AddComponent<AudioSource>();
        }

        bkMusic.clip = clip;
        bkMusic.volume = bkValue;
        bkMusic.loop = isLoop;
        bkMusic.Play();
    }


    /// <summary>
    /// 终止播放背景音乐
    /// </summary>
    public void StopBkMusic()
    {
        if (bkMusic != null)
        {
            bkMusic.Stop();
        }
    }

    /// <summary>
    /// 暂停播放背景音乐
    /// </summary>
    public void PauseBkMusic()
    {
        if (bkMusic != null)
        {
            bkMusic.Pause();
        }
    }


    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="effectName">音效名字</param>
    /// <param name="isLoop">音效是否循环</param>
    /// <param name="callback">加载完音效后的回调函数，默认为null</param>
    public void PlayMusicEffect(string effectName, bool isLoop, UnityAction<AudioSource> callback = null)
    {
        if (efMusicObj == null)
        {
            GameObject obj = new GameObject("effectMusic");
            efMusicObj = obj;
        }

        //异步加载音效
        ResMgr.GetInstance().LoadResAsync<AudioClip>("Music/effectMusic/" + effectName, (clip) =>
         {
             AudioSource efMusic = efMusicObj.AddComponent<AudioSource>();
             sourceList.Add(efMusic);
             efMusic.clip = clip;
             efMusic.volume = efValue;
             efMusic.loop = isLoop;
             efMusic.Play();
             if (callback != null)
             {
                 callback(efMusic);
             }
         });

    }


    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="audioClip">音效的effect</param>
    /// <param name="isLoop">是否循环</param>
    public void PlayMusicEffect(AudioClip audioClip,bool isLoop)
    {
        if (efMusicObj == null)
        {
            GameObject obj = new GameObject("effectMusic");
            efMusicObj = obj;
        }

        AudioSource efMusic = efMusicObj.AddComponent<AudioSource>();
        efMusic.clip = audioClip;
        efMusic.volume = efValue;
        efMusic.loop = isLoop;
        efMusic.Play();
    }


    /// <summary>
    /// 暂停某个音效播放
    /// </summary>
    /// <param name="sourse"></param>
    public void PauseMusicEffect(AudioSource sourse)
    {
        if (sourceList.Contains(sourse))
        {
            sourse.Pause();
        }
      
    }

    /// <summary>
    /// 停止某个音效
    /// </summary>
    /// <param name="sourse"></param>
    public void StopMusicEffect(AudioSource sourse)
    {
        if (sourceList.Contains(sourse))
        {
            sourceList.Remove(sourse);
            sourse.Stop();
            GameObject.Destroy(sourse);
        }
    }

    /// <summary>
    /// 设定背景音量
    /// </summary>
    /// <param name="bkValue"></param>
    public void SetBkValue(float bkValue)
    {
        this.bkValue = bkValue;
    }

    /// <summary>
    /// 设定声效音量
    /// </summary>
    /// <param name="effectValue"></param>
    public void SetEffectValue(float effectValue)
    {
        efValue = effectValue;
    }


}
