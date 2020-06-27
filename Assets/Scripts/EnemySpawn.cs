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

    private void FixedUpdate()
    {
        if (current < max)
        {
            if (!_enemy)
            {
                _enemy = Instantiate(enemy, transform.position + new Vector3(Random.Range(-2.5f, 2.5f), 0, 0), Quaternion.identity);
            }
            else
            {
                _enemy = null;
            }
            current++;
            total++;
        }
    }
}
