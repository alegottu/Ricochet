using System.Collections;
using System;
using UnityEngine;

/* Improvements:
 * Speed up the ball and figure out how to make that fun
 * Make walls disappear after a set amount of time based on their size (larger = more time, introduces "wall cancels")
 * Take the best of all previous gamemodes and stick with making one great gamemode; to start, have it so that the ball takes health away if it goes out of bounds.
 * Have the "Special" button make 4 walls around the stage and a ghost ball when no wall is drawn; otherwise have the drawn wall perform a special cyclone attack.
   Additionally, the ball may have to slow down during this attack in order to somewhat decrease its risk.
 */

public class Player : MonoBehaviour
{
    public event Action<bool> OnChargeGained;
    public event Action OnRadiationDamage;

    [SerializeField] private Health health = null;
    [SerializeField] private PlayerData data = null;
    [SerializeField] private PlayerInput input = null;
    [SerializeField] private GameObject bulletPrefab = null;
    [SerializeField] private GameObject wallPrefab = null;
    [SerializeField] private GameObject barrierPrefab = null;

    private int chargesLeft = 0;
    private float radPercent = 0; // Each time the player draws a wall, it fills part of this meter, based on the size of the wall. If it overflows, they take damage.
    private GameObject bullet = null;
    private Wall wall = null;

    private void Awake()
    {
        input.onPenDown += CreateWall;
        input.onPenUp += FinishWall;
        input.onSpecial += ActivateSpecial;

        health.OnDeath += OnDeathEventHandler;

        chargesLeft = data.specialCharges;
        StartCoroutine(Recharge());
    }

    private void Shoot()
    {
        bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetUp(health);
    }

    private void FixedUpdate()
    {
        if (bullet == null)
        {
            Shoot();
        }
    }

    private void CreateWall()
    {
        if (wall != null)
        {
            Destroy(wall.gameObject);
        }

        wall = Instantiate(wallPrefab).GetComponent<Wall>();
        Vector2 start = input.GetMousePos();
        wall.SetPositions(start, start);
    }

    private void DrawWall()
    {
        Vector2 end = input.GetMousePos();
        wall.SetEnd(end);
    }

    private void FinishWall()
    {
        radPercent += wall.GetPercent();

        if (radPercent > 1)
        {
            health.TakeDamage(1);
            OnRadiationDamage?.Invoke();
            radPercent = 0;
        }

        wall.StartTimer();
    }

    // A special action where the bounds of the stage bounce the ball for a limited amount of time
    private void CreateBarrier()
    {
        Instantiate(barrierPrefab);
    }

    private void ActivateSpecial()
    {
        if (chargesLeft > 0)
        {
            if (wall == null)
            {
                CreateBarrier();
            }
            else
            {
                wall.Attack();
            }

            OnChargeGained?.Invoke(false);
            chargesLeft--;
        }
    }

    private IEnumerator Recharge()
    {
        while (true)
        {
            yield return new WaitForSeconds(data.rechargeTime);

            if (chargesLeft < data.specialCharges)
            {
                OnChargeGained?.Invoke(true);
                chargesLeft++;
            }
        }
    }

    private void OnDeathEventHandler()
    {
        Destroy(this);
    }

    private void Update()
    {
        if (input.penDown && wall != null)
        {
            DrawWall();
        }

        radPercent = Mathf.Max(0, radPercent - data.radDeplete * Time.deltaTime);
    }

    private void OnDisable()
    {
        input.onPenDown -= CreateWall;
        input.onPenUp -= FinishWall;
        input.onSpecial -= ActivateSpecial;
    }
}