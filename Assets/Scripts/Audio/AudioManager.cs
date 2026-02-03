using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("-------- Audio Source --------")]
    public AudioSource SFXSource;

    [Header("-------- Boss Audio Clip --------")]
    public AudioClip attackEmber; 
    public AudioClip attackBeam;
    public AudioClip bossHit; 
    public AudioClip bossText; 
    public AudioClip bossDeath; 
    public AudioClip bossLaugh; 

    [Header("-------- Player Audio Clip --------")]
    public AudioClip playerLowHealth; 
    public AudioClip playerDeath;
    public AudioClip playerDodge;
    public AudioClip playerAttack;
    public AudioClip playerHit;

    [Header("-------- User Interface --------")]
    public AudioClip button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance == null)
            Instance = this; 

        else 
            Destroy(gameObject);
    }

    public void SetVolume(float v)
    {
        SFXSource.volume = v;
    }
    
    public void PlaySFX(AudioClip clip)
    {   
        SFXSource.PlayOneShot(clip);
    }
    public void PlayAttackEmber() => PlaySFX(attackEmber); 
    public void PlayAttackBeam() => PlaySFX(attackBeam); 
    public void PlayBossHit() => PlaySFX(bossHit); 
    public void PlayBossText() => PlaySFX(bossText); 
    public void PlayBossDeath() => PlaySFX(bossDeath); 
    public void PlayBossLaugh() => PlaySFX(bossLaugh); 
    public void PlayLowHealth() => PlaySFX(playerLowHealth); 
    public void PlayDeath() => PlaySFX(playerDeath);
    public void PlayDodge() => PlaySFX(playerDodge);
    public void PlayAttack() => PlaySFX(playerAttack);
    public void PlayHit() => PlaySFX(playerHit);
    public void PlayButton() => PlaySFX(button);
}
