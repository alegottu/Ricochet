using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public int increment = 1;
    [HideInInspector] public int max = 1;
    public int ceiling = 1;
    [HideInInspector] public int current = 0;
    [HideInInspector] public int total = 0;
    [SerializeField] private float _spawnTimer = 1;
    [SerializeField] private float minTimer = 0;
    [SerializeField] private float timerReduction = 1;
    private float spawnTimer = 0;

    private Player player = null;
    [SerializeField] private GameObject enemy = null;
    private GameObject _enemy = null;
    public static List<Enemy> currentEnemies;
    [SerializeField] GameObject[] enemyTypes = null;
    private GameObject enemyType = null;
    Vector3 last = Vector3.zero;

    public Vector2 speed = Vector2.zero;
    [SerializeField] private float maxSpeed = 1;
    [SerializeField] private float speedIncrement = 0;
    [SerializeField] private float speedIncrementFactor = 1;
    [SerializeField] private float perTime = 100;

    private float bounds = 1f;
    private float enemyLength = 1f;

    private void Start()
    {
        currentEnemies = new List<Enemy>();
        enemyLength = enemy.GetComponentInChildren<SpriteRenderer>().bounds.size.x;

        bounds = GameManager.Instance.bounds.bounds.size.x - enemyLength * 2;
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        _spawnTimer = Mathf.Clamp((int)UIManager.Instance.time % perTime == 0 ? _spawnTimer - timerReduction : _spawnTimer, minTimer, Mathf.Infinity);
        ceiling = (int)UIManager.Instance.time % perTime == 0 && UIManager.Instance.time != 0 ? ceiling + 1 : ceiling;

        if (current < max && spawnTimer <= 0)
        {
            if (!_enemy)
            {
                speed = new Vector2(Mathf.Clamp(speed.x + speedIncrement * speedIncrementFactor, 0, maxSpeed), Mathf.Clamp(speed.y + speedIncrement * speedIncrementFactor, 0, maxSpeed));
                speedIncrementFactor += UIManager.Instance.time / perTime;

                Vector3 pos = new Vector3(Random.Range(-bounds, bounds), 0, 0);
                if (pos.x <= last.x + enemy.transform.localScale.x && pos.x >= last.x - enemy.transform.localScale.x)
                {
                    float x = Random.Range(0, 1) == 0 ? last.x + enemy.transform.localScale.x * 2 : last.x - enemy.transform.localScale.x * 2;
                    x = x < -bounds ? -bounds : x > bounds ? bounds : x;
                    pos = new Vector3(x, 0, 0);
                }
                _enemy = InstantiateEnemy();
                currentEnemies.Add(_enemy.GetComponent<Enemy>());
                last = _enemy.transform.position;

                spawnTimer = _spawnTimer;

                current++;
                total++;
            }
            else
            {
                _enemy = null;
            }
        }

        spawnTimer -= Time.deltaTime;
    }

    public GameObject InstantiateEnemy()
    {
        enemyType = enemyTypes[Random.Range(0, enemyTypes.Length)];
        GameObject enemy = Instantiate(enemyType, transform);
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.setSpeed(speed);
        enemyScript.setPlayer(player);
        return enemy;
    }
}
