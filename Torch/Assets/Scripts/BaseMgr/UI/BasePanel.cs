using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BasePanel : MonoBehaviour
{


    private Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();
    protected List<Text> textList = new List<Text>();

    [Header("Image")]
    public List<Image> imgList;
    public float fadeTime;


    public EventSystem eventSystem;




    /// <summary>
    /// 所有面板在Awake的时候会自动寻找面板上存在的组件,减少手动拖曳的工作量,默认寻找Button和Text组件
    /// </summary>
    protected virtual void Awake()
    {
        FindChildrenControls<Button>();
        FindChildrenControls<Text>();
        //为text注册事件
        TextOnClick();

        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    //展示面板
    public virtual void ShowMe()
    {

    }

    //隐藏面板
    public virtual void HideMe()
    {

    }

    /// <summary>
    /// 从controlDic中找到对应的对象中的那个组件并返回出去给注册事件使用
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
    /// 找到一个面板下的所有T UI组件，并按 控件对象名 - 组件 放进controlDic中 
    /// 并且如果找到了text组件，就把它放进textList中为自定义text事件做准备
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
                Debug.Log("添加了" + name + "的回调");
            }

            //在寻找text中，添加到list中并且为其注册自定义显示事件
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
    /// 给子类重写，用来方便管理单击的事件注册，而不用一个一个拖
    /// </summary>
    /// <param name="name">需要注册单击事件的控件名</param>
    protected virtual void OnClick(string name)
    {

    }

    /// <summary>
    /// 给子类重写，用来方便管理OnValueChange的事件注册，而不用一个一个拖
    /// </summary>
    /// <param name="toogleName"></param>
    /// <param name="value"></param>
    protected virtual void OnValueChange(string name, float value)
    {

    }

    /// <summary>
    /// 当鼠标移动进入txt时， 显示自己并取消显示其他text，
    /// </summary>
    /// <param name="txtName">当前需要被选中的txtName</param>
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


    //为txt注册点击函数
    protected virtual void TextOnClick()
    {
       
    }



    /// <summary>
    /// 调用Fade的接口
    /// </summary>
    /// <param name="orginAlpha"></param>
    /// <param name="targetAlpha"></param>
    /// <param name="waitTime"></param>
    /// <param name="callback"></param>
    protected void ExcuteFade(float orginAlpha, float targetAlpha, float waitTime = 0, UnityAction callback = null)
    {
        StartCoroutine(Fade(orginAlpha, targetAlpha, waitTime, callback));
    }



    /// <summary>
    /// 默认自带一个淡入淡出的协程
    /// </summary>
    /// <param name="originAlpha">初始Alpha</param>
    /// <param name="targetAlpha">目标Alpha</param>
    /// <param name="callBack">结束Fade后的回调，默认为null</param>
    /// <returns></returns>
    protected virtual IEnumerator Fade(float originAlpha, float targetAlpha,float waitTime = 0, UnityAction callBack = null)
    {

        yield return new WaitForSecondsRealtime(waitTime);

        Debug.Log("fadetime是" + fadeTime);

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
