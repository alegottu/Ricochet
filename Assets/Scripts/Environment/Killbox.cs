using UnityEngine;

public class Killbox : MonoBehaviour
{
	[SerializeField] private Vector2 padding; // Percent amount of camera's size

	private void Awake()
	{
		float size = Camera.main.orthographicSize;
		float x = size;
		float aspect = Screen.height / Screen.width;
		Vector2 _padding = new Vector2(size * padding.x, size * padding.y); 
		transform.localScale = new Vector2(x + _padding.x, x * aspect + _padding.y);
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
