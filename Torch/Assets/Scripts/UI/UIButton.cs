using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public class UIButton : MonoBehaviour
{

    public bool initialActive;

    protected EventTrigger eventTrigger;
    protected Image image;
    protected TMP_Text text; 


    protected Color selectColor = new Color(1,1,1,1);
    protected Color deSelectColor = new Color(0.4235294f,0.4235294f, 0.4235294f,1);




    void Start()
    {
        

        image = GetComponent<Image>();
        text = GetComponentInChildren<TMP_Text>();
        eventTrigger = GetComponent<EventTrigger>();

        // ≥ı º…Ë÷√—’…´
        if (initialActive)
        {
            image.color = selectColor;
            text.color = selectColor;
        }
        else
        {
            image.color = deSelectColor;
            text.color = deSelectColor;
        }


        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Select;
        entry.callback.AddListener(OnButtonSelect);
        eventTrigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Deselect;
        entry.callback.AddListener(OnButtonDeSelect);
        eventTrigger.triggers.Add(entry);

     
    }


    public void OnButtonSelect(BaseEventData data)
    {
        image.color = selectColor;
        text.color = selectColor;
    }

    public void OnButtonDeSelect(BaseEventData data)
    {

        image.color = deSelectColor;
        text.color = deSelectColor;
    }




}
