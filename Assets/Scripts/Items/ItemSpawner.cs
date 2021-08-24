using UnityEngine;

public class ItemSpawner : Spawner
{
    [SerializeField] private InventoryData data = null;

    private void OnEnable()
    {
        Enemy.OnEnemyKilled += OnEnemyKilledEventHandler;
    }

    private void OnEnemyKilledEventHandler(Enemy enemy)
    {
        GameObject item = data.itemPrefabs[GetSpawn(data.spawnChances)];

        if (item != null)
        {
            Instantiate(item, enemy.transform.position, Quaternion.identity);
        }
    }

    private void OnDisable()
    {
        Enemy.OnEnemyKilled -= OnEnemyKilledEventHandler;
    }
}
