using UnityEngine;

public class ItemSpawner : Spawner
{
    [SerializeField] private InventoryData data = null;
    [SerializeField] private Player player = null; // To give to items that need to change the player's stats

    private void OnEnable()
    {
        Enemy.OnEnemyKilled += OnEnemyKilledEventHandler;
        PlayerEffect.OnPlayerStatChange += OnPlayerStatChangeEventHandler;
    }

    private void OnEnemyKilledEventHandler(Transform enemy)
    {
        GameObject item = data.itemPrefabs[GetSpawn(data.spawnChances)];

        if (item != null)
        {
            Instantiate(item, enemy.position, Quaternion.identity);
        }
    }

    private void OnPlayerStatChangeEventHandler(PlayerEffect item)
    {
        item.CastEffect(player);
    }

    private void OnDisable()
    {
        Enemy.OnEnemyKilled -= OnEnemyKilledEventHandler;
        PlayerEffect.OnPlayerStatChange -= OnPlayerStatChangeEventHandler;
    }
}
