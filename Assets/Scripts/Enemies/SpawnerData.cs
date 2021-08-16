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

    [SerializeField] private Vector2 _spawnRateRange = Vector2.zero; // x is the base, y is the minimum
    public Vector2 spawnRateRange { get { return _spawnRateRange; } }

    [SerializeField] private float _spawnRateDecrease = 0; // The amount by which the spawn rate decreases (in seconds) every cycle
    public float spawnRateDecrease { get { return _spawnRateDecrease; } }

    [SerializeField] private float[] _spawnChance = null; // index i corresponds to the percent chance that enemyPrefabs[i] will spawn
    public float[] spawnChance { get { return _spawnChance; } }

    [SerializeField] private float _difficultyMultiplierIncrease = 0; // The amount by which the diffculty multiplier increases each cycle
    public float difficultyMultiplierIncrease { get { return _difficultyMultiplierIncrease; } }

    [SerializeField] private float _maxDifficultyMultiplier = 0; // x is the minimum, y is the maximum
    public float maxDifficultyMultiplier { get { return _maxDifficultyMultiplier; } }
}
