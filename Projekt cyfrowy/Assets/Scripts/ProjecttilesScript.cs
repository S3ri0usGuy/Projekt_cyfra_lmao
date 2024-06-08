using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesScript : MonoBehaviour
{
    private AudioMenager AudioMenager;
    private PlayerResources playerResources;

    [SerializeField] private int speed;
    [SerializeField] int dmg;
    [SerializeField] float lifeTime;
    [SerializeField] float knockbackForce;
    private Transform target;
    private Rigidbody2D rb;
    private Vector2 force;
    private Vector2 direction;

    private void Awake()
    {
        Invoke(nameof(DestroyProjectile), lifeTime);

        AudioMenager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioMenager>();
        playerResources = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerResources>();

        if (gameObject.tag == "Fireball")
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        else if (gameObject.tag == "Arrow")
        {
            GameObject cursorTarget = new GameObject("CursorTarget");
            cursorTarget.transform.position = GetCursorWorldPosition();
            target = cursorTarget.transform;
            Destroy(cursorTarget);
        }

        direction = (target.position - transform.position).normalized;
        force = speed * direction;
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.tag == "Fireball")
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                playerResources.TakeDamage(dmg);
                force = direction * knockbackForce;
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(force);
            }
        }
        else if (gameObject.tag == "Arrow")
        {
            if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Ranged Enemy"))
            {
                EnemyHp enemyHp = collision.gameObject.GetComponent<EnemyHp>();
                if (enemyHp != null)
                {
                    enemyHp.GetHit(dmg, GameObject.FindGameObjectWithTag("Player"), false);
                    force = direction * knockbackForce;
                    Debug.Log("Arrow: " + force);
                    collision.gameObject.GetComponent<Rigidbody2D>().AddForce(force);
                }
            }
        }
        AudioMenager.PlaySFX(AudioMenager.fireBallPop);
        Destroy(gameObject);
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    private Vector3 GetCursorWorldPosition()
    {
        Vector3 cursorScreenPosition = Input.mousePosition;
        Vector3 cursorWorldPosition = Camera.main.ScreenToWorldPoint(cursorScreenPosition);
        cursorWorldPosition.z = 0; // Ustaw z na 0, poniewa¿ 2D gry zazwyczaj u¿ywaj¹ p³aszczyzny XY
        return cursorWorldPosition;
    }
}
