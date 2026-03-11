using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [Header("AudioManager Settings")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;

    public SoundData backgroundMusic;

    private AudioSource _musicSource;

    public override void Awake()
    {
        base.Awake();

        _musicSource = gameObject.AddComponent<AudioSource>();
        _musicSource.loop = true;
        _musicSource.playOnAwake = false;

        if (backgroundMusic != null) PlayMusic(backgroundMusic);
    }

    public void PlayMusic(SoundData data)
    {
        if (data?.clip == null) return;
        _musicSource.clip = data.clip;
        _musicSource.volume = data.volume * musicVolume * masterVolume;
        _musicSource.pitch = data.pitch;
        _musicSource.Play();
    }

    public void PlaySFX(SoundData data, AudioSource source)
    {
        if (data?.clip == null || source == null) return;
        source.clip = data.clip;
        source.volume = data.volume * sfxVolume * masterVolume;
        source.pitch = data.pitch;
        source.Play();
    }

    public void StopMusic() => _musicSource.Stop();
}