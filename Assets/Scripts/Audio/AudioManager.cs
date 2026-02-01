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
    public AudioClip lowHealth; 
    public AudioClip death;
    public AudioClip dodge;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance == null)
            Instance = this; 

        else 
            Destroy(gameObject);
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
    public void PlayLowHealth() => PlaySFX(lowHealth); 
    public void PlayDeath() => PlaySFX(death);
    public void PlayDodge() => PlaySFX(dodge);
}
