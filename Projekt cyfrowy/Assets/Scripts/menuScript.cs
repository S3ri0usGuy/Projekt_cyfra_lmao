using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class menuScript : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeText;
    [SerializeField] private int difficulty;
    [SerializeField] private Button EasyButton;
    [SerializeField] private Button MediumButton;
    [SerializeField] private Button HardButton;
    private EventSystem eventSystem;

    private void Start()
    {
        eventSystem = EventSystem.current;

        if (PlayerPrefs.HasKey("volume"))
        {
            LoadVolume();
        }
        else
        {
            SetVolume();
        }

        if (PlayerPrefs.HasKey("difficulty"))
        {
            difficulty = PlayerPrefs.GetInt("difficulty");
        }
        else
        {
            difficulty = 2;
            PlayerPrefs.SetInt("difficulty", difficulty);
        }
    }

    public void RestartGame()
    {
        PlayerPrefs.DeleteKey("isNight");
        PlayerPrefs.DeleteKey("playerHasBow");
        PlayerPrefs.DeleteKey("playerHasability2");
        PlayerPrefs.DeleteKey("playerHasability3");
        PlayerPrefs.SetInt("Day", 0);
        PlayerPrefs.SetInt("plotStage", 1);
        SceneManager.LoadScene(1);
    }

    public void CountinueGame()
    {
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
    }

    public void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
        SetVolume();
    }

    public void ChangeDifficulty(int newDifficulty)
    {
        difficulty = newDifficulty;
        PlayerPrefs.SetInt("difficulty", difficulty);
    }

    public void GetDifficulty()
    {
        if (difficulty == 1)
        {
            eventSystem.SetSelectedGameObject(EasyButton.gameObject);
        }
        else if (difficulty == 2)
        {
            eventSystem.SetSelectedGameObject(MediumButton.gameObject);
        }
        else
        {
            eventSystem.SetSelectedGameObject(HardButton.gameObject);
        }
    }
}