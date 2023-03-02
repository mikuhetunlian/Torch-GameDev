using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
//using PathCreation;

public class ToBeFlower : PlayerAblity
{

    public Transform from;
    //������е���С����ʱ��
    public float minDuration;
    //������е�������ʱ��
    public float maxDuration;
    public GameObject[] flowers = new GameObject[3];

    protected float[] durationTimes = new float[3];
    protected short bezierNum;
    protected Transform effect;
    protected Transform to;
    private string PATH = "Prefab/";

    [Range(0,1)]
    [HideInInspector]
    //�����ڱ��������ߵ�λ�ã�0������㣬1�����յ�
    public float t;

    protected override void Start()
    {
        base.Start();
        bezierNum = 12;
    }

    /// <summary>
    /// ������������ �����ܹ���Խ����� ����
    /// </summary>
    /// <param name="effect">Ӱ�����ߵĵ�</param>
    /// <param name="target">Ŀ���</param>
    public void BeFlower(Transform effect,Transform target)
    {
        _player.ToTransparency();

        Vector3[] path;
        float durationTime = 1;

        SetPathPoint(effect, target);
        
        for (int i = 0; i < durationTimes.Length; i++)
        {
            durationTime += Random.Range(minDuration, maxDuration);
            durationTimes[i] = durationTime;
        }

        path = BezierPath();
        this.gameObject.transform.DOPath(path, durationTimes[1], PathType.CatmullRom, PathMode.Sidescroller2D).SetEase(Ease.Linear);

        for (int i = 0; i < flowers.Length; i++)
        {
            if (flowers[i] != null)
            {
                string name = flowers[i].name;

                GameObject obj = GameObject.Instantiate(Resources.Load(PATH + name)) as GameObject;
                obj.transform.position = from.position;

                path = BezierPath();
               
                //�ɳ�ֽм����ת
                obj.transform.DORotate(new Vector3(0, 0, 250), durationTime, RotateMode.WorldAxisAdd).SetId(obj.name);
                //�ɳ�ֽм��·��
                obj.transform.DOPath(path, durationTimes[i], PathType.CatmullRom, PathMode.Sidescroller2D).SetEase(Ease.InQuad).SetId(obj.name);
              
            }

           
        }
    }


    /// <summary>
    /// ���ó�ʼ���Ӱ���
    /// </summary>
    /// <param name="effect"></param>
    /// <param name="to"></param>
    public void SetPathPoint(Transform effect,Transform to)
    {
        this.effect = effect;
        this.to =to;
    }



    /// <summary>
    /// ���bezier���ߵ�wayP��oints ����
    /// </summary>
    /// <returns></returns>
    private Vector3[] BezierPath()
    {
        Vector3[] bezierPath = new Vector3[bezierNum];
        Vector3 offset  = new Vector3(0, Random.Range(-3, 3));
        for (int i = 0; i < bezierNum; i++)
        {
            float t = (float)i / (bezierNum - 1);
            bezierPath[i] = BezierPoint(t, from.position, effect.position + offset, to.position);

        }

        return bezierPath;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="t">0����·����㣬1����·���յ�</param>
    /// <param name="pos1">���</param>
    /// <param name="pos2">ƫ�Ƶ�</param>
    /// <param name="pos3">�յ�</param>
    /// <returns></returns>
    private Vector3 BezierPoint(float t, Vector3 pos1, Vector3 pos2, Vector3 pos3)
    {
        float t1 = (1 - t) * (1 - t);
        float t2 = 2 * t * (1 - t);
        float t3 = t * t;
        return t1 * pos1 + t2 * pos2 + t3 * pos3;
    }
}
