using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    private MusicPlayer musicPlayer;

    void Start()
    {
        musicPlayer = transform.root.GetComponentInChildren<MusicPlayer>();
    } 

    void Update()
    {
        if (Input.anyKeyDown)
            musicPlayer.StopTitleMusic();
            SceneManager.LoadScene("GameScene");
    }
}