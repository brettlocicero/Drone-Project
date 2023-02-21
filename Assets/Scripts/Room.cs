using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] Transform[] enemySpawnPoints;
    [SerializeField] GameObject[] enemies;
    [SerializeField] int enemyCount = 0;
    [SerializeField] float spawnDelay = 2f;

    void Start ()
    {
        StartCoroutine(SpawnEnemyWave(enemyCount));
    }

    IEnumerator SpawnEnemyWave (int count)
    {
        yield return new WaitForSeconds(spawnDelay);

        GameObject enemy = enemies[Random.Range(0, enemies.Length)];
        Vector3 pos = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)].position;
        Instantiate(enemy, pos, Quaternion.identity);

        if (count > 0) StartCoroutine(SpawnEnemyWave(--count));
    }
}
