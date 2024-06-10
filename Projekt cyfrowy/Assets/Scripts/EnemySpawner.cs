using System.Collections;
using System.Collections.Generic;
using Unity.XR.GoogleVr;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject meleeEnemyPrefab;
    [SerializeField] private GameObject rangedEnemyPrefab;
    [SerializeField] private GameObject assasinEnemyPrefab;
    [SerializeField] private GameObject sniperEnemyPrefab;
    [SerializeField] private float spawnInterval;
    [SerializeField] private int spawnOrder;
    private int wave = 1;

    private PlayerResources playerResources;

    void Start()
    {
        playerResources = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerResources>();
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (playerResources.isNight) SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        if (wave >= 4) wave = 1;
        else wave++;

        if (wave != spawnOrder) return;
        switch(playerResources.day)
        {
            case 1:
                Instantiate(meleeEnemyPrefab, transform.position, Quaternion.identity); // melee
                break;
            case 2:
                Instantiate(meleeEnemyPrefab, transform.position, Quaternion.identity); // melee
                Instantiate(sniperEnemyPrefab, transform.position, Quaternion.identity); // sniper
                break;
            case 3:
                Instantiate(meleeEnemyPrefab, transform.position, Quaternion.identity); // melee
                Instantiate(assasinEnemyPrefab, transform.position, Quaternion.identity); // assasin
                Instantiate(rangedEnemyPrefab, transform.position, Quaternion.identity); // ranged
                break;
            case 4:
                Instantiate(meleeEnemyPrefab, transform.position, Quaternion.identity); // melee
                Instantiate(rangedEnemyPrefab, transform.position, Quaternion.identity); // ranged
                Instantiate(sniperEnemyPrefab, transform.position, Quaternion.identity); // sniper
                Instantiate(assasinEnemyPrefab, transform.position, Quaternion.identity); // assasin
                break;
            case 5:
                Instantiate(meleeEnemyPrefab, transform.position, Quaternion.identity); // melee
                Instantiate(meleeEnemyPrefab, transform.position, Quaternion.identity); // melee
                Instantiate(rangedEnemyPrefab, transform.position, Quaternion.identity); // ranged
                Instantiate(sniperEnemyPrefab, transform.position, Quaternion.identity); // sniper
                Instantiate(assasinEnemyPrefab, transform.position, Quaternion.identity); // assasin
                break;
            case 6:
                Instantiate(meleeEnemyPrefab, transform.position, Quaternion.identity); // melee
                Instantiate(meleeEnemyPrefab, transform.position, Quaternion.identity); // melee
                Instantiate(rangedEnemyPrefab, transform.position, Quaternion.identity); // ranged
                Instantiate(sniperEnemyPrefab, transform.position, Quaternion.identity); // sniper
                Instantiate(assasinEnemyPrefab, transform.position, Quaternion.identity); // assasin
                Instantiate(assasinEnemyPrefab, transform.position, Quaternion.identity); // assasin
                break;
            case 7:
                Instantiate(meleeEnemyPrefab, transform.position, Quaternion.identity); // melee
                Instantiate(meleeEnemyPrefab, transform.position, Quaternion.identity); // melee
                Instantiate(rangedEnemyPrefab, transform.position, Quaternion.identity); // ranged
                Instantiate(sniperEnemyPrefab, transform.position, Quaternion.identity); // sniper
                Instantiate(sniperEnemyPrefab, transform.position, Quaternion.identity); // sniper
                Instantiate(assasinEnemyPrefab, transform.position, Quaternion.identity); // assasin
                Instantiate(assasinEnemyPrefab, transform.position, Quaternion.identity); // assasin
                break;
            case 8:
                Instantiate(meleeEnemyPrefab, transform.position, Quaternion.identity); // melee
                Instantiate(meleeEnemyPrefab, transform.position, Quaternion.identity); // melee
                Instantiate(rangedEnemyPrefab, transform.position, Quaternion.identity); // ranged
                Instantiate(rangedEnemyPrefab, transform.position, Quaternion.identity); // ranged
                Instantiate(sniperEnemyPrefab, transform.position, Quaternion.identity); // sniper
                Instantiate(sniperEnemyPrefab, transform.position, Quaternion.identity); // sniper
                Instantiate(assasinEnemyPrefab, transform.position, Quaternion.identity); // assasin
                Instantiate(assasinEnemyPrefab, transform.position, Quaternion.identity); // assasin
                break;
        }
    }
}
