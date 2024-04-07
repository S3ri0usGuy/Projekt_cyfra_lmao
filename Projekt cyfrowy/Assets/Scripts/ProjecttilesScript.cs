using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjecttilesScript : MonoBehaviour
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
        Invoke(nameof(DestoryProjectile), lifeTime);

        AudioMenager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioMenager>();
        playerResources = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerResources>();

        target = GameObject.FindGameObjectWithTag("Player").transform;

        direction = (target.position - transform.position).normalized;
        force = speed * direction;
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject && !(collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Fireball")))
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                playerResources.TakeDamage(dmg);
                force = direction * knockbackForce;
                GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().AddForce(force);
            }

            AudioMenager.PlaySFX(AudioMenager.fireBallPop);
            Destroy(gameObject);
        }
    }

    void DestoryProjectile()
    {
        Destroy(gameObject);
    }
}
