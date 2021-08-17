using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs = null;
    [SerializeField] private Health player = null;
    [SerializeField] private SpawnerData data = null;

    private float difficultyMultiplier = 1;
    private float spawnCooldown;
    private int deaths = 0;
    private int poolsAvailable = 1;
    private int spawnChanceTotal;

    private void Awake()
    {
        spawnCooldown = data.spawnRateRange.x;
        StartCoroutine(SpawnEnemies());
    }

    private int GetEnemy()
    {
        if (poolsAvailable == 1)
        {
            return 0;
        }

        int ticket = Random.Range(0, spawnChanceTotal);
        Vector2 winningRange = Vector2.zero;

        for (int i = 0; i < poolsAvailable; i++)
        {
            winningRange = new Vector2(winningRange.y, winningRange.y + data.spawnChances[i]);
            print(ticket + " : " + winningRange.x + "-" + winningRange.y);

            if (ticket >= winningRange.x && ticket <= winningRange.y)
            {
                return i;
            }
        }

        return 0;
    }

    private Vector3 SpawnEnemy(Vector3 previousEnemyPos)
    {
        Vector3 position = transform.position + Vector3.right * Random.Range(-data.spawningRange, data.spawningRange);
        while (position.x >= previousEnemyPos.x - data.spawnBufferSize && position.x <= previousEnemyPos.x + data.spawnBufferSize)
        {
            position = transform.position + Vector3.right * Random.Range(-data.spawningRange, data.spawningRange);
        }

        int index = GetEnemy();
        print(index + " / " + (poolsAvailable - 1));
        GameObject currentEnemy = Instantiate(enemyPrefabs[index], position, Quaternion.identity);
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

            spawnCooldown = Mathf.Max(spawnCooldown - data.spawnRateDecrease, data.spawnRateRange.y); // consider having the spawn cooldown be dictated by an array of fixed values too
            difficultyMultiplier = Mathf.Min(difficultyMultiplier + data.difficultyMultiplierIncrease, data.maxDifficultyMultiplier);
        }
    }

    private void OnEnemyDeathEventHandler()
    {
        deaths++;

        if (poolsAvailable < enemyPrefabs.Length && deaths >= data.baseDeathsThreshold * difficultyMultiplier)
        {
            poolsAvailable++;

            spawnChanceTotal = 0;
            for (int i = 0; i < poolsAvailable; i++)
                spawnChanceTotal += data.spawnChances[i];
        }
    }
}
