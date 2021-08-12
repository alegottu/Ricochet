using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPool = null;
    [SerializeField] private int[] waveAmounts = null;
    // The number at x index should correspond to the difficulty multiplier at which the enemy at x index should be introduced to the pool.
    [SerializeField] private int[] enemyAmounts = null;
    // Each index should correspond to the amount of enemies to be destroyed before the difficulty increases
    [SerializeField] private Health player = null;
    [SerializeField] private float spawningRange = 0;
    [SerializeField] private float bufferSize = 0;
    [SerializeField] private float spawnCooldown = 0;
    [SerializeField] private float spawnRateDecrease = 1;
    [SerializeField] private float difficultyMultiplier = 1;

    private int poolSize = 1;
    private int deaths = 0;
    private float spawnTimer = 0;
    private Vector3 currentEnemyPos = Vector3.zero;

    private Vector3 SpawnEnemy(Vector3 previous)
    {
        Vector3 position = transform.position + Vector3.right * Random.Range(-spawningRange, spawningRange);
        while (position.x >= previous.x - bufferSize && position.x <= previous.x + bufferSize)
        {
            position = transform.position + Vector3.right * Random.Range(-spawningRange, spawningRange);
        }

        GameObject currentEnemy = Instantiate(enemyPool[Random.Range(0, poolSize)], position, Quaternion.identity);
        currentEnemy.GetComponent<Enemy>().SetUp(player, difficultyMultiplier);
        currentEnemy.GetComponent<Health>().OnDeath += OnEnemyDeathEventHandler;
        
        return position;
    }

    private void OnEnemyDeathEventHandler()
    {
        deaths++;
    }

    private void IncreaseDifficulty()
    {
        difficultyMultiplier++;
        
        if (waveAmounts[poolSize - 1] * poolSize == difficultyMultiplier)
        {
            poolSize++;
        }

        spawnCooldown -= spawnRateDecrease;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            currentEnemyPos = SpawnEnemy(currentEnemyPos);
            spawnTimer = spawnCooldown;
        }

        if (deaths >= enemyAmounts[(int)difficultyMultiplier - 1])
        {
            deaths = 0;
            IncreaseDifficulty();
        }
    }
}
