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
    [SerializeField] private int spawnOrder;

    private float SpawnInterval;
    private int wave;

    private PlayerResources playerResources;

    void Start()
    {
        playerResources = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerResources>();
    }

    public void SpawnEnemies(int thisWave, float thisSpawnInterval)
    {
        SpawnInterval = thisSpawnInterval;
        wave = thisWave;
        RepeatSpawning();
    }

    private void RepeatSpawning()
    {
        if (playerResources != null)
        {
            if (playerResources.isNight)
            {
                SpawnEnemy();
                Invoke("RepeatSpawning", SpawnInterval);
            }
        }
        else
        {
            playerResources = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerResources>();
            RepeatSpawning();
        }
    }

    public void StopSpawning()
    {
        CancelInvoke("RepeatSpawning");
    }

    private void SpawnEnemy()
    {
        wave++;
        if (wave >= 5) wave = 1;
        if (wave != spawnOrder) return;

        switch (playerResources.day)
        {
            case 1:
                Instantiate(meleeEnemyPrefab, transform.position, Quaternion.identity); // melee
                break;
            case 2:
                Instantiate(meleeEnemyPrefab, transform.position, Quaternion.identity); // melee
                Instantiate(rangedEnemyPrefab, transform.position, Quaternion.identity); // ranged
                break;
            case 3:
                Instantiate(meleeEnemyPrefab, transform.position, Quaternion.identity); // melee
                Instantiate(assasinEnemyPrefab, transform.position, Quaternion.identity); // assasin
                Instantiate(sniperEnemyPrefab, transform.position, Quaternion.identity); // sniper
                break;
            case 4:
                Instantiate(meleeEnemyPrefab, transform.position, Quaternion.identity); // melee
                Instantiate(rangedEnemyPrefab, transform.position, Quaternion.identity); // ranged
                Instantiate(sniperEnemyPrefab, transform.position, Quaternion.identity); // sniper
                Instantiate(assasinEnemyPrefab, transform.position, Quaternion.identity); // assasin
                break;
            case 5:
                Instantiate(meleeEnemyPrefab, transform.position, Quaternion.identity); // melee
                break;
            case 6:
                Instantiate(meleeEnemyPrefab, transform.position, Quaternion.identity); // melee
                Instantiate(rangedEnemyPrefab, transform.position, Quaternion.identity); // ranged
                break;
            case 7:
                Instantiate(meleeEnemyPrefab, transform.position, Quaternion.identity); // melee
                Instantiate(assasinEnemyPrefab, transform.position, Quaternion.identity); // assasin
                Instantiate(sniperEnemyPrefab, transform.position, Quaternion.identity); // sniper
                break;
            case 8:
                Instantiate(meleeEnemyPrefab, transform.position, Quaternion.identity); // melee
                Instantiate(rangedEnemyPrefab, transform.position, Quaternion.identity); // ranged
                Instantiate(sniperEnemyPrefab, transform.position, Quaternion.identity); // sniper
                Instantiate(assasinEnemyPrefab, transform.position, Quaternion.identity); // assasin
                break;
            case 9:
                Instantiate(meleeEnemyPrefab, transform.position, Quaternion.identity); // melee
                break;
            case 10:
                Instantiate(meleeEnemyPrefab, transform.position, Quaternion.identity); // melee
                Instantiate(rangedEnemyPrefab, transform.position, Quaternion.identity); // ranged
                break;
            case 11:
                Instantiate(meleeEnemyPrefab, transform.position, Quaternion.identity); // melee
                Instantiate(assasinEnemyPrefab, transform.position, Quaternion.identity); // assasin
                Instantiate(sniperEnemyPrefab, transform.position, Quaternion.identity); // sniper
                break;
            case 12:
                Instantiate(meleeEnemyPrefab, transform.position, Quaternion.identity); // melee
                Instantiate(rangedEnemyPrefab, transform.position, Quaternion.identity); // ranged
                Instantiate(sniperEnemyPrefab, transform.position, Quaternion.identity); // sniper
                Instantiate(assasinEnemyPrefab, transform.position, Quaternion.identity); // assasin
                break;
        }
    }
}
