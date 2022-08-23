using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    public bool loop;

    public bool isMusic = false;

    [HideInInspector]
    public AudioSource source;

    //public Sound()
    //{
    //    Debug.Log("created");
    //}
    //~Sound()
    //{
    //    Debug.Log("destroyed: " + name);
    //}
}
