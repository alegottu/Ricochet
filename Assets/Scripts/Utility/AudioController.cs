using UnityEngine;
using UnityEngine.Audio;

public class AudioController : Singleton<AudioController>
{
    [SerializeField] private AudioSource source = null;
    [SerializeField] private AudioMixer mix = null;

    public void ChangeVolume(float volume)
    {
        mix.SetFloat("Master Volume", volume);
    }

    public void ChangeMusicVolume(float volume)
    {
        mix.SetFloat("Music Volume", volume);
    }

    public void ChangeSFXVolume(float volume)
    {
        mix.SetFloat("SFX Volume", volume);
    }

    public void ChangeTrack(AudioClip track)
    {
        source.clip = track;
    }

    public void Play()
    {
        source.Stop();
        source.PlayOneShot(source.clip);
    }

    public bool IsPlaying()
    {
        return source.isPlaying;
    }

    public void SetLoop(bool loop)
    {
        source.loop = loop;
    }
}
