using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Toggle toggleFullscreen;

    private void Start()
    {
        toggleFullscreen.isOn = false;
    }

    public void setVolume (float volume)
    {
        audioMixer.SetFloat("masterVolume", volume);
    }

    public void setFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
