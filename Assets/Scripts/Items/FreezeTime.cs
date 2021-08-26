using UnityEngine;

public class FreezeTime : Item
{
    [SerializeField] private LayerMask enemyLayer = new LayerMask();

    protected override void CastEffect()
    {
        foreach (RaycastHit2D enemyHit in Physics2D.BoxCastAll(Vector2.zero, Bounds.size, 0, Vector2.zero, 0, enemyLayer))
        {
            enemyHit.collider.gameObject.AddComponent<FreezeObject>();
        }
    }
}
