using UnityEngine;

[CreateAssetMenu(fileName = "New Spawner Data", menuName = "Spawner Data", order = 2)]
public class SpawnerData : ScriptableObject
{
    [SerializeField] private int _baseDeathsThreshold = 0; // The base amount of enemy deaths required for the difficulty to change
    public int baseDeathsThreshold { get { return _baseDeathsThreshold; } }

    [SerializeField] private float _spawningRange = 0; // x amount from zero that enemies can spawn within
    public float spawningRange { get { return _spawningRange; } }

    [SerializeField] private float _spawnBufferSize = 0; // The size that indicates the area which the previous spawn took up
    public float spawnBufferSize { get { return _spawnBufferSize; } }

    [SerializeField] private float _baseSpawnRate = 0;
    public float baseSpawnRate { get { return _baseSpawnRate; } }

    [SerializeField] private float _spawnRateDecrease = 0; // The amount by which the spawn rate decreases (in seconds) every cycle
    public float spawnRateDecrease { get { return _spawnRateDecrease; } }

    [SerializeField] private float _difficultyMultiplierIncrease = 0; // The amount by which the diffculty multiplier increases each cycle
    public float difficultyMultiplierIncrease { get { return _difficultyMultiplierIncrease; } }
}
