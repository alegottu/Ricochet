using UnityEngine;

public class Bounds : MonoBehaviour
{
    public static Vector2 size; // Size of the entire stage's bounds

    private void Awake()
    {
		float camSize = Camera.main.orthographicSize;
		float x = camSize;
		float aspect = Screen.height / Screen.width;
		size = new Vector2(x, x * aspect);
		transform.localScale = size;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out Enemy enemy))
        {
            enemy.Reflect();
        }
    }
}
