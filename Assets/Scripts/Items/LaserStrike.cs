using UnityEngine;

public class LaserStrike : Item
{
    [SerializeField] private GameObject laserPrefab = null;
    private Vector2 direction = Vector2.zero;

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        direction = Vector2.Perpendicular(collider.attachedRigidbody.velocity);
        base.OnTriggerEnter2D(collider);
    }

    protected override void CastEffect()
    {       
        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
        laser.transform.eulerAngles = Vector3.forward * Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
