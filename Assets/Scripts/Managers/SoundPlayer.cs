using System.Security.Cryptography;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    public SoundData[] sounds;

    private AudioSource _source;

    private void Awake() => _source = GetComponent<AudioSource>();

    public void Play(int index = 0)
    {
        if (sounds == null || index >= sounds.Length) return;
        AudioManager.Instance?.PlaySFX(sounds[index], _source);
    }

    public void Play(string soundName)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i] != null && sounds[i].name == soundName)
            {
                Play(i);
                return;
            }
        }
    }

    public void Stop() => _source.Stop();
}
