using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Transform bounds = null;

    [HideInInspector] public int bounces = 0;
    private Rigidbody2D rb = null;

    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        KillBox();

        velocity = rb.velocity;
    }

    private void KillBox()
    {
        if (transform.position.x > bounds.localScale.x / 2 || transform.position.x < -bounds.localScale.x / 2 || transform.position.y > bounds.localScale.y / 2 || transform.position.y < -bounds.localScale.y / 2)
        {
            bounces = 0;
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float speed = velocity.magnitude;
        Vector3 direction = Vector3.Reflect(velocity.normalized, collision.contacts[0].normal);
        rb.velocity = direction * speed;

        bounces++;
    }
}
