using System.Collections;
using UnityEngine;

public class Wall : TemporaryObject
{
    [SerializeField] private PlayerData data = null;
    [SerializeField] private EdgeCollider2D edge = null;
    [SerializeField] private LineRenderer render = null;

    private bool attacking = false;

    // The distance of this wall turned into a percentage of the greatest possible length of a wall, found by the pythagorean theorem using the stage's scale
    public float GetPercent()
    {
        return Vector2.Distance(edge.points[0], edge.points[1]) / Mathf.Sqrt(Mathf.Pow(Bounds.size.x, 2) + Mathf.Pow(Bounds.size.y, 2));
    }

    public void SetPositions(Vector2 start, Vector2 end)
    {
        edge.points = new Vector2[2] { start, end };
        render.SetPositions(new Vector3[2] { start, end });
    }

    public void SetEnd(Vector2 point)
    {
        edge.points = new Vector2[2] { edge.points[0], point };
        render.SetPosition(1, point);
    }
   
    public void StartTimer()
    {
        float lifetime = data.maxWallLifetime * GetPercent();
        StartCoroutine(Timer(lifetime));
    }

    public void Attack()
    {
        StopAllCoroutines();
        StartCoroutine(Cyclone());
        attacking = true;
    }

    private IEnumerator Cyclone()
    {
        Vector3 midpoint = new Vector3((edge.points[0].x + edge.points[1].x) / 2, (edge.points[0].y + edge.points[1].y) / 2);

        for (float time = data.wallAttackTime; time > 0; time -= Time.deltaTime)
        {
            transform.RotateAround(midpoint, Vector3.forward, data.wallAttackSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!attacking)
        {
            if (collision.TryGetComponent(out Bullet bullet))
            {
                Vector2 direction = (edge.points[1] - edge.points[0]).normalized;
                bullet.Reflect(Vector2.Perpendicular(direction));
            }
        }
        else
        {
            if (collision.TryGetComponent(out Enemy enemy))
            {
                enemy.Kill();
            }
        }
    }
}
