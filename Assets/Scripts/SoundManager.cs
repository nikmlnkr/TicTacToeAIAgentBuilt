using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource effectsSource;
    public AudioSource musicSource;
    
    [Header("Sound Effects")]
    public AudioClip buttonClickSound;
    public AudioClip moveSound;
    public AudioClip winSound;
    public AudioClip drawSound;
    
    [Header("Settings")]
    public float effectsVolume = 0.8f;
    public float musicVolume = 0.5f;
    
    private static SoundManager instance;
    
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<SoundManager>();
            }
            return instance;
        }
    }
    
    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SetupAudioSources();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    void SetupAudioSources()
    {
        if (effectsSource == null)
        {
            effectsSource = gameObject.AddComponent<AudioSource>();
        }
        
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }
        
        effectsSource.volume = effectsVolume;
        musicSource.volume = musicVolume;
        musicSource.loop = true;
    }
    
    public void PlayButtonClick()
    {
        PlaySoundEffect(buttonClickSound);
    }
    
    public void PlayMove()
    {
        PlaySoundEffect(moveSound);
    }
    
    public void PlayWin()
    {
        PlaySoundEffect(winSound);
    }
    
    public void PlayDraw()
    {
        PlaySoundEffect(drawSound);
    }
    
    public void PlaySoundEffect(AudioClip clip)
    {
        if (clip != null && effectsSource != null)
        {
            effectsSource.PlayOneShot(clip);
        }
    }
    
    public void PlayMusic(AudioClip musicClip)
    {
        if (musicClip != null && musicSource != null)
        {
            musicSource.clip = musicClip;
            musicSource.Play();
        }
    }
    
    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }
    
    public void SetEffectsVolume(float volume)
    {
        effectsVolume = Mathf.Clamp01(volume);
        if (effectsSource != null)
        {
            effectsSource.volume = effectsVolume;
        }
    }
    
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }
    }
    
    public void ToggleSounds()
    {
        if (effectsSource != null)
        {
            effectsSource.mute = !effectsSource.mute;
        }
    }
    
    public void ToggleMusic()
    {
        if (musicSource != null)
        {
            musicSource.mute = !musicSource.mute;
        }
    }
}