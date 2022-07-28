using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public Toggle toggleFullscreen;
    public Slider slider;

    private void Start()
    {
        toggleFullscreen.isOn = false;
        slider.value = AudioListener.volume;
    }

    public void setVolume (float volume)
    {
        AudioManager.instance.changeMasterVolume(volume);
    }

    public void setFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
