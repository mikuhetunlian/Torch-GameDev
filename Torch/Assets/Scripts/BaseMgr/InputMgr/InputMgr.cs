using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMgr : BaseManager<InputMgr>
{

    public enum  KeyType 
    {
      Getkey,
      GetkeyUp,
    }

    /// <summary>
    /// 全局开启或关闭输入功能
    /// </summary>
    /// <param name="isInput">isInput为true，打开输入功能，反之。</param>
    public void InputEnable(bool isInput)
    {
        if (isInput == true)
        {
            MonoManager.GetInstance().AddUpdateLinstener(PlayerUpdate); 
        }
        else
        {
            MonoManager.GetInstance().RemoveUpdateLinstener(PlayerUpdate);
        }
    }


    /// <summary>
    /// 方便管理的衔接函数，用来通知注册了对应 “按键 - 按键模式” 事件的对象执行相应的后续逻辑
    /// </summary>
    /// <param name="key"></param>
    /// <param name="type"></param>
    private void CheckKeyCode(KeyCode key,KeyType type)
    {
        switch (type)
        {
            case KeyType.Getkey:
                if (Input.GetKey(key))
                {
                    EventMgr.GetInstance().EventTrigger("GetKey", key);
                }
                break;
            case KeyType.GetkeyUp:
                if (Input.GetKeyUp(key))
                {
                    EventMgr.GetInstance().EventTrigger("GetKeyUp", key);
                }
                break;
        }
    }

    /// <summary>
    /// 玩家需要使用的所有按键事件
    /// 以 “按键 - 按键模式” 为单位来触发 
    /// </summary>
    private void PlayerUpdate()
    {
        CheckKeyCode(KeyCode.UpArrow, KeyType.Getkey);
        CheckKeyCode(KeyCode.DownArrow, KeyType.Getkey);
        CheckKeyCode(KeyCode.D,KeyType.Getkey);
        CheckKeyCode(KeyCode.A,KeyType.Getkey);
        CheckKeyCode(KeyCode.Space, KeyType.Getkey);

        CheckKeyCode(KeyCode.UpArrow, KeyType.GetkeyUp);
        CheckKeyCode(KeyCode.DownArrow, KeyType.GetkeyUp);
        CheckKeyCode(KeyCode.D, KeyType.GetkeyUp);
        CheckKeyCode(KeyCode.A, KeyType.GetkeyUp);
        CheckKeyCode(KeyCode.Space, KeyType.GetkeyUp);

        //CheckKeyCode(KeyCode.S);
        //CheckKeyCode(KeyCode.D);
    }


}
