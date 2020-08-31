using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] float _maxMagnitude = 1;
    [SerializeField] float _minMagnitude = 0;
    private Vector3 originalPos = Vector3.zero;

    public Animator anim = null;

    private void Start()
    {
        originalPos = transform.position;
    }

    public void OnShake()
    {
        Vector3 direction = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
        float magnitude = Random.Range(_minMagnitude, _maxMagnitude);

        transform.position += direction * magnitude;
    }

    private void OnShakeComplete()
    {
        transform.position = originalPos;
    }
}
