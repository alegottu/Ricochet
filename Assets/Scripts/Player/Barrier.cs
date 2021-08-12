using UnityEngine;

public class Barrier : TemporaryObject
{
    [SerializeField] private float lifetime = 0;

    private void Awake()
    {
        StartCoroutine(Timer(lifetime));
    }
}
