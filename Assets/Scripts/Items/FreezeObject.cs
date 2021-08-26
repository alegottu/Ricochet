using System.Collections;
using UnityEngine;

public class FreezeObject : MonoBehaviour
{
    private float freezeTime = 5; // Hard-code a default value here if you dont want to use Setup()

    public void Setup(float freezeTime)
    {
        this.freezeTime = freezeTime;
    }

    private void Awake()
    {
        StartCoroutine(FreezeTimer());        
    }

    private IEnumerator FreezeTimer()
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        Vector2 originalVelocity = rb.velocity;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(freezeTime);

        rb.velocity = originalVelocity;
        Destroy(this);
    }
}
