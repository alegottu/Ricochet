using UnityEngine;

public class Bounds : MonoBehaviour
{
    public static Vector2 size = new Vector2(28, 50); // Size of the entire stage's bounds

    private void Awake()
    {
        transform.localScale = size;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out Enemy enemy))
        {
            enemy.Turn();
        }
    }
}
