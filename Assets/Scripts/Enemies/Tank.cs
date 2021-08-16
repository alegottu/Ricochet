using System.Collections;
using UnityEngine;

public class Tank : Enemy
{
    [SerializeField] private int pathCycles = 0; // The amount of times the snaking pattern the tank uses to move is repeated

    private float downDistance = 0; // The distance the tank moves downward for each path cycle
    private Vector2 originalSpeed = Vector2.zero;

    private void Awake()
    {
        downDistance = Bounds.size.y / pathCycles;
        originalSpeed = speed;

        StartCoroutine(Snake());
    }

    private IEnumerator Snake()
    {
        float downTime;
        float lateralTime;

        while (true)
        {
            downTime = downDistance / speed.y;
            for (float timer = downTime; timer > 0; timer -= Time.deltaTime)
            {
                speed = originalSpeed * Vector2.up;
                base.Move();
                yield return new WaitForFixedUpdate();
            }

            lateralTime = Random.Range(0, speed.x < 0 ? transform.position.x : Bounds.size.x - transform.position.x);
            for (float timer = lateralTime; timer > 0; timer -= Time.deltaTime)
            {
                speed = originalSpeed * new Vector2(-1, 0);
                base.Move();
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
