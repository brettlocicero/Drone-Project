using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    void Awake () => instance = this;

    [Header("Runtime")]
    [SerializeField] int wave = 0;
    [SerializeField] List<Transform> currentEnemies;

    [Header("Settings")]
    [SerializeField] Transform[] enemies;
    [SerializeField] Transform[] enemySpawnpoints;

    void Start ()
    {
        StartCoroutine(Intermission());
    }

    IEnumerator Intermission () 
    {
        print("Intermission");
        yield return new WaitForSeconds(5f);

        wave++;
        currentEnemies.Clear();
        print("Starting wave " + wave);
        int enemyCount = wave * 2;
        StartCoroutine(StartWave(wave));
    }

    IEnumerator StartWave (int enemyCount)
    {
        float dt = 0f;
        while (enemyCount > 0) 
        {
            dt += Time.deltaTime;

            if (dt >= 1f) 
            {
                Vector3 pos = enemySpawnpoints[Random.Range(0, enemySpawnpoints.Length)].position;
                Transform e = Instantiate(enemies[Random.Range(0, enemies.Length)], pos, Quaternion.identity);
                currentEnemies.Add(e);
                enemyCount--;
                dt = 0f;
            }
            
            yield return null;
        }

        while (currentEnemies.Count > 0) 
            yield return null;
        
        StartCoroutine(Intermission());
    }

    public void RemoveEnemyFromList (Transform enemy) 
    {
        currentEnemies.Remove(enemy);
    }

    public int CurrentEnemyCount () 
    {
        return currentEnemies.Count;
    }

    public Transform GetRandomEnemy () 
    {
        return currentEnemies[Random.Range(0, currentEnemies.Count)];
    }
}
