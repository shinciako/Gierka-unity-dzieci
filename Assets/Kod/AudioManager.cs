using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    private float volume;
    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        volume = PlayerPrefs.GetFloat("VolumeLevel", 0.5f);
        SetVolume(volume);
    }

    private void Start()
    {
        musicSource.Play();
    }

    public void SetVolume(float volumeLevel)
    {
        volume = Mathf.Clamp(volumeLevel, 0.0f, 1.0f);
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("VolumeLevel", volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
