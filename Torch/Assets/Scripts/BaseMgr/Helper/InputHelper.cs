using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHelper 
{


    //按键的状态
    public enum ButtonState { Off,ButtonDown,ButtonPressed,ButtonUp}
    

    public class IMButton
    {

        public StateMachine<ButtonState> State;
        //在 edit 中设定的特定的按键的名字 由 playerName_buttonName 组成
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
            //初始的状态是 ButtonState 的默认状态 Off
            State = new StateMachine<ButtonState>(null, false);
            ButtonID = playerID + "_" + buttonID;
            ButtonDownMethod = btnDown;
            ButtonPressedMethod = btnPressed;
            ButtonUpMethod = btnUp;
        }

        /// <summary>
        /// 触发 ButtonDownMethod ,并改变按钮状态
        /// </summary>
        public virtual void TriggerButtonDown()
        {
  
              ButtonDownMethod?.Invoke();
              State.ChangeState(InputHelper.ButtonState.ButtonDown);
        }


        /// <summary>
        /// 触发 ButtonPressedMethod ,并改变按钮状态
        /// </summary>
        public virtual void TriggerButtonPressed()
        {
            ButtonPressedMethod?.Invoke();
            State.ChangeState(ButtonState.ButtonPressed);
        }


        /// <summary>
        /// 触发 ButtonUpMethod ,并改变按钮状态
        /// </summary>
        public virtual void TriggerButtonUp()
        {
            ButtonUpMethod?.Invoke();
            State.ChangeState(ButtonState.ButtonUp);
        }

    }



    
}
