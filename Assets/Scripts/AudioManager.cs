using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private Sound[] musics;
    [SerializeField] private Sound[] sfx;

    private Dictionary<string, AudioClip> musicClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sfxClips = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        //se já existe um AudioManager, destrói o duplicado
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in musics) musicClips[s.name] = s.clip;
        foreach (Sound s in sfx) sfxClips[s.name] = s.clip;

        musicSource.loop = true;
    }

    public void PlayMusic(string name)
    {
        if (!musicClips.TryGetValue(name, out AudioClip clip))
        {
            Debug.LogWarning("Música não encontrada: " + name);
            return;
        }

        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySFX(string name)
    {
        if (!sfxClips.TryGetValue(name, out AudioClip clip))
        {
            Debug.LogWarning("SFX não encontrado: " + name);
            return;
        }

        sfxSource.PlayOneShot(clip);
    }
}