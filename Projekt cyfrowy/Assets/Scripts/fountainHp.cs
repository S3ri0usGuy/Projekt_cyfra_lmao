using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class fountainHp : MonoBehaviour
{
    AudioMenager AudioMenager;
    SpriteRenderer spriteRenderer;

    public int baseHp;
    public int hp;
    [SerializeField] TextMeshProUGUI HpText;

    [SerializeField] private float getHitAnimationTime;
    [SerializeField] private float repelRadius;
    [SerializeField] private float repelForce;
    [SerializeField] private int HpRegenAmount;
    [SerializeField] private float HpRegenFrequency;

    public bool canRegen;
    private Vector3 repelCenter;
    [SerializeField] private float repelCenterFix;

    private void Start()
    {
        AudioMenager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioMenager>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        repelCenter = transform.position - new Vector3(0, repelCenterFix, 0);

        canRegen = true;

        hp = baseHp;
    }

    private void Update()
    {
        if (hp < baseHp && canRegen)
        {
            canRegen = false;
            Invoke("RegenHP", HpRegenFrequency);
        }
        else if (hp >= baseHp)
        {
            hp = baseHp;
            CancelInvoke("RegenHP");
        }
        ChangeHP();
    }

    public void RegenHP()
    {
        hp += HpRegenAmount;
        canRegen = true;
    }

    public void TakeDamage(int damage)
    {
        AudioMenager.PlaySFX(AudioMenager.fountainDamage);
        hp -= damage;
        damageAnimation();
        if (hp <= 0)
        {
            Invoke(nameof(RestartGame), 1f);
        }
    }

    public void RestartGame()
    {
        AudioMenager.PlaySFX(AudioMenager.playerDeath);
        SceneManager.LoadScene(1);
    }

    public void ChangeHP()
    {
        HpText.text = "Fountain HP: " + (100 * hp / baseHp) + "%";
    }

    public void RepelEnemies()
    {
        Collider2D[] enemiesToRepel = Physics2D.OverlapCircleAll(repelCenter, repelRadius);
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

    private void damageAnimation()
    {
        spriteRenderer.color = new Color(1f, 0.5f, 0.5f, 1f);
        Invoke(nameof(damageAnimationHelper), getHitAnimationTime);
    }

    private void damageAnimationHelper()
    {
        spriteRenderer.color = Color.white;
    }
}