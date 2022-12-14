using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CreateCharacterPanel : UIPanel
{
    public event UnityAction OnOpenPanel;
    public event UnityAction OnClosePanel;

    public enum BUTTON
    {
        CreateButton,
        CancelButton,

        LancerButton
    }

    private CharacterSlot selectSlot;
    private CHARACTER_TYPE selectClass;
    private Animator animator;
    private bool isOpen;

    public void Initialize()
    {
        animator = GetComponent<Animator>();

        BindButton(typeof(BUTTON));

        selectClass = CHARACTER_TYPE.Null;

        GetButton((int)BUTTON.LancerButton).onClick.AddListener(() => { OnClickCharacterButton(CHARACTER_TYPE.Lancer); });
        GetButton((int)BUTTON.CreateButton).onClick.AddListener(() => { OnClickCreateButton(); });
        GetButton((int)BUTTON.CancelButton).onClick.AddListener(OnClickCancelButton);

        isOpen = false;
    }

    public void OpenPanel()
    {
        GetButton((int)BUTTON.CreateButton).interactable = false;
        GetButton((int)BUTTON.CancelButton).interactable = true;
        SetAnimation(true);
        OnOpenPanel();
        isOpen = true;
    }
    public void ClosePanel()
    { 
        if (isOpen)
        {
            GetButton((int)BUTTON.CreateButton).interactable = false;
            GetButton((int)BUTTON.CancelButton).interactable = false;
            SetAnimation(false);
            OnClosePanel();
            isOpen = false;
        }
    }

    public void SetSlot(CharacterSlot targetSlot)
    {
        selectSlot = targetSlot;
    }

    public void SetAnimation(bool isOpen)
    {
        if (animator != null)
        {
            animator.SetBool("isOpen", isOpen);
        }
    }

    #region Event Function
    public void OnClickCreateButton()
    {
        Managers.DataManager.PlayerData.CharacterDatas[selectSlot.slotIndex] = new CharacterData();
        Managers.DataManager.PlayerData.CharacterDatas[selectSlot.slotIndex].Initialize(selectClass);
        Managers.DataManager.SavePlayerData();

        ClosePanel();
    }
    public void OnClickCancelButton()
    {
        ClosePanel();
    }
    public void OnClickCharacterButton(CHARACTER_TYPE characterClass)
    {
        selectClass = characterClass;
        GetButton((int)BUTTON.CreateButton).interactable = true;
    }
    #endregion
}
