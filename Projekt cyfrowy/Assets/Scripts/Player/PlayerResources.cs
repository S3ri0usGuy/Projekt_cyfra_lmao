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
    int hp;
    public int actions;

    public bool isNight;
    [SerializeField] private float nightTime;
    private float nightTimeLeft;

    AudioMenager AudioMenager;
    [SerializeField] GameObject nightTimeFilter;

    public bool barrierIsActive;

    [SerializeField] private GameObject hp1, hp2, hp3, hp4, hp5;
    [SerializeField] private GameObject pointer, dayClock, nightClock;
    private Vector3 targetRotation;

    [SerializeField] private Transform teleportDestiny;
    private Abilities abilities;
    private GameObject[] enemySpawnerArray = new GameObject[12];
    private EnemySpawner[] enemySpawner = new EnemySpawner[12];

    SpriteRenderer spriteRenderer;
    [SerializeField] private float getHitAnimationTime;

    public float knockbackForce;
    public float knockbackForceForRangedEnemies;
    float thisDayEnemySpawnInterval;

    [SerializeField] private TextMeshProUGUI dayCounter;
    public int day;

    private fountainHp fountainHp;

    [SerializeField] private GameObject questsField, fountainHpField;

    [SerializeField] private float repelRadius;
    [SerializeField] private float repelForce;

    [SerializeField] private float enemySpawnInterval;

    [SerializeField] private GameObject NPC1, NPC2, NPC3, NPC4;

    private void Start()
    {
        AudioMenager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioMenager>();
        abilities = GetComponent<Abilities>();
        fountainHp = GameObject.FindGameObjectWithTag("Fountain").GetComponent<fountainHp>();
        enemySpawnerArray = GameObject.FindGameObjectsWithTag("Spawner");
        for (int i = 0; i < 12; i++)
        {
            enemySpawner[i] = enemySpawnerArray[i].GetComponent<EnemySpawner>();
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        nightTimeLeft = nightTime;
        actions = baseActions;

        if (PlayerPrefs.HasKey("Day"))
        {
            day = PlayerPrefs.GetInt("Day");
        }
        if (PlayerPrefs.HasKey("isNight"))
        {
            isNight = true;
            gameObject.transform.position = teleportDestiny.position;
            actions = 0;
        }
        hp = baseHp;

        if (!isNight)
        {
            ChangeAction();
        }

        changeTimeOfDay();

        AudioMenager.PlayAmbient();
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
                AudioMenager.PlaySFX(AudioMenager.playerDeath);
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
            day++;
            dayCounter.text = "Day: " + day;
            for (int i = 0; i < 12; i++)
            {
                enemySpawner[i].StopSpawning();
            }
            abilities.ability1.SetActive(false);
            abilities.ability2.SetActive(false);
            abilities.ability3.SetActive(false);
            NPC1.SetActive(true);
            NPC2.SetActive(true);
            NPC3.SetActive(true);
            NPC4.SetActive(true);
            AudioMenager.PlayDayMusic();
        }
        else
        {
            Invoke("SpawningHelper", 1f);
            dayCounter.text = "Night: " + day;
            PlayerPrefs.SetInt("isNight", 1);
            abilities.ability1.SetActive(true);
            if (abilities.playerHasability2) abilities.ability2.SetActive(true);
            if (abilities.playerHasability3) abilities.ability3.SetActive(true);
            NPC1.SetActive(false);
            NPC2.SetActive(false);
            NPC3.SetActive(false);
            NPC4.SetActive(false);
            AudioMenager.PlayNightMusic();
        }
        hp = baseHp;
        fountainHp.hp = fountainHp.baseHp;
        ChangeHP();
        fountainHp.ChangeHP();
        fountainHp.canRegen = true;
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

    private void SpawningHelper()
    {
        int wave = UnityEngine.Random.Range(1, 13);
        if (day <= 4) thisDayEnemySpawnInterval = enemySpawnInterval + (enemySpawnInterval * (day - 1) * 0.85f);
        else if (day <= 8) thisDayEnemySpawnInterval = (enemySpawnInterval + (enemySpawnInterval * (day - 5) * 0.85f)) * 0.8f;
        else thisDayEnemySpawnInterval = (enemySpawnInterval + (enemySpawnInterval * (day - 9) * 0.85f)) * 0.64f;
        for (int i = 0; i < 12; i++)
        {
            enemySpawner[i].SpawnEnemies(wave, thisDayEnemySpawnInterval);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Kit"))
        {
            AudioMenager.PlaySFX(AudioMenager.kitPickUp);
            HealDamage(1);
            Destroy(other.gameObject);
        }
    }
}