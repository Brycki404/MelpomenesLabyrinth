using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    private GameObject musicPlayerGO;
    private MusicPlayer musicPlayer;

    void Start()
    {
        musicPlayerGO = GameObject.FindGameObjectWithTag("MusicPlayer");
        if (musicPlayerGO)
            musicPlayer = musicPlayerGO.GetComponent<MusicPlayer>();
    } 

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (musicPlayer)
                musicPlayer.StopTitleMusic();
                
            SceneManager.LoadScene("GameScene");
        }
    }
}