using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public float sfxVolume = 1f;
    public float musicVolume = 1f;
    public float basicMusicVolume = 0.3f; 

    private AudioSource music;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            music = GetComponent<AudioSource>();
            if (music == null)
                Debug.LogError("AudioManager: Music AudioSource eksik veya atanmamýþ.");

           
            musicVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
            sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1f);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
       
        UpdateAllSfxSourcesVolume();
    }

  
    public void PlayMusic()
    {
        if (music == null)
        {
            Debug.LogError("PlayMusic(): Music AudioSource atanmamýþ!");
            return;
        }

        float finalVolume = musicVolume * basicMusicVolume;
        music.volume = finalVolume;

        if (finalVolume > 0f)
        {
            if (!music.isPlaying) 
                music.Play();
        }
        else
        {
            music.Pause(); 
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.Save();

        if (music == null) return;

        float finalVolume = musicVolume * basicMusicVolume;
        music.volume = finalVolume;

        if (finalVolume > 0f)
        {
            if (!music.isPlaying) 
                music.Play();
        }
        else
        {
            music.Pause();
        }
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public void SetSfxVolume(float volume)
    {
        sfxVolume = volume;
        PlayerPrefs.SetFloat("sfxVolume", volume);
        PlayerPrefs.Save();

        UpdateAllSfxSourcesVolume();
    }

    public float GetSfxVolume()
    {
        return sfxVolume;
    }

    private void UpdateAllSfxSourcesVolume()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource source in allAudioSources)
        {
            if (source != music && source.clip != null)
            {
                source.volume = sfxVolume;
            }
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        UpdateAllSfxSourcesVolume();
    }
}