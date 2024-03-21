using UnityEngine;

public class AudioMenager : MonoBehaviour
{
    [Header("Audio sources")]
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio clips")]
    public AudioClip background;
    public AudioClip click;

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
}
