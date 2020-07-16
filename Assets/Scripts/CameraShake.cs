using UnityEngine;

public class CameraShake : Singleton<CameraShake>
{
    [SerializeField] private float maxDuration = 0;
    [SerializeField] private float magnitude = 0;
    private float duration = 0f;

    private Vector3 originalPos = Vector3.zero;
    private Transform cam = null;

    void Start()
    {
        Setup();
    }

    public void Setup()
    {
        duration = maxDuration;
    }

    public void SetCamera(Transform camera)
    {
        cam = camera;
        originalPos = cam.position;
    }

    public void Shake()
    {
        if (duration > 0)
        {
            cam.position = originalPos + Random.insideUnitSphere * magnitude;
            duration -= Time.deltaTime;
        }
        else
        {
            cam.position = originalPos;
        }
    }
}
