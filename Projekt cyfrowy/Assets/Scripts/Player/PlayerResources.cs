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
    [SerializeField] GameObject nightTimeFilter;

    public bool barrierIsActive;

    [SerializeField] private GameObject hp1, hp2, hp3, hp4, hp5;
    [SerializeField] private GameObject pointer, dayClock, nightClock;
    private Vector3 targetRotation;

    private Abilities abilities;

    SpriteRenderer spriteRenderer;
    [SerializeField] private float getHitAnimationTime;

    public float knockbackForce;
    public float knockbackForceForRangedEnemies;

    private void Awake()
    {
        AudioMenager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioMenager>();
        abilities = GameObject.FindGameObjectWithTag("Player").GetComponent<Abilities>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        hp = baseHp;

        nightTimeLeft = nightTime;
        actions = baseActions;

        if (!isNight)
        {
            ChangeAction();
        }

        changeTimeOfDay();
    }

    private void Update()
    {
        if (isNight)
        {
            if (nightTimeLeft > 0)
            {
                nightTimeLeft -= Time.deltaTime;
                UpdateTime();
            }
            else
            {
                isNight = false;
                changeTimeOfDay();
                nightTimeLeft = 0;
                nightTimeLeft = nightTime;
                actions = baseActions;
                ChangeAction();
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (barrierIsActive)
        {
            abilities.RemoveBarrier();
            AudioMenager.PlaySFX(AudioMenager.barrierBrake);
        }
        else
        {
            hp -= damage;
            AudioMenager.PlaySFX(AudioMenager.damage);
            damageAnimation();
            ChangeHP();
            if (hp <= 0)
            {
                AudioMenager.PlaySFX(AudioMenager.death);
                Invoke(nameof(RestartGame), 1f);
            }
        }
    }

    private void ChangeHP()
    {
        hp1.SetActive(true);
        hp2.SetActive(true);
        hp3.SetActive(true);
        hp4.SetActive(true);
        hp5.SetActive(true);
        if (hp <= 4)
        {
            hp5.SetActive(false);
            if (hp <= 3)
            {
                hp4.SetActive(false);
                if (hp <= 2)
                {
                    hp3.SetActive(false);
                    if (hp <= 1)
                    {
                        hp2.SetActive(false);
                        if (hp <= 0)
                        {
                            hp1.SetActive(false);
                        }
                    }
                }
            }
        }
    }

    private void damageAnimation()
    {
        spriteRenderer.color = new Color(1f, 0.5f, 0.5f, 1f);
        Invoke(nameof(damageAnimationHelper), getHitAnimationTime);
    }

    private void damageAnimationHelper()
    {
        spriteRenderer.color = Color.white;
    }

    private void ChangeAction()
    {
        switch (actions)
        {
            case 3:
                pointer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 152));
                break;
            case 2:
                pointer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                break;
            case 1:
                pointer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 27));
                break;
            case 0:
                pointer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;
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
            ChangeAction();
        }
        else
        {
            Debug.Log("You don't have enought time!");
        }

        if (actions <= 0)
        {
            isNight = true;
            changeTimeOfDay();
        }
        else
        {
            ChangeAction();
        }
    }

    private void changeTimeOfDay()
    {
        nightTimeFilter.SetActive(isNight);
        dayClock.SetActive(!isNight);
        nightClock.SetActive(isNight);
    }

    private void UpdateTime()
    {
        pointer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, nightTimeLeft / nightTime * 180));
    }
}