using UnityEngine;

[CreateAssetMenu(fileName = "New Spawner Data", menuName = "Spawner Data", order = 3)]
public class SpawnerData : ScriptableObject
{
    [SerializeField] private int[] _deathThresholds = null; // The amount of enemy deaths required for the difficulty to change
    public int[] deathThresholds { get { return _deathThresholds; } }

    [SerializeField] private float _spawningRange = 0; // x amount from zero that enemies can spawn within
    public float spawningRange { get { return _spawningRange; } }

    [SerializeField] private float _spawnBufferSize = 0; // The size that indicates the area which the previous spawn took up
    public float spawnBufferSize { get { return _spawnBufferSize; } }

    [SerializeField] private Vector2 _spawnRateRange = Vector2.zero; // x is the base, y is the minimum
    public Vector2 spawnRateRange { get { return _spawnRateRange; } }

    [SerializeField] private float _spawnRateDecrease = 0; // The amount by which the spawn rate decreases (in seconds) every cycle
    public float spawnRateDecrease { get { return _spawnRateDecrease; } }

    [SerializeField] private int[] _spawnChances = null; // index i corresponds to the percent chance that enemyPrefabs[i] will spawn
    public int[] spawnChances { get { return _spawnChances; } }

    [SerializeField] private float _difficultyMultiplierIncrease = 0; // The amount by which the diffculty multiplier increases each cycle
    public float difficultyMultiplierIncrease { get { return _difficultyMultiplierIncrease; } }

    [SerializeField] private float _maxDifficultyMultiplier = 0; // x is the minimum, y is the maximum
    public float maxDifficultyMultiplier { get { return _maxDifficultyMultiplier; } }
}
