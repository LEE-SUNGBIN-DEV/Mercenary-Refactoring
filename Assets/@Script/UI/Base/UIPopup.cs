using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIPopup : UIBase, IPointerDownHandler
{
    public event UnityAction<UIPopup> OnFocus;

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        OnFocus?.Invoke(this);
    }
}
