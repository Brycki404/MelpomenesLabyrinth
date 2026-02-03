using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenuController : MonoBehaviour
{
    [Header("UI")]
    public GameObject panelGO;
    public CanvasGroup group;
    public RectTransform panel;


    public Slider musicSlider;
    public Slider sfxSlider;

    private GameObject musicPlayerGO;
    private MusicPlayer musicPlayer; 
    private bool isOpen = false;

    void Start()
    {
        panelGO.SetActive(false);

        musicPlayerGO = GameObject.Find("MusicPlayer");
        musicPlayer = musicPlayerGO.GetComponent<MusicPlayer>();

        // Initialize sliders from AudioBus
        musicSlider.value = AudioBus.Instance.musicVolume;
        sfxSlider.value = AudioBus.Instance.sfxVolume;

        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            panelGO.SetActive(true);
            Time.timeScale = 0f;
            musicPlayer.StopCombatMusic();
            musicPlayer.PlayPauseMusic();
            StartCoroutine(UIAnimations.FadeScaleIn(group, panel, 0.15f));
        }
        else
        {
            Time.timeScale = 1f;
            musicPlayer.StopPauseMusic();
            musicPlayer.PlayCombatMusic();
            StartCoroutine(CloseRoutine());
        }
    }

    IEnumerator CloseRoutine()
    {
        yield return StartCoroutine(UIAnimations.FadeScaleOut(group, panel, 0.15f));
        panelGO.SetActive(false);
    }

    public void OnMusicVolumeChanged(float v)
    {
        AudioBus.Instance.musicVolume = v;
        MusicPlayer.Instance.SetVolume(v);
    }

    public void OnSFXVolumeChanged(float v)
    {
        AudioBus.Instance.sfxVolume = v;
    }

    public void OnResumePressed()
    {
        TogglePause();
    }
    
    public void onTitlePressed()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }
}