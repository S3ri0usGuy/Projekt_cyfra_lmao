using UnityEngine;

public class AudioMenager : MonoBehaviour
{
    [Header("Audio sources")]
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio clips")]
    public AudioClip background;
    public AudioClip click;
    public AudioClip damage;
    public AudioClip death;
    public AudioClip hit;
    public AudioClip kill;
    public AudioClip fireBallPop;
    public AudioClip barrierBrake;

    private void Start()
    {
        MusicSource.clip = background;
        SFXSource.clip = click;
        MusicSource.Play();
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
