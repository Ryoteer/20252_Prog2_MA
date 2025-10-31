using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderAction : MonoBehaviour
{
    [Header("<color=orange>UI</color>")]
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _uiSlider;

    private void Start()
    {
        _masterSlider.value = AudioManager.Instance.MasterGroupVolume;
        _musicSlider.value = AudioManager.Instance.MusicGroupVolume;
        _sfxSlider.value = AudioManager.Instance.SfxGroupVolume;
        _uiSlider.value = AudioManager.Instance.UiGroupVolume;
    }

    public void SetMasterVolume(float value)
    {
        AudioManager.Instance.SetMasterVolume(value);
    }

    public void SetMusicVolume(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
    }

    public void SetSFXVolume(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
    }

    public void SetUIVolume(float value)
    {
        AudioManager.Instance.SetUIVolume(value);
    }
}
