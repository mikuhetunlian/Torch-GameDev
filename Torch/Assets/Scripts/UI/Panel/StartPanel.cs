using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class StartPanel : BasePanel
{

    public Button btnStart;
    public Button btnExit;
    public Button btnSet;

    public Image fadeImage;

    protected GameObject lastSelectGameObject;


    public void Start()
    {
        eventSystem.firstSelectedGameObject = btnStart.gameObject;

        btnStart.onClick.AddListener(BtnStartOnClick);
        btnExit.onClick.AddListener(BtnExitOnClick);
        btnSet.onClick.AddListener(BtnSetOnClick);

        InputManager.GetInstance().DisabeMouseInput();

        ExcuteFade(1, 0, 0.5f);

    }



    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            lastSelectGameObject = EventSystem.current.currentSelectedGameObject;

            Button currentBtn = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            //if (currentBtn != null)
            //{
            //    Debug.Log(currentBtn.name);
            //}
            //else
            //{
            //    Debug.Log("选中了没有button的物体" + lastSelectGameObject.name);
            //}
         
        }

        if (eventSystem.currentSelectedGameObject == null)
        {
            eventSystem.SetSelectedGameObject(lastSelectGameObject);
        }
    }


    protected void BtnStartOnClick()
    {
        ExcuteFade(0, 1,0, () =>
        {
            SceneMgr.GetInstance().LoadScene("First");
        });
     
    }

    protected void BtnExitOnClick()
    {
        #if UNITY_EDITOR
             UnityEditor.EditorApplication.isPlaying = false;
        #else
             Application.Quit();
        #endif 
    }

    protected void BtnSetOnClick()
    {
        Debug.Log("set desu");
    }



 

}
