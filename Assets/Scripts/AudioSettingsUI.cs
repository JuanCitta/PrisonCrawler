using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Liga sliders de volume ao AudioManager.
/// Adiciona ao painel de configurações.
/// </summary>
public class AudioSettingsUI : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        if (AudioManager.Instance == null) return;

        // Inicializa sliders com volume atual
        if (musicSlider != null)
        {
            musicSlider.value = AudioManager.Instance.music.volume;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = AudioManager.Instance.sfx.volume;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    void SetMusicVolume(float value) => AudioManager.Instance.music.volume = value;
    void SetSFXVolume(float value)   => AudioManager.Instance.sfx.volume   = value;
}
