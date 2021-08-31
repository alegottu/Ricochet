using UnityEngine;

public class BulletFlurry : Item
{
    [SerializeField] private GameObject bulletPrefab = null; // Use ghost bullet
    [SerializeField] private int bulletsAmount = 0;

    protected override void CastEffect()
    {
        int deltaAngle = 360 / bulletsAmount;

        for (int i = 0; i < bulletsAmount; i++)
        {
            Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<GhostBullet>().Setup(deltaAngle * i);
        }
    }
}
