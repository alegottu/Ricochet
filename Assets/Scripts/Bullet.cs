using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Transform bounds = null;

    [HideInInspector] public int bounces = 0;
    private Rigidbody2D rb = null;

    [SerializeField] private float speed = 1;
    private Vector3 direction = Vector3.zero;

    [SerializeField] private int enemyPoints = 1;
    private int enemyMultiplier = 1;
    [SerializeField] private int timeBonusScale = 1;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        direction = new Vector2(Random.Range(-0.5f, 0.5f), 1);
    }

    private void FixedUpdate()
    {
        KillBox();

        transform.position += (direction * speed);
    }

    private void KillBox()
    {
        if (transform.position.x > bounds.localScale.x / 2 || transform.position.x < -bounds.localScale.x / 2 || transform.position.y > bounds.localScale.y / 2 || transform.position.y < -bounds.localScale.y / 2)
        {
            bounces = 0;
            Destroy(gameObject);

            if (GameManager.Instance.getMode() == GameManager.GameMode.HARDCORE)
            {
                Player.Instance.lives--;
                UIManager.Instance.anim.SetTrigger("ammo");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        direction = Vector3.Reflect(direction, collision.contacts[0].normal);

        bounces++;

        if (collision.gameObject.name.Contains("nemy") && GameManager.Instance.getMode() == GameManager.GameMode.ARCADE)
        {
            int timeBonus = collision.gameObject.transform.position.y - bounds.position.y > 0 ? (int)(collision.gameObject.transform.position.y - bounds.position.y) * timeBonusScale : 1;

            UIManager.Instance.updateScore(transform.position, enemyPoints * enemyMultiplier * timeBonus);
            GameManager.Instance.addScore(enemyPoints * enemyMultiplier * timeBonus);
            enemyMultiplier++;
        }
    }
}
