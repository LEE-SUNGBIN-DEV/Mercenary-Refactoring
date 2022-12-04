using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIEventHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
{
	public event UnityAction OnClickHandler = null;
	public event UnityAction OnPressHandler = null;

    public void AddEvent(UnityAction action, UI_EVENT eventType)
    {
        switch (eventType)
        {
            case UI_EVENT.Click:
                OnClickHandler -= action;
                OnClickHandler += action;
                break;

            case UI_EVENT.Press:
                OnPressHandler -= action;
                OnPressHandler += action;
                break;
        }
    }
	public void RemoveEvent(UnityAction action, UI_EVENT eventType)
    {
        switch (eventType)
        {
            case UI_EVENT.Click:
                OnClickHandler -= action;
                break;

            case UI_EVENT.Press:
                OnPressHandler -= action;
                break;
        }
    }
	public void ClearEvent()
    {
        OnClickHandler = null;
        OnPressHandler = null;
    }

	public void OnPointerClick(PointerEventData eventData)
	{
		OnClickHandler?.Invoke();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		OnPressHandler?.Invoke();
	}

}
