using UnityEngine;

public class Killbox : MonoBehaviour
{
	[SerializeField] private Vector2 padding;

	private void Awake()
	{
		float x = Camera.main.orthographicSize;
		float aspect = Screen.height / Screen.width;
		transform.localScale = new Vector2(x + padding.x, x * aspect + padding.y);
	}

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out Enemy enemy))
        {
            enemy.Destroy();
        }
        if (collider.TryGetComponent(out Projectile projectile))
        {
            Destroy(projectile.gameObject);
        }
    }
}
