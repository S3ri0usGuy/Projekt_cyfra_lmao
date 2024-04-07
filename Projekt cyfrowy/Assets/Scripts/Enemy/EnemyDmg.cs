using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDmg : MonoBehaviour
{
    [SerializeField] int dmg;
    public PlayerResources playerResources;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            playerResources.TakeDamage(dmg);
        }
    }
}