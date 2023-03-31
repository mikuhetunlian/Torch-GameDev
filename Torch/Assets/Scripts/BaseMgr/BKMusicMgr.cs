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
    /// ≤•∑≈±≥æ∞“Ù¿÷
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
    /// Õ£÷π≤•∑≈“Ù¿÷
    /// </summary>
    public void StopBK()
    {
        audioSource.Stop();
    }



    /// <summary>
    /// …Ë÷√“Ù¡ø
    /// </summary>
    public  void SetVolumn()
    {
        audioSource.volume = (float)MusicVolumnMgr.GetInstance().VolumnValue /10;
    }




}
