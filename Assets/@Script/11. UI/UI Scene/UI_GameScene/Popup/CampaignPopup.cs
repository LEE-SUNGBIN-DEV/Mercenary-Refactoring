using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CampaignPopup : UIPopup
{
    [SerializeField] private Button forestButton;
    [SerializeField] private Button templeButton;

    public void Initialize(CharacterData characterData)
    {
        characterData.QuestData.OnChangeQuestData += (PlayerQuestData questData) =>
        {
            bool canEnable = questData.MainQuestProgress >= 1000 ? true : false;
            SetForestButton(canEnable);

            canEnable = questData.MainQuestProgress >= 2000 ? true : false;
            SetTempleButton(canEnable);
        };
    }

    public void CampaignButton(SCENE_LIST scene)
    {
        Managers.AudioManager.PlaySFX("Button Click");
        Managers.SceneManagerCS.LoadSceneAsync(scene);
    }

    public void SetForestButton(bool isEnable)
    {
        forestButton.interactable = isEnable;
    }

    public void SetTempleButton(bool isEnable)
    {
        templeButton.interactable = isEnable;
    }

    #region Property
    public Button ForestButton
    {
        get { return forestButton; }
        private set { forestButton = value; }
    }
    public Button TempleButton
    {
        get { return templeButton; }
        private set { templeButton = value; }
    }
    #endregion
}
