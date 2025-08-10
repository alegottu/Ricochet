using System;
using System.Linq;
using System.Collections;
using UnityEngine;

public class EnemySpawner : Spawner
{
    [SerializeField] private Health player = null;
    [SerializeField] private SpawnerData data = null;

	private float spawningRange = 0;
    private float difficultyMultiplier = 1;
    private float spawnCooldown;
    private int deaths = 0;
    private int poolsAvailable = 1;

    private void Awake()
    {
		spawningRange = Camera.main.orthographicSize / 2 - Camera.main.orthographicSize * data.rangePadding;
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

    private float SpawnEnemy(float previousEnemyPos)
    {
        float x = UnityEngine.Random.Range(-spawningRange, spawningRange);
        while (x >= previousEnemyPos - data.spawnBufferSize && x <= previousEnemyPos + data.spawnBufferSize)
        {
        	x = UnityEngine.Random.Range(-spawningRange, spawningRange);
        }

		Vector3 position = new Vector3(x, transform.position.y, 0);
        GameObject currentEnemy = Instantiate(data.enemyPrefabs[GetEnemy()], position, Quaternion.identity);
        currentEnemy.GetComponent<Enemy>().Setup(player, difficultyMultiplier);
        currentEnemy.GetComponent<Health>().OnDeath += OnEnemyDeathEventHandler;
        
        return x;
    }

    private IEnumerator SpawnEnemies()
    {
        float currentEnemyPos = 0;

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
