using System;
using System.Linq;
using System.Collections;
using UnityEngine;

public class EnemySpawner : Spawner
{
    [SerializeField] private Health player = null;
    [SerializeField] private SpawnerData data = null;

    private float difficultyMultiplier = 1;
    private float spawnCooldown;
    private int deaths = 0;
    private int poolsAvailable = 1;

    private void Awake()
    {
        spawnCooldown = data.spawnRateRange.x;
        spawnChanceTotal = 0;
        StartCoroutine(SpawnEnemies());
    }

    private int GetEnemy()
    {
        if (poolsAvailable == 1)
        {
            return 0;
        }
        else
        {
            return GetSpawn(new ArraySegment<int>(data.spawnChances, 0, poolsAvailable).ToArray());
        }
    }

    private Vector3 SpawnEnemy(Vector3 previousEnemyPos)
    {
        Vector3 position = transform.position + Vector3.right * UnityEngine.Random.Range(-data.spawningRange, data.spawningRange);
        while (position.x >= previousEnemyPos.x - data.spawnBufferSize && position.x <= previousEnemyPos.x + data.spawnBufferSize)
        {
            position = transform.position + Vector3.right * UnityEngine.Random.Range(-data.spawningRange, data.spawningRange);
        }

        GameObject currentEnemy = Instantiate(data.enemyPrefabs[GetEnemy()], position, Quaternion.identity);
        currentEnemy.GetComponent<Enemy>().SetUp(player, difficultyMultiplier);
        currentEnemy.GetComponent<Health>().OnDeath += OnEnemyDeathEventHandler;
        
        return position;
    }

    private IEnumerator SpawnEnemies()
    {
        Vector3 currentEnemyPos = Vector3.zero;

        while (true)
        {
            currentEnemyPos = SpawnEnemy(currentEnemyPos);
            yield return new WaitForSeconds(spawnCooldown);

            spawnCooldown = Mathf.Max(spawnCooldown - data.spawnRateDecrease, data.spawnRateRange.y);
            difficultyMultiplier = Mathf.Min(difficultyMultiplier + data.difficultyMultiplierIncrease, data.maxDifficultyMultiplier);
        }
    }

    private void OnEnemyDeathEventHandler()
    {
        deaths++;

        if (poolsAvailable < data.enemyPrefabs.Length && deaths >= data.deathThresholds[poolsAvailable - 1])
        {
            deaths = 0;
            poolsAvailable++;

            spawnChanceTotal = 0;
            for (int i = 0; i < poolsAvailable; i++)
                spawnChanceTotal += data.spawnChances[i];
        }
    }
}
