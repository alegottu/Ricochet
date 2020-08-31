using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 bounds = Vector2.one;
    [SerializeField] private Player player = null;

    [HideInInspector] public int bounces = 0;
    private float speed = 1;
    private Vector3 direction = Vector3.zero;

    [SerializeField] private int enemyPoints = 1;
    private int enemyMultiplier = 1;
    [SerializeField] private int timeBonusScale = 1;

    private void Start()
    {
        bounds = GameManager.Instance.bounds.bounds.size;
        direction = new Vector2(Random.Range(-0.5f, 0.5f), 1);
    }

    private void FixedUpdate()
    {
        KillBox();

        transform.position += (direction * speed);
    }

    private void KillBox()
    {
        if (transform.position.x > bounds.x / 2 || transform.position.x < -bounds.x / 2 || transform.position.y > bounds.y / 2 || transform.position.y < -bounds.y / 2)
        {
            bounces = 0;
            Destroy(gameObject);

            if (GameManager.Instance.getMode() == GameManager.GameMode.HARDCORE)
            {
                player.lives--;
                UIManager.Instance.anim.SetTrigger("ammo");
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        direction = Vector3.Reflect(direction, collision.contacts[0].normal);

        bounces++;

        if (collision.gameObject.name.Contains("nemy") && GameManager.Instance.getMode() == GameManager.GameMode.ARCADE)
        {
            int timeBonus = collision.gameObject.transform.position.y > 0 ? (int)(collision.gameObject.transform.position.y) * timeBonusScale : 1; // if center of map or desired measuring point is at zero

            UIManager.Instance.updateScore(transform.position, enemyPoints * enemyMultiplier * timeBonus);
            GameManager.Instance.addScore(enemyPoints * enemyMultiplier * timeBonus);
            enemyMultiplier++;
        }
    }

    public void setSpeed(float speed)
    {
        this.speed = speed;
    }
}
