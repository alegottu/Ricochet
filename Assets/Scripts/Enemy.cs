using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform bounds = null;
    [SerializeField] private Animator anim = null;

    private int type = 0;

    private Vector2 speed;
    [SerializeField] private float speedFluctation = 2;

    private bool damage = false;

    private void Start()
    {
        speed = EnemySpawn.Instance.speed;
        type = Random.Range(1, 4);

        float x = Random.Range(speed.x / speedFluctation, speed.x);
        float total = speed.x + speed.y;
        speed = new Vector2(x, total - x);
    }

    private void FixedUpdate()
    {
        switch(type)
        {
            case 1:
                transform.position -= new Vector3(0, speed.y, 0);
                break;
            case 2:
                transform.position -= new Vector3(speed.x, speed.y, 0);
                break;
            case 3:
                transform.position -= new Vector3(-speed.x, speed.y, 0);
                break;
            default:
                transform.position -= new Vector3(0, speed.y, 0);
                break;
        }

        KillBox();
    }

    private void KillBox()
    {
        if (transform.position.x > bounds.localScale.x / 2 || transform.position.x < -bounds.localScale.x / 2)
        {
            speed = new Vector2(-speed.x, speed.y);
        }

        if (transform.position.y < -bounds.localScale.y / 2)
        {
            anim.SetTrigger("kill");
            damage = true;

            CameraShake.Instance.Shake();

            if (Player.Instance.lives == 1)
            {
                UIManager.Instance.anim.SetTrigger("damage");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        anim.SetTrigger("kill");
    }

    private void OnDeathComplete()
    {
        EnemySpawn.Instance.current--;
        EnemySpawn.Instance.max = Mathf.Clamp(EnemySpawn.Instance.max + EnemySpawn.Instance.total / 4, 1, EnemySpawn.Instance.ceiling);

        if (damage)
        {
            Player.Instance.lives--;
            CameraShake.Instance.Setup();
        }

        damage = false;
        Destroy(gameObject);
    }
}
