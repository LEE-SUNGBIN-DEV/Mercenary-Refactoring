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
        Drop_Item_Image
    }

    private CanvasGroup canvasGroup;

    [SerializeField] private TextMeshProUGUI dropItemText;
    [SerializeField] private Image dropItemImage;

    private Coroutine showSlotCoroutine;
    private int itemAmount;

    public void Initialize()
    {
        TryGetComponent(out canvasGroup);

        BindText(typeof(TEXT));
        BindImage(typeof(IMAGE));

        dropItemText = GetText((int)TEXT.Drop_Item_Text);
        dropItemImage = GetImage((int)IMAGE.Drop_Item_Image);

        HideSlot();
    }

    public bool IsShowing()
    {
        return showSlotCoroutine == null ? false : true;
    }

    public void ShowSlot(BaseItem item)
    {
        if (showSlotCoroutine != null)
            StopCoroutine(showSlotCoroutine);

        gameObject.SetActive(true);
        showSlotCoroutine = StartCoroutine(CoShowSlot(item));
    }
    public void HideSlot()
    {
        if (showSlotCoroutine != null)
            StopCoroutine(showSlotCoroutine);

        itemAmount = 0;
        gameObject.SetActive(false);
    }

    public IEnumerator CoShowSlot(BaseItem item)
    {
        float fadeTime = 0.5f;
        float activeDuration = 2f;

        Managers.AudioManager.PlaySFX(Constants.Audio_Item_Get);
        if (item is IStackableItem stackableItem)
        {
            itemAmount += stackableItem.ItemCount;
            dropItemText.text = $"{item.GetItemName()} X{Functions.GetIntCommaString(itemAmount)}";
        }
        else
        {
            dropItemText.text = $"{item.GetItemName()}";
        }
        dropItemImage.sprite = item.GetItemSprite();
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

        HideSlot();
    }
}
