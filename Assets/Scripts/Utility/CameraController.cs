using System.Collections;
using UnityEngine;

// Allowed as singleton since it only controls effects, rename if necessary
public class CameraController : Singleton<CameraController>
{
    private static Vector3 originalPos;

    private void Start()
    {
        originalPos = transform.position;
    }

    public void StartShake(float duration, float magnitude=1)
    {
        StartCoroutine(Shake(duration, magnitude));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        float magnitudeFadeTime = magnitude / duration;

        for (float timer = duration; timer > 0; timer -= Time.deltaTime)
        {
            transform.position = originalPos + Random.insideUnitSphere * magnitude;
            transform.position = new Vector3(transform.position.x, transform.position.y, originalPos.z);

            yield return new WaitForFixedUpdate();
        }

        transform.position = originalPos;
    }

    public void StartKick(Vector2 direction, float magnitude=1, float velocity=1)
    {
        StartCoroutine(Kick(direction, magnitude, velocity));
    }

    private IEnumerator Kick(Vector2 direction, float magnitude, float velocity)
    {
        Vector3 target = originalPos + (Vector3)direction * magnitude;

        while (transform.position != target)
        {
            transform.position = Vector3.Lerp(transform.position, target, velocity);

            yield return new WaitForFixedUpdate();
        }

        while (transform.position != originalPos)
        {
            transform.position = Vector3.Lerp(transform.position, originalPos, velocity / 4);

            yield return new WaitForFixedUpdate();
        }
    }
}
