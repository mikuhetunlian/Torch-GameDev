using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingeltonAutoManager<InputManager>
{
    [Header("Stauts")]
    /// InputManager ���ܿ���
    public bool InputDetectionActive = true;
    public string playerID = "Player1";

    //�Ƿ��������ƶ�
    public bool SmoothMovement;
    public List<InputHelper.IMButton> ButtonList;
    public Vector2 PrimaryMovement { get { return _primaryMovement; } }

    protected Vector2 _primaryMovement = Vector2.zero;
    protected string _axisHorizontal;
    protected string _axisVertical;


    public InputHelper.IMButton LeftMove { get; protected set; }
    public InputHelper.IMButton RightMove { get; protected set; }
    public InputHelper.IMButton JumpButton { get; set; }
    public InputHelper.IMButton ControlButton { get; set; }

    protected void Start()
    {
        InitializaButtons();
        InitializeAixs();
    }

    /// <summary>
    /// ��ʼ����������ӵ� buttonList ��
    /// </summary>
    protected void InitializaButtons()
    {
        ButtonList = new List<InputHelper.IMButton>();
        //ButtonList.Add(LeftMove = new InputHelper.IMButton(playerID, "LeftMove", LeftMoveDown, LeftMovePresswd, LeftMoveUp));
        //ButtonList.Add(RightMove = new InputHelper.IMButton(playerID, "RightMove",RightMoveDown,RightMovePresswd,RightMoveUp));

        ButtonList.Add(JumpButton = new InputHelper.IMButton(playerID, "Jump", JumpButtonDown, JumpButtonPresswd, JumpButtonUp));
        ButtonList.Add(ControlButton = new InputHelper.IMButton(playerID, "Control", ControlButtonDown, ControlButtonPresswd, ControlButtonUp));

    }

    /// <summary>
    /// ��ʼ��������
    /// </summary>
    protected void InitializeAixs()
    {
        _axisHorizontal = playerID + "_Horizontal";
        _axisVertical = playerID + "_Vertical";
    }

    protected void LateUpdate()
    {
        if (InputDetectionActive)
        {
            ProcessButtonStates();
        }
        else
        {
            ResetButtonState();
        }
    }

    protected void Update()
    {
        if (InputDetectionActive)
        {

            SetMovement();
            SetButtons();

        }
        else
        {
            ResetMovement();
        }
    }

    /// <summary>
    /// ÿһ֡���ˮƽ�����ֱ���ֵ ���� _primaryMovement
    /// </summary>
    public void SetMovement()
    {

        if (SmoothMovement)
        {
            _primaryMovement.x = Input.GetAxis(_axisHorizontal);
            _primaryMovement.y = Input.GetAxis(_axisVertical);
        }
        else
        {
            _primaryMovement.x = Input.GetAxisRaw(_axisHorizontal);
            _primaryMovement.y = Input.GetAxisRaw(_axisVertical);
        }

    }


    /// <summary>
    /// �ṩ���ƶ�UI����ˮƽ�ƶ��ٶ�
    /// </summary>
    /// <param name="movement"></param>
    public void SetMovementMobile(int x, int y)
    {
        _primaryMovement.x = x;
        _primaryMovement.y = y;
    }

    public void ResetMovement()
    {
        _primaryMovement = Vector2.zero;
    }

    /// <summary>
    /// ÿһ֡������button��״̬
    /// </summary>
    public virtual void SetButtons()
    {
        foreach (InputHelper.IMButton button in ButtonList)
        {

            if (Input.GetButton(button.ButtonID))
            {
                button.TriggerButtonPressed();
            }
            if (Input.GetButtonDown(button.ButtonID))
            {
                button.TriggerButtonDown();
            }
            if (Input.GetButtonUp(button.ButtonID))
            {
                button.TriggerButtonUp();
            }
        }
    }

    /// <summary>
    /// ��lateUpdate��ִ�У��������� ������״̬
    /// </summary>
    public virtual void ProcessButtonStates()
    {
        foreach (InputHelper.IMButton button in ButtonList)
        {
            if (button.State.CurrentState == InputHelper.ButtonState.ButtonDown)
            {
                StartCoroutine(DelayButtonPress(button));
            }
            if (button.State.CurrentState == InputHelper.ButtonState.ButtonUp)
            {
                button.State.ChangeState(InputHelper.ButtonState.Off);
            }
        }
    }
    /// <summary>
    /// ����ֹ�����ʱ���������а�ť��״̬
    /// </summary>
    public virtual void ResetButtonState()
    {
        foreach (InputHelper.IMButton button in ButtonList)
        {
            button.State.ChangeState(InputHelper.ButtonState.Off);
        }
    }
    /// <summary>
    /// �����֡�� buttonDown ��һ֡��������Ϊ buttonPressed ��״̬
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    protected IEnumerator DelayButtonPress(InputHelper.IMButton button)
    {
        //����һ��cycle�� update �� lateUpdate֮��ִ��
        yield return null;
        button.State.ChangeState(InputHelper.ButtonState.ButtonPressed);

    }

    public void DisabeMouseInput()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Input.simulateMouseWithTouches = false;
        Debug.Log("��ֹ���������");
    }

    public void EnableMouseInput()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Input.simulateMouseWithTouches = true;
    }





    public void JumpButtonDown() { JumpButton.State.ChangeState(InputHelper.ButtonState.ButtonDown); }
    public void JumpButtonPresswd() { JumpButton.State.ChangeState(InputHelper.ButtonState.ButtonPressed); }
    public void JumpButtonUp() 
    {
        JumpButton.State.ChangeState(InputHelper.ButtonState.ButtonUp);
    }

    public void ControlButtonDown() { ControlButton.State.ChangeState(InputHelper.ButtonState.ButtonDown); }
    public void ControlButtonPresswd() { ControlButton.State.ChangeState(InputHelper.ButtonState.ButtonPressed); }
    public void ControlButtonUp()
    {
        ControlButton.State.ChangeState(InputHelper.ButtonState.ButtonUp);
    }


    public void LeftMoveDown() { LeftMove.State.ChangeState(InputHelper.ButtonState.ButtonDown); }
    public void LeftMovePresswd() { LeftMove.State.ChangeState(InputHelper.ButtonState.ButtonPressed); }
    public void LeftMoveUp()
    {
        LeftMove.State.ChangeState(InputHelper.ButtonState.ButtonUp);
    }


    public void RightMoveDown() { RightMove.State.ChangeState(InputHelper.ButtonState.ButtonDown); }
    public void RightMovePresswd() { RightMove.State.ChangeState(InputHelper.ButtonState.ButtonPressed); }
    public void RightMoveUp()
    {
        RightMove.State.ChangeState(InputHelper.ButtonState.ButtonUp);
    }


}
