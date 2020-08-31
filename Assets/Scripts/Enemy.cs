using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected Vector2 bounds = Vector2.one;
    [SerializeField] protected Animator anim = null;
    protected EnemySpawn enemySpawn = null;
    private Player player = null;

    public enum EnemyType
    {
        STANDARD,
        PATH,
        TELEPORT
    }
    public abstract EnemyType type { get; }

    protected Vector2 speed = Vector2.zero;
    protected float speedFluctation = 2;
    private bool damage = false;
    private bool killed = false;

    private void Awake()
    {
        bounds = GameManager.Instance.bounds.bounds.size;
        enemySpawn = transform.parent.GetComponent<EnemySpawn>();
    }

    protected virtual void KillBox()
    {
        if (transform.position.y < -bounds.y / 2)
        {
            if (player.lives == 1 + 1) // lives have not been taken away yet, but position needs to be detected
            {
                UIManager.Instance.anim.SetTrigger("damage");
            }

            UIManager.Instance.ShakeCamera();
            anim.SetTrigger("kill");
            damage = true;
        }
    }

    protected virtual void FixedUpdate()
    {
        KillBox();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        anim.SetTrigger("kill");
        killed = true;
    }

    private void OnDeathComplete()
    {
        enemySpawn.current--;
        enemySpawn.max = Mathf.Clamp(enemySpawn.total % enemySpawn.increment == 0 ? enemySpawn.max + 1 : enemySpawn.max, 0, enemySpawn.ceiling);

        if (damage && !killed)
        {
            player.lives--;
        }

        damage = false;
        EnemySpawn.currentEnemies.Remove(this);
        Destroy(gameObject);
    }

    public void setSpeed(Vector2 speed)
    {
        this.speed = speed;
    }

    public void setPlayer(Player player)
    {
        this.player = player;
    }
}
