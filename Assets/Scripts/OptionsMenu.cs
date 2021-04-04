using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    GameObject fullscreenToggle;

    private void Start()
    {
        fullscreenToggle = GameObject.Find("ToggleFullscreen");
        if (Screen.fullScreen == true)
            fullscreenToggle.GetComponent<Toggle>().isOn = true;
        else
            fullscreenToggle.GetComponent<Toggle>().isOn = false;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
