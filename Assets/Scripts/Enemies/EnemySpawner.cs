using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs = null;
    [SerializeField] private Health player = null;
    [SerializeField] private SpawnerData data = null;

    private float difficultyMultiplier = 1;
    private float spawnCooldown = 0;
    private int poolSize = 1;
    private int deaths = 0;

    private void Awake()
    {
        spawnCooldown = data.baseSpawnRate;
        StartCoroutine(SpawnEnemies());
    }

    private Vector3 SpawnEnemy(Vector3 previousEnemyPos)
    {
        Vector3 position = transform.position + Vector3.right * Random.Range(-data.spawningRange, data.spawningRange);
        while (position.x >= previousEnemyPos.x - data.spawnBufferSize && position.x <= previousEnemyPos.x + data.spawnBufferSize)
        {
            position = transform.position + Vector3.right * Random.Range(-data.spawningRange, data.spawningRange);
        }

        GameObject currentEnemy = Instantiate(enemyPrefabs[Random.Range(0, poolSize)], position, Quaternion.identity);
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
            spawnCooldown -= data.spawnRateDecrease; // consider having the spawn cooldown be dictated by an array of fixed values too
            difficultyMultiplier += data.difficultyMultiplierIncrease;
        }
    }

    private void OnEnemyDeathEventHandler()
    {
        deaths++;

        if (deaths >= data.baseDeathsThreshold * difficultyMultiplier)
        {
            poolSize++;
        }
    }
}
