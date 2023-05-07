using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OptionPanel : UIPanel
{
    private enum SLIDER
    {
        BGM_Slider,
        SFX_Slider,
        Ambient_Slider            
    }

    [Header("Audio")]
    private Slider bgmSlider;
    private Slider sfxSlider;
    private Slider ambientSlider;

    public void Initialize()
    {
        BindSlider(typeof(SLIDER));
        bgmSlider = GetSlider((int)SLIDER.BGM_Slider);
        sfxSlider = GetSlider((int)SLIDER.SFX_Slider);
        ambientSlider = GetSlider((int)SLIDER.Ambient_Slider);

        bgmSlider.onValueChanged.AddListener(UpdateBGMVolume);
        sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
        ambientSlider.onValueChanged.AddListener(UpdateAmbientVolume);

        RefreshAudioOptions();
    }

    public void RefreshAudioOptions()
    {
        UpdateBGMVolume(Managers.DataManager.PlayerData.OptionData.BgmVolume);
        UpdateSFXVolume(Managers.DataManager.PlayerData.OptionData.SfxVolume);
        UpdateAmbientVolume(Managers.DataManager.PlayerData.OptionData.AmbientVolume);
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
}
