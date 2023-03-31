using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasePanel : MonoBehaviour
{


    private Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();
    protected List<Text> textList = new List<Text>();

    [Header("Image")]
    public List<Image> imgList;
    public float fadeTime;

    /// <summary>
    /// ���������Awake��ʱ����Զ�Ѱ������ϴ��ڵ����,�����ֶ���ҷ�Ĺ�����,Ĭ��Ѱ��Button��Text���
    /// </summary>
    protected virtual void Awake()
    {
        FindChildrenControls<Button>();
        FindChildrenControls<Text>();
        //Ϊtextע���¼�
        TextOnClick();
    }

    //չʾ���
    public virtual void ShowMe()
    {

    }

    //�������
    public virtual void HideMe()
    {

    }

    /// <summary>
    /// ��controlDic���ҵ���Ӧ�Ķ����е��Ǹ���������س�ȥ��ע���¼�ʹ��
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="controlName"></param>
    /// <returns></returns>
    protected T GetComponent<T>(string controlName) where T : UIBehaviour
    {
        if (controlDic.ContainsKey(controlName))
        {
            List<UIBehaviour> list = controlDic[controlName];
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] is T)
                {
                    return list[i] as T;
                }
            }
        }
        return null;
    }


    /// <summary>
    /// �ҵ�һ������µ�����T UI��������� �ؼ������� - ��� �Ž�controlDic�� 
    /// ��������ҵ���text������Ͱ����Ž�textList��Ϊ�Զ���text�¼���׼��
    /// </summary>
    /// <typeparam name="T"></typeparam>
    protected void FindChildrenControls<T>() where T : UIBehaviour
    {
        T[] controls = this.GetComponentsInChildren<T>();
        for (int i = 0; i < controls.Length; i++)
        {
            string name = controls[i].gameObject.name;
            if (!controlDic.ContainsKey(name))
            {
                controlDic.Add(name, new List<UIBehaviour>());
            }
            controlDic[name].Add(controls[i]);

            if (controls[i] is Button)
            {
                (controls[i] as Button).onClick.AddListener(() =>
                {
                    OnClick(name);
                });
            }
            else if (controls[i] is Slider)
            {
                (controls[i] as Slider).onValueChanged.AddListener((value) =>
                {
                    OnValueChange(name, value);

                });
                Debug.Log("�����" + name + "�Ļص�");
            }

            //��Ѱ��text�У���ӵ�list�в���Ϊ��ע���Զ�����ʾ�¼�
            if (controls[i] is Text)
            {
                textList.Add(controls[i] as Text);
                UIMgr.AddCustomEventTrigger(controls[i] as Text, EventTriggerType.PointerEnter, (data) =>
                {
                    SetCustomKey(name);
                });
            }
        }

    }

    /// <summary>
    /// ��������д������������������¼�ע�ᣬ������һ��һ����
    /// </summary>
    /// <param name="name">��Ҫע�ᵥ���¼��Ŀؼ���</param>
    protected virtual void OnClick(string name)
    {

    }

    /// <summary>
    /// ��������д�������������OnValueChange���¼�ע�ᣬ������һ��һ����
    /// </summary>
    /// <param name="toogleName"></param>
    /// <param name="value"></param>
    protected virtual void OnValueChange(string name, float value)
    {

    }

    /// <summary>
    /// ������ƶ�����txtʱ�� ��ʾ�Լ���ȡ����ʾ����text��
    /// </summary>
    /// <param name="txtName">��ǰ��Ҫ��ѡ�е�txtName</param>
    protected  void SetCustomKey(string txtName)
    {
        for (int i = 0; i < textList.Count; i++)
        {
            Animator animator = textList[i].gameObject.GetComponent<Animator>();
            if (animator != null)
            {
                if (!textList[i].gameObject.name.Equals(txtName))
                {
                    animator.SetBool("isShow", false);
                }
                else
                {
                    animator.SetBool("isShow", true);
                }
            }
          
        }
    }


    //Ϊtxtע��������
    protected virtual void TextOnClick()
    {
       
    }

    /// <summary>
    /// Ĭ���Դ�һ�����뵭����Э��
    /// </summary>
    /// <param name="originAlpha">��ʼAlpha</param>
    /// <param name="targetAlpha">Ŀ��Alpha</param>
    /// <param name="callBack">����Fade��Ļص���Ĭ��Ϊnull</param>
    /// <returns></returns>
    protected virtual IEnumerator Fade(float originAlpha, float targetAlpha, UnityAction callBack = null)
    {
        float t = 0;
        Color color = imgList[0].color;
        while (t <= fadeTime)
        {
            float a = Mathf.Lerp(originAlpha, targetAlpha, t / fadeTime);
            foreach (Image img in imgList)
            {
                img.color = new Color(color.r, color.g, color.b, a);
            }
            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        foreach (Image img in imgList)
        {
            img.color = new Color(color.r, color.g, color.b, targetAlpha);
        }
        callBack?.Invoke();
    }


}
