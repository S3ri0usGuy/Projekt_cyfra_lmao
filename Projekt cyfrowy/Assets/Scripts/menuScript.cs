using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class menuScript : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeText;

    private void Start()
    {
        if (PlayerPrefs.HasKey("volume"))
        {
            LoadVolume();
        }
        else
        {
            SetVolume();
        }
    }
    public void StartGame()
    {
        PlayerPrefs.DeleteKey("isNight");
        PlayerPrefs.SetInt("Day", 0);
        PlayerPrefs.SetInt("plotStage", 1);
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetVolume()
    {
        float volume = volumeSlider.value/100;
        volumeText.text = "Volume: " + volume * 100 + "%";
        if (volume == 0f) volume = 0.00001f;
        myMixer.SetFloat("Main", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("volume", volume*100);
        Debug.Log("Menu: " + volume);
    }

    public void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
        SetVolume();
    }
}