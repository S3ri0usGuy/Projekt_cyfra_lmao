using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMenager : MonoBehaviour
{
    [Header("Audio sources")]
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource AmbientSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio clips")]
    public AudioClip background;
    public AudioClip fight;
    public AudioClip ambient;
    public AudioClip click;
    public AudioClip damage;
    public AudioClip fountainDamage;
    public AudioClip playerDeath;
    public AudioClip meleeHit;
    public AudioClip enemyKill;
    public AudioClip barrierCast;
    public AudioClip barrierBrake;
    public AudioClip arrowHit;
    public AudioClip teleport;
    public AudioClip dash;
    public AudioClip kitPickUp;

    [SerializeField] private AudioMixer myMixer;
    private float volume;

    private void Start()
    {
        if (PlayerPrefs.HasKey("volume"))
        {
            volume = PlayerPrefs.GetFloat("volume") / 100;
            if (volume == 0f) volume = 0.00001f;
            myMixer.SetFloat("Main", Mathf.Log10(volume) * 20);
        }
        AmbientSource.clip = ambient;
        SFXSource.clip = click;
        PlayDayMusic();
    }

    public void PlayDayMusic()
    {
        MusicSource.clip = background;
        MusicSource.Play();
    }

    public void PlayNightMusic()
    {
        MusicSource.clip = fight;
        MusicSource.Play();
    }

    public void PlayAmbient()
    {
        AmbientSource.Play();
    }

    public void PlayClickSound()
    {
        SFXSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
