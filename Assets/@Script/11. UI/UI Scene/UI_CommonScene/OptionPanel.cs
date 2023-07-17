using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class OptionPanel : UIPanel
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

    public void Initialize()
    {
        // @ Defaults
        BindButton(typeof(BUTTON));
        selectionButton = GetButton((int)BUTTON.Selection_Button);
        quitButton = GetButton((int)BUTTON.Quit_Button);

        // @ Audios
        BindSlider(typeof(SLIDER));
        bgmSlider = GetSlider((int)SLIDER.BGM_Slider);
        sfxSlider = GetSlider((int)SLIDER.SFX_Slider);
        ambientSlider = GetSlider((int)SLIDER.Ambient_Slider);

        bgmSlider.onValueChanged.AddListener(UpdateBGMVolume);
        sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
        ambientSlider.onValueChanged.AddListener(UpdateAmbientVolume);

        RefreshAudioOptions();

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

        RefreshGraphicOptions();
    }

    public void RefreshAudioOptions()
    {
        UpdateBGMVolume(Managers.DataManager.PlayerData.OptionData.BgmVolume);
        UpdateSFXVolume(Managers.DataManager.PlayerData.OptionData.SfxVolume);
        UpdateAmbientVolume(Managers.DataManager.PlayerData.OptionData.AmbientVolume);
    }

    public void RefreshGraphicOptions()
    {
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
}
