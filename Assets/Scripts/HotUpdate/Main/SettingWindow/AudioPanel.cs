using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioPanel : MonoBehaviour
{
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private AudioMixer audioMixer;

    private void OnEnable()
    {
        masterVolumeSlider.value = SettingsManager.Instance.CurrentSettings.masterVolume;
        if (audioMixer == null) audioMixer = FindAnyObjectByType<AudioMixer>();
    }

    public void OnMasterVolumeChanged(float value)
    {
        SettingsManager.Instance.CurrentSettings.masterVolume = value;
        SetVolume(value);
    }

    public void ApplySettings()
    {
        SetVolume(SettingsManager.Instance.CurrentSettings.masterVolume);
    }

    private void SetVolume(float volume)
    {
        if (audioMixer)
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
    }
}