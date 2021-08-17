using System.Collections;
using UnityEngine;

public class Tank : Enemy
{
    [SerializeField] private int pathCycles = 0; // The amount of times the snaking pattern the tank uses to move is repeated

    public override void SetUp(Health player, float speedMultiplier)
    {
        base.SetUp(player, speedMultiplier);
        StartCoroutine(Snake());
    }

    private IEnumerator Snake()
    {
        float downDistance = Bounds.size.y / pathCycles; // The distance the tank moves downward for each path cycle
        float downTime = downDistance / data.speed;
        float lateralDistance;
        float lateralTime;
        float previousSpeedX = data.speed;
        float velocityY = data.speed * data.direction.y;

        while (true)
        {
            rb.velocity = new Vector2(0, velocityY);
            yield return new WaitForSeconds(downTime);

            lateralDistance = Random.Range(0, previousSpeedX > 0 ? transform.position.x : Bounds.size.x - transform.position.x);
            lateralTime = lateralDistance / data.speed;
            rb.velocity = new Vector2(-previousSpeedX, 0);
            yield return new WaitForSeconds(lateralTime);

            previousSpeedX = rb.velocity.x;
        }
    }
}
