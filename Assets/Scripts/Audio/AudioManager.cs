using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Settings")]
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private AudioSource musicSource;
    private List<AudioSource> sfxSources = new List<AudioSource>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SetupMusicSource();
        }
        else if (instance != this)
        {
            Destroy(gameObject); // destroy the duplicate
        }
    }


    private void SetupMusicSource()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = musicVolume;
    }

    // ========== MUSIC ==========
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null) return;

        musicSource.clip = clip;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
            musicSource.volume = musicVolume;
    }

    public void ToggleMusicMute(bool isMuted)
    {
        if (musicSource != null)
            musicSource.mute = isMuted;
    }

    // ========== SFX ==========
    public void PlaySFX(AudioClip clip, float volumeMultiplier = 1f, float pitch = 1f)
    {
        if (clip == null) return;

        AudioSource source = GetAvailableSFXSource();
        if (source == null) return;

        source.clip = clip;
        source.volume = sfxVolume * volumeMultiplier;
        source.pitch = pitch;
        source.Play();
    }


    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        foreach (var source in sfxSources)
        {
            if (source != null)
                source.volume = sfxVolume;
        }
    }

    public void ToggleSFXMute(bool isMuted)
    {
        foreach (var source in sfxSources)
        {
            if (source != null)
                source.mute = isMuted;
        }
    }

    // ========== INTERNAL ==========
    private AudioSource GetAvailableSFXSource()
    {
        foreach (var source in sfxSources)
        {
            if (source != null && !source.isPlaying)
                return source;
        }

        return CreateSFXSource();
    }

    private AudioSource CreateSFXSource()
    {
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.playOnAwake = false;
        newSource.loop = false;
        newSource.volume = sfxVolume;

        sfxSources.Add(newSource);
        return newSource;
    }
}
