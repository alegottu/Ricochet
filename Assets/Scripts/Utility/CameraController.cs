using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public void StartShake(float duration, float magnitude=1)
    {
        StartCoroutine(Shake(duration, magnitude));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        float magnitudeDelta = magnitude / duration;

        for (float timer = duration; timer > 0; timer -= Time.deltaTime)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * magnitude;
            magnitude -= magnitudeDelta;

            yield return new WaitForEndOfFrame();
        }

        transform.localPosition = originalPos;
    }

    public void StartKick(Vector2 direction, float magnitude=1, float velocity=1)
    {
        StartCoroutine(Kick(direction, magnitude, velocity));
    }

    private IEnumerator Kick(Vector2 direction, float magnitude, float velocity)
    {
        Vector3 originalPos = transform.localPosition;
        Vector3 target = originalPos + (Vector3)direction * magnitude;

        while (transform.localPosition != target)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, target, velocity);

            yield return new WaitForEndOfFrame();
        }

        while (transform.localPosition != originalPos)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, velocity / 4);

            yield return new WaitForEndOfFrame();
        }
    }
}
