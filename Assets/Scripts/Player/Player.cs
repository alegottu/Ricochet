using System.Collections;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event Action<bool> OnChargeGained;
    public event Action OnRadiationDamage;
    public event Action<float> OnRadiationGain;

    [SerializeField] private Health health = null;
    [SerializeField] private PlayerData data = null;
    [SerializeField] private PlayerInput input = null;
    [SerializeField] private GameObject bulletPrefab = null;
    [SerializeField] private GameObject wallPrefab = null;
    [SerializeField] private GameObject barrierPrefab = null;

    private int chargesLeft = 0;
    private float radiationPercent = 0; // Each time the player draws a wall, it fills part of this meter, based on the size of the wall. If it overflows, they take damage.
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
        radiationPercent += wall.GetPercent();
        OnRadiationGain?.Invoke(radiationPercent);

        if (radiationPercent > data.radThreshold)
        {
            health.TakeDamage(1);
            OnRadiationDamage?.Invoke();
            radiationPercent = 0;
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

        radiationPercent = Mathf.Max(0, radiationPercent - data.radDeplete * Time.deltaTime);
    }

    private void OnDisable()
    {
        input.onPenDown -= CreateWall;
        input.onPenUp -= FinishWall;
        input.onSpecial -= ActivateSpecial;
    }
}