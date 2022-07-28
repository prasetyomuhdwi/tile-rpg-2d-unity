using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    private GameObject musicSource, effectSource;
    
    [SerializeField]
    private Sound[] musics, effects;
    private int currentMusicPlay;

    int i = 0;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        foreach(Sound m in musics)
        {
            m.source = musicSource.AddComponent<AudioSource>();
            m.source.clip = m.clip;
            m.index = i;
            m.source.volume = m.volume;
            m.source.pitch = m.pitch;
            m.source.playOnAwake = m.playOnAwake;
            m.source.loop = m.loop;
            m.source.enabled = false;
            
            i++;
        }
        
        i = 0;

        foreach (Sound e in effects)
        {
            e.source = effectSource.AddComponent<AudioSource>();
            e.source.clip = e.clip;
            e.index = i;
            e.source.volume = e.volume;
            e.source.pitch = e.pitch;
            e.source.playOnAwake = e.playOnAwake;
            e.source.loop = e.loop;
            e.source.enabled = false;

            i++;
        }
    }

    public void PlayEffect(string name)
    {
        Sound s = Array.Find(effects, s => s.name == name);

        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!");
            return;
        }

        s.source.enabled = true;
        s.source.PlayOneShot(s.clip);
    }

    public void PlayMusic(string name)
    {
        if (currentMusicPlay >= 0)
        {
            musicSource.GetComponents<AudioSource>()[currentMusicPlay].enabled = false;
        }

        Sound s = Array.Find(musics, s => s.name == name);

        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!");
            return;
        }

        currentMusicPlay = s.index;

        s.source.enabled = true;
        s.source.Play();       

    }

    public void changeMasterVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void toggleMusic()
    {
        musicSource.GetComponent<AudioSource>().mute = !musicSource.GetComponent<AudioSource>().mute;
    }

    public void toggleEffect()
    {
        effectSource.GetComponent<AudioSource>().mute = !effectSource.GetComponent<AudioSource>().mute;
    }
}
