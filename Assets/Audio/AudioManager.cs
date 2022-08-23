using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;


public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;

            s.source.loop = s.loop;
        }

        Play("MainTheme");
    }

    private void Start()
    {
        
    }

    public void Play (string _name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == _name);

        if ((s.isMusic && PlayerPrefs.GetString("Music") == "ON") || (!s.isMusic && PlayerPrefs.GetString("Sound") == "ON"))
        {
            if (s.source.isPlaying && !s.source.loop)
            {
                Sound newSound = new Sound();
                newSound.source = gameObject.AddComponent<AudioSource>();
                newSound.source.clip = s.clip;
                newSound.source.volume = s.volume;
                newSound.source.Play();

                float time = s.clip.length + 1f;
                Destroy(newSound.source, time);
            }
            else
            {
                s.source.Play();
            }
        }    
    }

    public void StopAllSounds()
    {
        foreach(Sound s in sounds)
        {
            if (s.source.isPlaying)
                s.source.Stop();
        }
    }

    private List<Sound> pausedSounds = new List<Sound>();
    public void PauseAllSounds()
    {
        if (pausedSounds.Count > 0) pausedSounds.Clear();

        foreach (Sound s in sounds)
        {
            if (s.source.isPlaying)
            {
                pausedSounds.Add(s);
                s.source.Pause();

                
            }   
        }
    }
    public void UnPauseAllSounds()
    {
        if (pausedSounds.Count > 0)
        foreach (Sound s in pausedSounds)
        {
            s.source.UnPause();
        }
    }

    public void UnPauseAllInGameMusic()
    {
        if (PlayerPrefs.GetInt("currentLevel") <= 20)
        {
            Play("BackGroundCity");
            Play("BackGroundCity+");
        }
        else
        {
            Play("BackGroundCountrySide");
            Play("BackGroundCity+");
        }
    }


    public void Stop(string _name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == _name);
        s.source.Stop();
    }

    public void SetVolume(string _name, float value)
    {
        Sound s = Array.Find(sounds, sound => sound.name == _name);

        if ((s.isMusic && PlayerPrefs.GetString("Music") == "ON") || (!s.isMusic && PlayerPrefs.GetString("Sound") == "ON"))
            s.source.volume = value;
    }

    public bool isPlaying(string _name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == _name);
        return s.source.isPlaying;
    }

    public void StopAllMusic()
    {
        foreach(Sound s in sounds)
        {
            if (s.isMusic) s.source.Stop();
        }
    }
}
