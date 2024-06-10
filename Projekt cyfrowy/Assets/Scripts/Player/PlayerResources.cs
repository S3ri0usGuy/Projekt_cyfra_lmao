using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerResources : MonoBehaviour
{
    [SerializeField] int baseHp, baseActions;
    int hp, actions;

    public bool isNight = false;
    [SerializeField] private float nightTime;
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

    [SerializeField] private TextMeshProUGUI dayCounter;
    public int day;

    private fountainHp fountainHp;

    [SerializeField] private GameObject questsField, fountainHpField;

    [SerializeField] private float repelRadius;
    [SerializeField] private float repelForce;

    private void Awake()
    {
        AudioMenager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioMenager>();
        abilities = GameObject.FindGameObjectWithTag("Player").GetComponent<Abilities>();
        fountainHp = GameObject.FindGameObjectWithTag("Fountain").GetComponent<fountainHp>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (PlayerPrefs.HasKey("Day"))
        {
            day = PlayerPrefs.GetInt("Day");
        }

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

    private void HealDamage(int amount)
    {
        hp += amount;
        if (hp > 5) hp = 5;
        healAnimation();
        ChangeHP();
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

    private void healAnimation()
    {
        spriteRenderer.color = new Color(0.5f, 1f, 0.5f, 1f);
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

    public void RestartGame()
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
        questsField.SetActive(!isNight);
        fountainHpField.SetActive(isNight);
        if (!isNight)
        {
            dayCounter.text = "Day: " + day;
        }
        else
        {
            day++;
            dayCounter.text = "Night: " + day;
        }
        hp = baseHp;
        fountainHp.hp = fountainHp.baseHp;
        ChangeHP();
        fountainHp.ChangeHP();
        PlayerPrefs.SetInt("Day", day);
    }

    private void UpdateTime()
    {
        pointer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, nightTimeLeft / nightTime * 180));
    }

    public void RepelEnemies()
    {
        Collider2D[] enemiesToRepel = Physics2D.OverlapCircleAll(transform.position, repelRadius);
        foreach (var enemy in enemiesToRepel)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    Vector2 direction = enemy.transform.position - transform.position;
                    enemyRb.AddForce(direction.normalized * repelForce, ForceMode2D.Impulse);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Kit"))
        {
            HealDamage(1);
            Destroy(other.gameObject);
        }
    }
}