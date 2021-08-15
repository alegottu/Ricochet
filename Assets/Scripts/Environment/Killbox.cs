using UnityEngine;

public class Killbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out Enemy enemy))
        {
            enemy.Destroy();
        }
        if (collider.TryGetComponent(out Bullet bullet))
        {
            Destroy(bullet.gameObject);
        }
    }
}
