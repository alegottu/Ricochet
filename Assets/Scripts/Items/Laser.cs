using UnityEngine;

public class Laser : TemporaryObject
{
    [SerializeField] private float lifetime = 0;
    private void Awake()
    {
        StartCoroutine(Timer(lifetime));
    }
}
