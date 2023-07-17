using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropResonanceStoneSlot : UIBase
{
    public enum TEXT
    {
        Drop_Resonance_Stone_Text
    }

    public enum IMAGE
    {
        Drop_Resonance_Stone_Icon
    }

    private CanvasGroup canvasGroup;

    [SerializeField] private TextMeshProUGUI dropResonanceStoneText;
    [SerializeField] private Image dropResonanceIcon;

    private Coroutine showSlotCoroutine;
    private float resonanceStoneAmount;

    public void Initialize()
    {
        TryGetComponent(out canvasGroup);

        BindText(typeof(TEXT));
        BindImage(typeof(IMAGE));

        dropResonanceStoneText = GetText((int)TEXT.Drop_Resonance_Stone_Text);
        dropResonanceIcon = GetImage((int)IMAGE.Drop_Resonance_Stone_Icon);

        if (dropResonanceIcon.sprite == null)
        {

        }

        resonanceStoneAmount = 0;
    }

    public void RequestShowSlot(float amount)
    {
        if (showSlotCoroutine != null)
            StopCoroutine(showSlotCoroutine);

        showSlotCoroutine = StartCoroutine(CoShowSlot(amount));
    }

    public IEnumerator CoShowSlot(float amount)
    {
        float fadeTime = 0.5f;
        float activeDuration = 2f;

        resonanceStoneAmount += amount;
        dropResonanceStoneText.text = $"°ø¸í¼® +{Functions.GetIntCommaString((int)resonanceStoneAmount)}";
        gameObject.SetActive(true);

        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha = Mathf.Clamp01(canvasGroup.alpha += Time.deltaTime * (1 / fadeTime));
            yield return null;
        }

        float elapsedTime = 0f;
        while (elapsedTime < activeDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha = Mathf.Clamp01(canvasGroup.alpha -= Time.deltaTime * (1 / fadeTime));
            yield return null;
        }

        resonanceStoneAmount = 0f;
        gameObject.SetActive(false);
    }
}
