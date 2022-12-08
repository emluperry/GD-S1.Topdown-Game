using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

using Enums;

public class UI_Settings : UI_SimpleScreen
{
    [Header("SOUND")]
    [SerializeField] private Slider m_MasterVolumeSlider;
    [SerializeField] private Slider m_MusicVolumeSlider;
    [SerializeField] private Slider m_SFXVolumeSlider;

    public Action<VOLUME_SLIDER, float> onSliderChanged;
    public Action onSaveValues;

    protected override void Awake()
    {
        base.Awake();

        m_MasterVolumeSlider.onValueChanged.AddListener(delegate { ChangeMasterVolume(); });
        m_MusicVolumeSlider.onValueChanged.AddListener(delegate { ChangeMusicVolume(); });
        m_SFXVolumeSlider.onValueChanged.AddListener(delegate { ChangeSFXVolume(); });
    }

    protected override void OnDestroy()
    {
        m_MasterVolumeSlider.onValueChanged.RemoveAllListeners();

        base.OnDestroy();
    }

    protected override void BackButton()
    {
        base.BackButton();
        onSaveValues?.Invoke();
    }

    public void SetVolumeSliderValue(VOLUME_SLIDER slider, float value)
    {
        switch (slider)
        {
            case VOLUME_SLIDER.MASTERVOLUME:
                m_MasterVolumeSlider.value = value;
                break;

            case VOLUME_SLIDER.MUSICVOLUME:
                m_MusicVolumeSlider.value = value;
                break;
            case VOLUME_SLIDER.SFXVOLUME:
                m_SFXVolumeSlider.value = value;
                break;
        }
    }

    private void ChangeMasterVolume()
    {
        onSliderChanged?.Invoke(VOLUME_SLIDER.MASTERVOLUME, m_MasterVolumeSlider.value);
    }

    private void ChangeMusicVolume()
    {
        onSliderChanged?.Invoke(VOLUME_SLIDER.MUSICVOLUME, m_MusicVolumeSlider.value);
    }

    private void ChangeSFXVolume()
    {
        onSliderChanged?.Invoke(VOLUME_SLIDER.SFXVOLUME, m_SFXVolumeSlider.value);
    }
}
