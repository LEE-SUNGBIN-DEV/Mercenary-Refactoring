using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class OptionPanel : UIPanel, IFocusPanel
{
    public enum SLIDER
    {
        BGM_Slider,
        SFX_Slider,
        Ambient_Slider            
    }

    public enum BUTTON
    {
        Selection_Button,
        Quit_Button,
    }

    public enum DROPDOWN
    {
        Resolution_Dropdown
    }

    public enum TOGGLE
    {
        Full_Screen_Mode_Toggle
    }

    public event UnityAction<IFocusPanel> OnOpenFocusPanel;
    public event UnityAction<IFocusPanel> OnCloseFocusPanel;

    private bool isOpen;

    [Header("Buttons")]
    private Button selectionButton;
    private Button quitButton;

    [Header("Audio")]
    private Slider bgmSlider;
    private Slider sfxSlider;
    private Slider ambientSlider;

    [Header("Graphic")]
    private Resolution[] systemResolutions;
    private TMP_Dropdown resolutionDropdown;
    private Toggle fullScreenModeToggle;

    private PlayerOptionData playerOptionData;

    #region Private
    private void OnEnable()
    {
        ConnectData();
    }
    private void OnDisable()
    {
        DisconnectData();
    }
    private void ConnectData()
    {
        playerOptionData = Managers.DataManager.PlayerData.OptionData;
        if(playerOptionData != null)
        {
            UpdateBGMVolume(playerOptionData.BgmVolume);
            UpdateSFXVolume(playerOptionData.SfxVolume);
            UpdateAmbientVolume(playerOptionData.AmbientVolume);
        }
    }
    private void DisconnectData()
    {
        if (playerOptionData != null)
        {
            playerOptionData = null;
        }
    }
    #endregion

    public void Initialize()
    {
        isOpen = false;
        // @ Defaults
        BindButton(typeof(BUTTON));
        selectionButton = GetButton((int)BUTTON.Selection_Button);
        selectionButton.onClick.AddListener(ClickSelectionButton);
        quitButton = GetButton((int)BUTTON.Quit_Button);
        quitButton.onClick.AddListener(ClickQuitButton);

        // @ Audios
        BindSlider(typeof(SLIDER));
        bgmSlider = GetSlider((int)SLIDER.BGM_Slider);
        sfxSlider = GetSlider((int)SLIDER.SFX_Slider);
        ambientSlider = GetSlider((int)SLIDER.Ambient_Slider);

        bgmSlider.onValueChanged.AddListener(UpdateBGMVolume);
        sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
        ambientSlider.onValueChanged.AddListener(UpdateAmbientVolume);

        // @ Graphics
        BindObject<TMP_Dropdown>(typeof(DROPDOWN));
        resolutionDropdown = GetObject<TMP_Dropdown>((int)DROPDOWN.Resolution_Dropdown);
        resolutionDropdown.options.Clear();

        systemResolutions = Screen.resolutions;
        for (int i = 0; i < systemResolutions.Length; ++i)
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
            optionData.text = $"{systemResolutions[i].width} * {systemResolutions[i].height} ({systemResolutions[i].refreshRate}hz)";
            resolutionDropdown.options.Add(optionData);

            if (systemResolutions[i].width == Screen.width && systemResolutions[i].height == Screen.height)
                resolutionDropdown.value = i;
        }
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener(UpdateResolution);

        BindObject<Toggle>(typeof(TOGGLE));
        fullScreenModeToggle = GetObject<Toggle>((int)TOGGLE.Full_Screen_Mode_Toggle);
        fullScreenModeToggle.isOn = Managers.DataManager.PlayerData.OptionData.IsFullScreen;
        fullScreenModeToggle.onValueChanged.AddListener(UpdateWindowMode);

        UpdateResolution(resolutionDropdown.value);
        UpdateWindowMode(fullScreenModeToggle.isOn);
    }

    public void UpdateBGMVolume(float volume)
    {
        Managers.DataManager.PlayerData.OptionData.BgmVolume = volume;
    }
    public void UpdateSFXVolume(float volume)
    {
        Managers.DataManager.PlayerData.OptionData.SfxVolume = volume;
    }
    public void UpdateAmbientVolume(float volume)
    {
        Managers.DataManager.PlayerData.OptionData.AmbientVolume = volume;
    }

    public void UpdateResolution(int index)
    {
        Managers.DataManager.PlayerData.OptionData.UpdateResolution(systemResolutions[index].width, systemResolutions[index].height, systemResolutions[index].refreshRate);
    }
    public void UpdateWindowMode(bool isEnable)
    {
        Managers.DataManager.PlayerData.OptionData.UpdateFullScreenMode(isEnable);
    }

    public void ClickSelectionButton()
    {
        quitButton.interactable = false;
        Managers.AudioManager.PlaySFX(Constants.Audio_Button_Click_Normal);
        Managers.DataManager.SavePlayerData();
        Managers.SceneManagerEX.LoadSceneFade(SCENE_ID.Selection);
    }

    public void ClickQuitButton()
    {
        selectionButton.interactable = false;
        Managers.AudioManager.PlaySFX(Constants.Audio_Button_Click_Quit);
        Managers.GameManager.SaveAndQuit();
    }

    public void TogglePanel()
    {
        if (isOpen)
            ClosePanel();
        else
            OpenPanel();
    }
    public void OpenPanel()
    {
        isOpen = true;
        OnOpenFocusPanel?.Invoke(this);
        Managers.InputManager.PushInputMode(CHARACTER_INPUT_MODE.UI);
        Managers.UIManager.SetCursorMode(CURSOR_MODE.VISIBLE);
        FadeInPanel(Constants.TIME_UI_PANEL_DEFAULT_FADE);
    }

    public void ClosePanel()
    {
        isOpen = false;
        OnCloseFocusPanel?.Invoke(this);
        Managers.InputManager.PopInputMode();
        Managers.UIManager.SetCursorMode(CURSOR_MODE.INVISIBLE);
        FadeOutPanel(Constants.TIME_UI_PANEL_DEFAULT_FADE, () => gameObject.SetActive(false));
    }
}
