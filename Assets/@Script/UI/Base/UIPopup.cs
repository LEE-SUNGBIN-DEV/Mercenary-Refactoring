using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIPopup : UIBase, IPointerDownHandler
{
    public event UnityAction<UIPopup> OnFocus;

    public virtual void Initialize(UnityAction<UIPopup> action = null)
    {
        if(action != null)
        {
            OnFocus -= action;
            OnFocus += action;
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        OnFocus(this);
    }
}
