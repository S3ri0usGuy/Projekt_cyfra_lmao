using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDmg : MonoBehaviour
{
    [SerializeField] int dmg;
    private PlayerResources playerResources;
    private fountainHp fountainHp;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        playerResources = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerResources>();
        fountainHp = GameObject.FindGameObjectWithTag("Fountain").GetComponent<fountainHp>();

        if (collision.gameObject.CompareTag("Player"))
        {
            playerResources.TakeDamage(dmg);
            playerResources.RepelEnemies();
        }
        else if (collision.gameObject.CompareTag("Fountain"))
        {
            fountainHp.TakeDamage(dmg);
            fountainHp.RepelEnemies();
        }
    }
}