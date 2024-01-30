using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropResponseStoneSlot : UIBase
{
    public enum TEXT
    {
        Drop_Response_Stone_Text
    }

    public enum IMAGE
    {
        Drop_Response_Stone_Image
    }

    private CanvasGroup canvasGroup;

    [SerializeField] private TextMeshProUGUI dropResponseStoneText;
    [SerializeField] private Image dropResponseImage;

    private Coroutine showSlotCoroutine;
    private float responseStoneAmount;

    public void Initialize()
    {
        TryGetComponent(out canvasGroup);

        BindText(typeof(TEXT));
        BindImage(typeof(IMAGE));

        dropResponseStoneText = GetText((int)TEXT.Drop_Response_Stone_Text);
        dropResponseImage = GetImage((int)IMAGE.Drop_Response_Stone_Image);

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

        responseStoneAmount = 0f;
        gameObject.SetActive(false);
    }

    public IEnumerator CoShowSlot(float amount)
    {
        float fadeTime = 0.5f;
        float activeDuration = 2f;

        responseStoneAmount += amount;
        dropResponseStoneText.text = $"∞®¿¿ºÆ +{Functions.GetIntCommaString((int)responseStoneAmount)}";
        Managers.AudioManager.PlaySFX(Constants.Audio_Response_Stone_Get);

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
