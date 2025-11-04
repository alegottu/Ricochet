using System.Collections;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected static readonly Vector2Int bulletAngleRange = new Vector2Int(75, 105);
    protected const float speed = 15;

    [SerializeField] protected Rigidbody2D rb = null;
    [SerializeField] protected AudioSource sfx = null;

	private const float invulnTime = 0.5f;

	private IEnumerator TempInvuln()
	{
		// NOTE: Starts out on a layer ignored by Killbox
		yield return new WaitForSeconds(invulnTime);
		gameObject.layer = LayerMask.NameToLayer("Bullets");
	}

    private void Start()
    {
        StartCoroutine(TempInvuln());
	}

    protected void SetVelocity(int angle)
    {
        transform.eulerAngles = Vector3.forward * angle;
        rb.linearVelocity = transform.right * speed;
    }

    private void OnCollisionExit2D(Collision2D _)
    {
        rb.linearVelocity = rb.linearVelocity.normalized * speed; // To ensure the bullet never slows down off of odd collisions

        sfx.pitch = Random.Range(1f, 2f);
        sfx.PlayOneShot(sfx.clip);

        CameraController.Instance.StartShake(0.1f, 0.5f);
    }
}
