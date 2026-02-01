using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance;

    [Header("Tracks")]
    public AudioClip titleMusic;
    public AudioClip pauseMusic;

    private AudioSource source;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        source = gameObject.AddComponent<AudioSource>();
        source.loop = true;
        source.playOnAwake = false;
        source.volume = 1f;

        PlayTitleMusic();
    }

    public void PlayTitleMusic()
    {
        if (source.clip == titleMusic) return;
        source.clip = titleMusic;
        source.Play();
    }

    public void StopTitleMusic()
    {
        if (source.clip == titleMusic)
        {
            source.Stop();
            source.clip = null;   
        }
    }

    public void PlayPauseMusic()
    {
        if (source.clip == pauseMusic) return;
        source.clip = pauseMusic;
        source.Play();
    }

    public void StopPauseMusic()
    {
        if (source.clip == pauseMusic)
        {
            source.Stop();
            source.clip = null;
        }
    }

    public void SetVolume(float v)
    {
        source.volume = v;
    }
}