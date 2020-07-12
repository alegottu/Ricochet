using UnityEngine;
using System.Collections;

public class CameraShake : Singleton<CameraShake>
{
    [SerializeField] private float maxDuration = 0;
    [SerializeField] private float magnitude = 0;
    private float duration = 0f;
    private Vector3 originalPos = Vector3.zero;
    private Transform cam = null;

    //shake per life lost and special effect when on last life
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
        originalPos = cam.transform.position;
    }

    public void Shake()
    {
        if (duration > 0)
        {
            cam.transform.position = originalPos + Random.insideUnitSphere * magnitude;
            duration -= Time.deltaTime;
        }
        else
        {
            cam.transform.position = originalPos;
        }
    }
}
