using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerResources : MonoBehaviour
{
    [SerializeField] int baseHp;
    int hp;
    [SerializeField] TextMeshProUGUI HpText;

    [SerializeField] int baseActions;
    int actions;
    [SerializeField] TextMeshProUGUI ActionsText;

    [SerializeField] private bool isNight = false;
    [SerializeField] private float nightTime = 10;
    private float nightTimeLeft;

    AudioMenager AudioMenager;

    private void Awake()
    {
        AudioMenager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioMenager>();

        hp = baseHp;
        HpText.text = "HP: " + hp + " / " + baseHp;

        nightTimeLeft = nightTime;
        actions = baseActions;

        if (!isNight)
        {
            ActionsText.text = "Actions: " + actions + " / " + baseActions;
        }
        else
        {
            isNight = true;
        }
    }

    private void Update()
    {
        if (isNight)
        {
            if (nightTimeLeft > 0)
            {
                nightTimeLeft -= Time.deltaTime;
                UpdateTime(nightTimeLeft);
            }
            else
            {
                isNight = false;
                nightTimeLeft = 0;
                nightTimeLeft = nightTime;
                actions = baseActions;
                ActionsText.text = "Actions: " + actions + " / " + baseActions;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        hp-= damage;
        AudioMenager.PlaySFX(AudioMenager.damage);
        HpText.text = "HP: " + hp + " / " + baseHp;
        if (hp <= 0)
        {
            AudioMenager.PlaySFX(AudioMenager.death);
            Invoke(nameof(RestartGame), 1f);
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void UseActions(int amount)
    {
        if(amount <= actions)
        {
            actions -= amount;
        }
        else
        {
            Debug.Log("You don't have enought time!");
        }

        if (actions <= 0)
        {
            isNight = true;
        }
        else
        {
            ActionsText.text = "Actions: " + actions + " / " + baseActions;
        }
    }

    private void UpdateTime(float currentTime)
    {
        currentTime++;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        ActionsText.text = "Night time remaining: " + string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}