using UnityEngine;

public class Comet : MonoBehaviour
{
    [SerializeField] private Vector2 maxScale = Vector2.one;
    [SerializeField] private Vector2 scaleDivisorRange = Vector2.zero; // x = minimum, y = maximum

    private void Start()
    {
        float amountX = Bounds.size.x * Random.Range(-0.5f, 0.5f);
        float amountY = Bounds.size.y * Random.Range(-0.5f, 0.5f);
        transform.position = new Vector3(amountX, amountY);
        transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));

        float scaleDivisor = Random.Range(scaleDivisorRange.x, scaleDivisorRange.y);
        transform.localScale = new Vector3(maxScale.x / scaleDivisor, maxScale.y / scaleDivisor);
    }

    private void OnAnimationEnd() // For use as an animation event
    {
        Destroy(gameObject);
    }
}
