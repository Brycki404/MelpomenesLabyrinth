using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance;

    [Header("Tracks")]
    public AudioClip titleMusic;
    public AudioClip pauseMusic;
    public AudioClip combatMusic;

    private AudioSource source;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        source = gameObject.AddComponent<AudioSource>();
        source.loop = true;
        source.playOnAwake = false;
        source.volume = 1f;
    
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "TitleScene")
            PlayTitleMusic();
        else if (currentScene.name == "GameScene")
            PlayCombatMusic();
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

    public void PlayCombatMusic()
    {
        if (source.clip == combatMusic) return;
        source.clip = combatMusic;
        source.Play();
    }

    public void StopCombatMusic()
    {
        if (source.clip == combatMusic)
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