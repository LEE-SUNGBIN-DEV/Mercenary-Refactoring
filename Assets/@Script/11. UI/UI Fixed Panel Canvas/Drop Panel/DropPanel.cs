using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropPanel : UIPanel
{
    private CharacterInventoryData inventoryData;
    private CharacterStatusData statusData;

    private Queue<float> dropExpQueue = new Queue<float>();
    private Queue<int> dropResponseStoneQueue = new Queue<int>();
    private Queue<BaseItem> dropItemQueue = new Queue<BaseItem>();

    [SerializeField] private DropExpSlot dropExpSlot;
    [SerializeField] private DropResponseStoneSlot dropResponseStoneSlot;
    [SerializeField] private DropItemSlot[] dropItemSlots;
    private Coroutine dropCoroutine;
    [SerializeField] private LayoutGroup[] layoutGroups;

    #region Private
    private void OnEnable()
    {
        ConnectData();
        if (dropCoroutine == null)
            dropCoroutine = StartCoroutine(CoUpdatePanel());
    }
    private void OnDisable()
    {
        DisconnectData();
        if (dropCoroutine != null)
            StopCoroutine(dropCoroutine);
    }
    private void ConnectData()
    {
        inventoryData = Managers.DataManager.CurrentCharacterData.InventoryData;
        if (inventoryData != null)
        {
            inventoryData.OnRewardResponseStone -= EnqueueDropResponseStone;
            inventoryData.OnRewardResponseStone += EnqueueDropResponseStone;
            inventoryData.OnRewardItem -= EnqueueDropItem;
            inventoryData.OnRewardItem += EnqueueDropItem;
        }

        statusData = Managers.DataManager.CurrentCharacterData.StatusData;
        if (statusData != null)
        {
            statusData.OnGetRewardedExperience -= EnqueueDropExperience;
            statusData.OnGetRewardedExperience += EnqueueDropExperience;
        }
    }
    private void DisconnectData()
    {
        if (inventoryData != null)
        {
            inventoryData.OnRewardResponseStone -= EnqueueDropResponseStone;
            inventoryData.OnRewardItem -= EnqueueDropItem;
            inventoryData = null;
        }
        if (statusData != null)
        {
            statusData.OnGetRewardedExperience -= EnqueueDropExperience;
            statusData = null;
        }
    }

    private IEnumerator CoUpdatePanel()
    {
        while (true)
        {
            bool isModified = false;

            if (dropExpQueue.Count > 0)
            {
                dropExpSlot.ShowSlot(dropExpQueue.Dequeue());
                isModified = true;
            }

            if (dropResponseStoneQueue.Count > 0)
            {
                dropResponseStoneSlot.ShowSlot(dropResponseStoneQueue.Dequeue());
                isModified = true;
            }

            if (dropItemQueue.Count > 0)
            {
                for (int i = 0; i < dropItemSlots.Length; ++i)
                {
                    if (dropItemSlots[i].IsShowing() == false)
                    {
                        dropItemSlots[i].ShowSlot(dropItemQueue.Dequeue());
                        isModified = true;
                        break;
                    }
                }
            }

            if (isModified)
            {
                Functions.RebuildLayout(layoutGroups);
                yield return new WaitForSeconds(0.1f);
            }
            else
                yield return null;
        }
    }
    #endregion

    public void Initialize()
    {
        dropExpSlot = GetComponentInChildren<DropExpSlot>(true);
        dropExpSlot.Initialize();

        dropResponseStoneSlot = GetComponentInChildren<DropResponseStoneSlot>(true);
        dropResponseStoneSlot.Initialize();

        dropItemSlots = GetComponentsInChildren<DropItemSlot>(true);
        for (int i = 0; i < dropItemSlots.Length; ++i)
        {
            dropItemSlots[i].Initialize();
        }
        layoutGroups = GetComponentsInChildren<LayoutGroup>(true);
        OpenPanel();
    }
    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
    public void EnqueueDropExperience(float amount)
    {
        dropExpQueue.Enqueue(amount);
    }

    public void EnqueueDropResponseStone(int amount)
    {
        dropResponseStoneQueue.Enqueue(amount);

    }
    public void EnqueueDropItem(BaseItem item)
    {
        dropItemQueue.Enqueue(item);
    }
}
