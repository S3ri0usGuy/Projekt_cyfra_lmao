using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHp : MonoBehaviour
{
    private AudioMenager AudioMenager;
    private EnemyAi enemyAi;

    [SerializeField]
    private int maxHealth, currentHealth;

    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    [SerializeField]
    private bool isDead = false;

    private void Awake()
    {
        AudioMenager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioMenager>();
        enemyAi = GetComponent<EnemyAi>();
        currentHealth = maxHealth;
    }

    public void InitializeHealth(int healthValue)
    {
        currentHealth = healthValue;
        maxHealth = healthValue;
        isDead = false;
    }

    public void GetHit(int amount, GameObject sender, bool isDamageFromMelee)
    {
        if (isDead)
            return;
        if (sender.layer == gameObject.layer)
            return;

        currentHealth -= amount;
        enemyAi.RunAway(isDamageFromMelee);

        if (currentHealth > 0)
        {
            AudioMenager.PlaySFX(AudioMenager.hit);
            OnHitWithReference?.Invoke(sender);
        }
        else
        {
            OnDeathWithReference?.Invoke(sender);
            AudioMenager.PlaySFX(AudioMenager.kill);
            isDead = true;
            Destroy(gameObject);
        }
    }
}
