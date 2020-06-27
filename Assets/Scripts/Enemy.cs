using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 1;

    [SerializeField] private Transform bounds = null;

    private Animator anim = null;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        transform.position -= new Vector3(0, speed, 0);

        KillBox();
    }

    private void KillBox()
    {
        if (transform.position.x > bounds.localScale.x / 2 || transform.position.x < -bounds.localScale.x / 2 || transform.position.y > bounds.localScale.y / 2)
        {
            Kill();
        }

        if (transform.position.y < -bounds.localScale.y / 2)
        {
            Kill();

            if (GameManager.Instance.getMode() == GameManager.GameMode.SURVIVAL || GameManager.Instance.getMode() == GameManager.GameMode.HARDCORE)
            {
                Player.Instance.lives--;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Kill();
    }

    private void Kill()
    {
        EnemySpawn.Instance.current--;
        EnemySpawn.Instance.max = Mathf.Clamp(EnemySpawn.Instance.max + EnemySpawn.Instance.total / 4, 1, EnemySpawn.Instance.ceiling);

        anim.SetBool("kill", true);
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            Destroy(gameObject);
        }
    }
}
