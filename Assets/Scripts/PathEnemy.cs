using UnityEngine;

public class PathEnemy : Enemy
{
    [SerializeField] private float speedModifier = 1;
    public override EnemyType type => EnemyType.STANDARD;

    private void Start()
    {
        float x = Random.Range(speed.x / speedFluctation, speed.x);
        float total = speed.x + speed.y;
        speed = new Vector2(x, total - x);
        speed = new Vector2(Random.Range(0, 2) > 0 ? -speed.x : speed.x, -speed.y);
        speed *= speedModifier;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        transform.position += (Vector3)speed;
    }

    protected override void KillBox()
    {
        base.KillBox();

        if (transform.position.x > bounds.x / 2 || transform.position.x < -bounds.x / 2)
        {
            speed = new Vector2(-speed.x, speed.y);
        }
    }
}
