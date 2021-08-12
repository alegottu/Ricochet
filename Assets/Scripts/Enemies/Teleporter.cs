using System.Collections;
using UnityEngine;

public class Teleporter : Enemy
{
    [SerializeField] private EnemyMedia media = null;
    [SerializeField] private float teleportCooldown = 0;

    private void Awake()
    {
        StartCoroutine(Teleport());
    }

    private IEnumerator Teleport()
    {
        while (true)
        {
            yield return new WaitForSeconds(teleportCooldown);
            media.SetTrigger("Teleport");
        }
    }
}
