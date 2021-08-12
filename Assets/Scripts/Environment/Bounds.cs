using UnityEngine;

public class Bounds : MonoBehaviour
{
    public static Vector2 size = new Vector2(28, 50);

    private void Awake()
    {
        transform.localScale = size;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out Bullet bullet))
        {
            Destroy(bullet.gameObject);
        }
        if (collider.TryGetComponent(out Enemy enemy))
        {
            enemy.Destroy();
        }
    }
}
