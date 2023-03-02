using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHelper 
{


    //������״̬
    public enum ButtonState { Off,ButtonDown,ButtonPressed,ButtonUp}
    

    public class IMButton
    {

        public StateMachine<ButtonState> State;
        //�� edit ���趨���ض��İ��������� �� playerName_buttonName ���
        public string ButtonID;

        public delegate void ButtonDownMethodDelegate();
        public delegate void BUttonPressedMethodDelegate();
        public delegate void ButtonUpMethodDelegate();

        public ButtonDownMethodDelegate ButtonDownMethod;
        public BUttonPressedMethodDelegate ButtonPressedMethod;
        public ButtonUpMethodDelegate ButtonUpMethod;


        public IMButton(string playerID, string buttonID, 
                        ButtonDownMethodDelegate btnDown = null,
                        BUttonPressedMethodDelegate btnPressed = null,
                        ButtonUpMethodDelegate btnUp= null)
        {
            //��ʼ��״̬�� ButtonState ��Ĭ��״̬ Off
            State = new StateMachine<ButtonState>(null, false);
            ButtonID = playerID + "_" + buttonID;
            ButtonDownMethod = btnDown;
            ButtonPressedMethod = btnPressed;
            ButtonUpMethod = btnUp;
        }

        /// <summary>
        /// ���� ButtonDownMethod ,���ı䰴ť״̬
        /// </summary>
        public virtual void TriggerButtonDown()
        {
  
              ButtonDownMethod?.Invoke();
              State.ChangeState(InputHelper.ButtonState.ButtonDown);
        }


        /// <summary>
        /// ���� ButtonPressedMethod ,���ı䰴ť״̬
        /// </summary>
        public virtual void TriggerButtonPressed()
        {
            ButtonPressedMethod?.Invoke();
            State.ChangeState(ButtonState.ButtonPressed);
        }


        /// <summary>
        /// ���� ButtonUpMethod ,���ı䰴ť״̬
        /// </summary>
        public virtual void TriggerButtonUp()
        {
            ButtonUpMethod?.Invoke();
            State.ChangeState(ButtonState.ButtonUp);
        }

    }



    
}
