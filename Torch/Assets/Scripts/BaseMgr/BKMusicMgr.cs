using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKMusicMgr : SingeltonAutoManager<BKMusicMgr>
{

    protected AudioSource audioSource;

    private void Awake()
    {
        audioSource =  this.gameObject.AddComponent<AudioSource>();
    }

    /// <summary>
    /// ���ű�������
    /// </summary>
    /// <param name="clip"></param>
    public void PlayBK(AudioClip clip,bool isLoop)
    {
        SetVolumn();
        audioSource.clip = clip;
        audioSource.loop = isLoop;
        audioSource.Play();
    }

    /// <summary>
    /// ֹͣ��������
    /// </summary>
    public void StopBK()
    {
        audioSource.Stop();
    }



    /// <summary>
    /// ��������
    /// </summary>
    public  void SetVolumn()
    {
        audioSource.volume = (float)MusicVolumnMgr.GetInstance().VolumnValue /10;
    }




}
