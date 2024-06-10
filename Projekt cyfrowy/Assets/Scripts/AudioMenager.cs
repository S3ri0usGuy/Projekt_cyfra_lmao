using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

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
