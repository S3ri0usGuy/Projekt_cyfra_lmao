using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class fountainHp : MonoBehaviour
{
    public int baseHp;
    public int hp;
    [SerializeField] TextMeshProUGUI HpText;

    SpriteRenderer spriteRenderer;
    [SerializeField] private float getHitAnimationTime;
    [SerializeField] private float repelRadius;
    [SerializeField] private float repelForce;

    private void Awake()
    {
        ChangeHP();

        spriteRenderer = GetComponent<SpriteRenderer>();

        hp = baseHp;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        damageAnimation();
        ChangeHP();
        if (hp <= 0)
        {
            Invoke(nameof(RestartGame), 1f);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ChangeHP()
    {
        HpText.text = "Fountain HP: " + (100 * hp / baseHp) + "%";
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

    private void damageAnimation()
    {
        spriteRenderer.color = new Color(1f, 0.5f, 0.5f, 1f);
        Invoke(nameof(damageAnimationHelper), getHitAnimationTime);
    }

    private void damageAnimationHelper()
    {
        spriteRenderer.color = new Color(110f / 255f, 66f / 255f, 120f / 255f, 1f);
    }
}