using UnityEngine.Events;

public class OptionPopup : UIPopup
{
    private enum SLIDER
    {
        BGMSlider,
        SFXSlider
    }

    public override void Initialize(UnityAction<UIPopup> action = null)
    {
        base.Initialize(action);
    }
}
