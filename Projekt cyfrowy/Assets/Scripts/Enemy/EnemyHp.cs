using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHp : MonoBehaviour
{
    private AudioMenager audioMenager;
    private EnemyAi enemyAi;
    Quaternion emptyQuaternion;

    [SerializeField]
    private int maxHealth, currentHealth;

    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    private bool isDead = false;
    [SerializeField] private GameObject drop;

    private void Awake()
    {
        audioMenager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioMenager>();
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
            OnHitWithReference?.Invoke(sender);
        }
        else
        {
            OnDeathWithReference?.Invoke(sender);
            audioMenager.PlaySFX(audioMenager.enemyKill);
            isDead = true;
            Instantiate(drop, gameObject.transform.position, emptyQuaternion);
            Destroy(gameObject);
        }
    }
}
