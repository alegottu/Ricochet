using UnityEngine;

public class TeleportEnemy : Enemy
{
    private int steps = 0;

    [SerializeField] private float speedModifier = 1;
    public override EnemyType type => EnemyType.TELEPORT;

    private void Start()
    {
        if (Random.Range(0, 2) > 0)
        {
            float x = Random.Range(speed.x / speedFluctation, speed.x);
            float total = speed.x + speed.y;
            speed = new Vector2(x, total - x);
            speed = new Vector2(Random.Range(0, 2) > 0 ? -speed.x : speed.x, -speed.y);
        }
        else
        {
            speed = new Vector2(0, -speed.x - speed.y);
        }
        speed *= speedModifier;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        steps++;
    }

    private void OnTeleport()
    {
        Vector3 newPos = transform.position + (Vector3)speed * steps;
        if (newPos.x > bounds.x / 2 || newPos.x < -bounds.x / 2)
        {
            speed = new Vector2(-speed.x, speed.y);
            newPos = transform.position + (Vector3)speed * steps;
        }
        transform.position = newPos;

        steps = 0;
    }
}
