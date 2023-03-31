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
    /// ȫ�ֿ�����ر����빦��
    /// </summary>
    /// <param name="isInput">isInputΪtrue�������빦�ܣ���֮��</param>
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
    /// ���������νӺ���������֪ͨע���˶�Ӧ ������ - ����ģʽ�� �¼��Ķ���ִ����Ӧ�ĺ����߼�
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
    /// �����Ҫʹ�õ����а����¼�
    /// �� ������ - ����ģʽ�� Ϊ��λ������ 
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
