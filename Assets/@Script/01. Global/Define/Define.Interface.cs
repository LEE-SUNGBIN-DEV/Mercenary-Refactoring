using UnityEngine;
using UnityEngine.Events;

public interface IPoolObject
{
    ObjectPooler ObjectPooler { get; }

    public void ActionAfterRequest(ObjectPooler owner);
    public void ActionBeforeReturn();
    public void ReturnOrDestoryObject();
}

#region Character Data
public interface ICharacterData
{
    public void CreateData();
    public void LoadData();
    public void UpdateData(CharacterData characterData);
    public void SaveData();
}
#endregion

#region UI
public interface IFocusPanel
{
    public event UnityAction<IFocusPanel> OnOpenFocusPanel;
    public event UnityAction<IFocusPanel> OnCloseFocusPanel;

    public void ClosePanel();
}
public interface IDialogueablePanel
{
    public void SetDialogue(DialogueData dialogueData);
}
#endregion

#region State
public interface IDurationState
{
    public float Duration { get; }
    public void SetDuration(float duration = 0f);
}

public interface IActionState
{
    public int StateWeight { get; }

    public void Enter();
    public void Update();
    public void Exit();
}
#endregion

#region Combat
public interface IStunable
{
    public void OnStun(float duration);
}
public interface IStaggerable
{
    public void OnStagger();
}
public interface ICompetable
{
    public void OnCompete();
}
#endregion

#region Item
public interface IUniqueEquipment
{
    public void Equip(CharacterStatusData statusData);
    public void UnEquip(CharacterStatusData statusData);
}
public interface IStackableItem
{
    public int ItemCount { get; set; }

    void Initialize(string itemID);
}
public interface IConsumableItem
{
    public void Consume(CharacterStatusData statusData);
}
public interface IEquipableItem
{
    public void Equip(CharacterStatusData statusData);
    public void UnEquip(CharacterStatusData statusData);
}
public interface IShopableItem
{
    public int ItemPrice { get; set; }

    public void BuyItem(PlayerCharacter character);
    public void SellItem(PlayerCharacter character);
}
#endregion

#region Interacting
public interface IInteractableObject
{
    public PlayerCharacter TargetCharacter { get; }
    public float Distance { get; }

    public void EnterDetection(PlayerCharacter character);
    public void UpdateDetection(PlayerCharacter character);
    public void ExitDetection(PlayerCharacter character);

    public void EnterInteraction(PlayerCharacter character);
    public void UpdateInteraction(PlayerCharacter character);
    public void ExitInteraction(PlayerCharacter character);
}
#endregion

#region NPC
public interface IFunctionalNPC
{
    public void EnableFunctionUI();
    public void DisableFunctionUI();
    public void ExecuteFunction();
    public void EndFunction();
}
#endregion

#region Object

public interface IWarpObject
{
    public Transform WarpPointTransform { get; }
}
#endregion

#region Tooltip
public interface ITooltipItemSlot
{
    public ItemTooltipPanel TooltipPanel { get; set; }
    public void ShowTooltip();
    public void HideTooltip();
}
public interface IItemTooltipModule
{
    public void Initialize();
    public void UpdateModule<T>(T item, CharacterInventoryData inventoryData) where T : BaseItem;
}
public interface ITooltipSkillNode
{
    public SkillTooltipPanel TooltipPanel { get; set; }
    public void ShowTooltip();
    public void HideTooltip();
}
public interface ISkillTooltipModule
{
    public void Initialize();
    public void UpdateModule(SkillData skillData);
}
#endregion