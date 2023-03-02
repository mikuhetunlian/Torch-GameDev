using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTriggerBase : MonoBehaviour
{
    ///���������䰵������ͼƬ
    public  SpriteRenderer[] BeDrakScenceMasks;
    ///�Լ�������͸���ĵ�����ͼƬ
    public  SpriteRenderer[] BeTranspaencyScenceMasks;
    ///����trigger������б�
    public List<GameObject> ActiveListWhenIn;
    public bool IsCameraOutsideBlack;

    protected BoxCollider2D cameraTriggerBox;
    protected string _VCameraName;
    protected CinemachineVirtualCamera _VCamera;
    protected float _inBlendTime = 2;
    protected float _exitBlednTime = 2;

    /// <summary>
    /// ����Ҫ��base()֮ǰ ��д _VCameraName 
    /// �������Ҫ��Ҫ��awake����д _inBlendTime ��  _exitBlednTime��Ĭ��ֵ�ֱ��� 2 �� 2
    /// </summary>
    protected virtual void Awake()
    {
        _VCamera = GameObject.Find(_VCameraName).GetComponent<CinemachineVirtualCamera>();
        cameraTriggerBox = GetComponent<BoxCollider2D>();
 
    }


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            DoWhenTriggerEnter(collision);
        }
    }

    protected virtual void DoWhenTriggerEnter(Collider2D collision)
    {
        if (_VCamera == null)
        {
            Debug.Log("");
        }
        _VCamera.enabled = true;
        CameraMgr.GetInstance().SetDefaultBlednTime(_inBlendTime);
        if (IsCameraOutsideBlack)
        {
            StopAllCoroutines();
            StartCoroutine(TransparentizeScene());
            StartCoroutine(DarkScene());
        }

        //�������trriger����Ҫ����Ķ���
        foreach (GameObject obj in ActiveListWhenIn)
        {
            obj.SetActive(true);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            DoWhenTriggerExit(collision);
        }
    }

    /// <summary>
    /// Ĭ���뿪��ʱ�򲻻�����������ǵ�size��ƫ��
    /// </summary>
    /// <param name="collision"></param>
    protected virtual void DoWhenTriggerExit(Collider2D collision)
    {
        _VCamera.enabled = false;
        CameraMgr.GetInstance().SetDefaultBlednTime(_exitBlednTime);
    }


    protected IEnumerator TransparentizeScene()
    {
        float t = 0;
        while (t <= _inBlendTime)
        {
            for (int i = 0; i < BeTranspaencyScenceMasks.Length; i++)
            {
                SpriteRenderer s = BeTranspaencyScenceMasks[i];
                if (s.color.a == 0)
                {
                    continue;
                }
                float a = Mathf.Lerp(1, 0, t / _inBlendTime);
                s.color = new Color(s.color.r, s.color.g, s.color.b, a);
            }
            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        for (int i = 0; i < BeDrakScenceMasks.Length; i++)
        {
            SpriteRenderer s = BeDrakScenceMasks[i];
            s.color = new Color(s.color.r, s.color.g, s.color.b, 0);
        }
    }

    protected IEnumerator DarkScene()
    {
        float t = 0;
        while (t <= _inBlendTime)
        {
            for(int i = 0; i < BeDrakScenceMasks.Length; i++)
            {
                SpriteRenderer s = BeDrakScenceMasks[i];
                if (s.color.a == 1)
                {
                    continue;
                }
                float a = Mathf.Lerp(0, 1, t / _inBlendTime);
                s.color = new Color(s.color.r, s.color.g, s.color.b, a);
            }

            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }


        for (int i = 0; i < BeDrakScenceMasks.Length; i++)
        {
            SpriteRenderer s = BeDrakScenceMasks[i];
            s.color = new Color(s.color.r, s.color.g, s.color.b, 1);
        }
    }





}
