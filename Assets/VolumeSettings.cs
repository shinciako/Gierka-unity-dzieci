using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField]private Slider musicSlider;

    private void Start()
    {
        musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        if (AudioManager.instance != null)
        {
            musicSlider.value = AudioManager.instance.GetVolume();
        }
    }

    private void UpdateMusicVolume(float volumeLevel)
    {
        float volumeNormalized = volumeLevel / musicSlider.maxValue;
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetVolume(volumeNormalized);
            Debug.Log(volumeLevel);
        }
    }
}
