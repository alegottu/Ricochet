using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : Singleton<EnemySpawn>
{
    [HideInInspector] public int max = 1;
    public int ceiling = 1;
    [HideInInspector] public int current = 0;
    [HideInInspector] public int total = 0;

    [SerializeField] private GameObject enemy = null;
    private GameObject _enemy = null;
    Vector3 last = Vector3.zero;

    public Vector2 speed = Vector2.one;
    [SerializeField] private Vector2 maxSpeed = Vector2.one;
    [SerializeField] private Vector2 speedIncrement = Vector2.zero;

    [SerializeField] private float bounds = 1f;

    private void Update()
    {
        if (current < max)
        {
            if (!_enemy)
            {
                speed = new Vector2(Mathf.Clamp(speed.x + speedIncrement.x, 0, maxSpeed.x), Mathf.Clamp(speed.y + speedIncrement.y, 0, maxSpeed.y));

                Vector3 pos = new Vector3(Random.Range(-bounds, bounds), 0, 0);
                if (pos.x <= last.x + enemy.transform.localScale.x && pos.x >= last.x - enemy.transform.localScale.x)
                {
                    float x = Random.Range(0, 1) == 0 ? last.x + enemy.transform.localScale.x * 2 : last.x - enemy.transform.localScale.x * 2;
                    x = x < -bounds ? -bounds : x > bounds ? bounds : x;
                    pos = new Vector3(x, 0, 0);
                }
                _enemy = Instantiate(enemy, transform.position + pos, Quaternion.identity);
                last = _enemy.transform.position;

                current++;
                total++;
            }
            else
            {
                _enemy = null;
            }
        }
    }
}
