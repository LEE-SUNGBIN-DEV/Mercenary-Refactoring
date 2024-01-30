using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionPanel : UIPanel
{
    public enum TEXT
    {
        Interaction_Text
    }

    [SerializeField] private TextMeshProUGUI interactionText;

    public void Initialize()
    {
        BindText(typeof(TEXT));
        interactionText = GetText((int)TEXT.Interaction_Text);
    }

    public void OpenPanel(string content)
    {
        FadeInPanel();
        interactionText.text = content;
    }
    public void ClosePanel()
    {
        FadeOutPanel(Constants.TIME_UI_PANEL_DEFAULT_FADE, () => gameObject.SetActive(false));
    }
}
