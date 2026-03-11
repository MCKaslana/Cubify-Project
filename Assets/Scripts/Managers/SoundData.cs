using UnityEngine;

[CreateAssetMenu(fileName = "NewSoundData", menuName = "Audio/Sound Data")]
public class SoundData : ScriptableObject
{
    [Header("Clip")]
    [Tooltip("The audio clip to play.")]
    public AudioClip clip;

    [Header("Playback Settings")]
    [Range(0f, 1f)]
    [Tooltip("Base volume for this sound.")]
    public float volume = 1f;

    [Range(0.1f, 3f)]
    [Tooltip("Base pitch for this sound.")]
    public float pitch = 1f;

    [Tooltip("Whether this sound loops.")]
    public bool loop = false;

    [Header("Randomisation")]
    [Range(0f, 0.5f)]
    [Tooltip("Adds a random ±variance to volume each time the clip plays.")]
    public float volumeVariance = 0f;

    [Range(0f, 0.5f)]
    [Tooltip("Adds a random ±variance to pitch each time the clip plays.")]
    public float pitchVariance = 0f;

    public void ApplyTo(AudioSource source)
    {
        if (source == null) return;

        source.clip = clip;
        source.loop = loop;
    }
}
