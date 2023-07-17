using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropItemSlot : UIBase
{
    public enum TEXT
    {
        Drop_Item_Text
    }

    public enum IMAGE
    {
        Drop_Item_Icon
    }

    private CanvasGroup canvasGroup;

    [SerializeField] private TextMeshProUGUI dropItemText;
    [SerializeField] private Image dropItemIcon;

    private Coroutine showSlotCoroutine;
    private int itemAmount;

    public void Initialize()
    {
        BindText(typeof(TEXT));
        BindImage(typeof(IMAGE));

        dropItemText = GetText((int)TEXT.Drop_Item_Text);
        dropItemIcon = GetImage((int)IMAGE.Drop_Item_Icon);
    }

    public void RequestShowSlot(BaseItem item, int amount)
    {
        if (showSlotCoroutine != null)
            StopCoroutine(showSlotCoroutine);

        showSlotCoroutine = StartCoroutine(CoShowSlot(item, amount));
    }

    public IEnumerator CoShowSlot(BaseItem item, int amount)
    {
        float fadeTime = 0.5f;
        float activeDuration = 2f;

        itemAmount += amount;
        dropItemText.text = $"{item.ItemName} x{Functions.GetIntCommaString(itemAmount)}";
        dropItemIcon.sprite = item.ItemSprite;
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

        itemAmount = 0;
        gameObject.SetActive(false);
    }
}
