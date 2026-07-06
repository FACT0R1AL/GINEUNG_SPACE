using System;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public enum SoundType
{
    Master,
    BGM,
    SFX
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    
    public AudioMixer  mixer;

    private bool masterMute = false;
    private bool bgmMute = false;
    private bool sfxMute = false;
    private float defaultMasterVolume;
    private float defaultBGMVolume;
    private float defaultSFXVolume;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool MuteCheck(SoundType type)
    {
        return type switch
        {
            SoundType.Master => masterMute,
            SoundType.BGM => bgmMute,
            SoundType.SFX => sfxMute,
            _ => false,
        };
    }

    public void ChangeVolume(SoundType type, float volume)
    {
        switch (type)
        {
            case SoundType.Master: masterMute = false; break;
            case SoundType.BGM: bgmMute = false; break;
            case SoundType.SFX: sfxMute = false; break;
        }
        SetMixerVolume(type, volume);
    }

    private void SetMixerVolume(SoundType type, float volume)
    {
        mixer.SetFloat(type.ToString(), Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
    }

    public void Mute(SoundType type)
    {
        switch (type)
        {
            case SoundType.Master:
                if (!masterMute)
                {
                    masterMute = true;
                    defaultMasterVolume = mixer.GetFloat(type.ToString(), out float volume) ? Mathf.Pow(10, volume / 20) : 1f;
                    SetMixerVolume(SoundType.Master, 0.0001f);
                }
                else
                {
                    masterMute = false;
                    SetMixerVolume(SoundType.Master, defaultMasterVolume);
                }
                break;
            case SoundType.BGM:
                if (!bgmMute)
                {
                    bgmMute = true;
                    defaultBGMVolume = mixer.GetFloat(type.ToString(), out float volume) ? Mathf.Pow(10, volume / 20) : 1f;
                    SetMixerVolume(SoundType.BGM, 0.0001f);
                }
                else
                {
                    bgmMute = false;
                    SetMixerVolume(SoundType.BGM, defaultBGMVolume);
                }
                break;
            case SoundType.SFX:
                if (!sfxMute)
                {
                    sfxMute = true;
                    defaultSFXVolume = mixer.GetFloat(type.ToString(), out float volume) ? Mathf.Pow(10, volume / 20) : 1f;
                    SetMixerVolume(SoundType.SFX, 0.0001f);
                }
                else
                {
                    sfxMute = false;
                    SetMixerVolume(SoundType.SFX, defaultSFXVolume);
                }
                break;
        }
    }
}
