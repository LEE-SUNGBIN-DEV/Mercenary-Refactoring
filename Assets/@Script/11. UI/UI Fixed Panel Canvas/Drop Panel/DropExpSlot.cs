using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class DropExpSlot : UIBase
{
    public enum TEXT
    {
        Drop_Exp_Text
    }

    public enum IMAGE
    {
        Drop_Exp_Icon
    }

    private CanvasGroup canvasGroup;

    [SerializeField] private TextMeshProUGUI dropExpText;
    [SerializeField] private Image dropExpIcon;

    private Coroutine showSlotCoroutine;
    private float expAmount;

    public void Initialize()
    {
        TryGetComponent(out canvasGroup);

        BindText(typeof(TEXT));
        BindImage(typeof(IMAGE));

        dropExpText = GetText((int)TEXT.Drop_Exp_Text);
        dropExpIcon = GetImage((int)IMAGE.Drop_Exp_Icon);

        HideSlot();
    }

    public void ShowSlot(float amount)
    {
        if (showSlotCoroutine != null)
            StopCoroutine(showSlotCoroutine);

        gameObject.SetActive(true);
        showSlotCoroutine = StartCoroutine(CoShowSlot(amount));
    }
    public void HideSlot()
    {
        if (showSlotCoroutine != null)
            StopCoroutine(showSlotCoroutine);

        expAmount = 0f;
        gameObject.SetActive(false);
    }

    public IEnumerator CoShowSlot(float amount)
    {
        float fadeTime = 0.5f;
        float activeDuration = 2f;

        expAmount += amount;
        dropExpText.text = $"°æÇèÄ¡ +{Functions.GetIntCommaString((int)expAmount)}";

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

        HideSlot();
    }
}
