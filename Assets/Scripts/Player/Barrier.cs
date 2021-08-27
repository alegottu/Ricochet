using UnityEngine;

public class Barrier : TemporaryObject
{
    [SerializeField] private float lifetime = 0;
    [SerializeField] private GameObject ghostBulletPrefab = null;

    public void Setup(Transform bullet)
    {
        StartCoroutine(Timer(lifetime));

        Instantiate(ghostBulletPrefab, bullet.transform.position, Quaternion.identity).GetComponent<GhostBullet>()
            .Setup(Random.Range(0, 360));
    }
}
