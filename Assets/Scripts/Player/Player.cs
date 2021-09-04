using System.Collections;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static event Action OnDamageTaken;
    public event Action<bool> OnChargeGained;
    public event Action OnOverheatDamage;
    public event Action<float> OnPhotonLoss;

    [SerializeField] private Health health = null;
    [SerializeField] private PlayerData data = null;
    [SerializeField] private PlayerInput input = null;

    private float photonsPercent = 1; // Each time the player draws a wall, it takes away part of this meter, based on the size of the wall. If it runs out, they take damage.
    private int chargesLeft = 0;
    private float rechargeDecrease = 0;
    private GameObject bullet = null;
    private Wall wall = null;

    private void Awake()
    {
        photonsPercent = data.photonMax;
        bullet = Instantiate(data.bulletPrefab, transform.position, Quaternion.identity);

        chargesLeft = data.specialCharges;
        StartCoroutine(Recharge());
    }

    private void OnEnable()
    {
        input.onPenDown += CreateWall;
        input.onPenUp += FinishWall;
        input.onSpecial += ActivateSpecial;

        health.OnDeath += OnDeathEventHandler;
        health.OnDamageTaken += OnDamageTaken.Invoke; 

        Bullet.OnBulletDestroyed += OnBulletDestroyedEventHandler;
        ExtraBullet.OnExtraBulletDestroyed += OnExtraBulletDestroyedEventHandler;
    }

    public void AddExtraBullet()
    {
        Instantiate(data.extraBulletPrefab, transform.position, Quaternion.identity);
    }

    private void CreateWall()
    {
        if (wall != null)
        {
            Destroy(wall.gameObject);
        }

        wall = Instantiate(data.wallPrefab).GetComponent<Wall>();
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
        photonsPercent -= wall.GetPercent();

        if (photonsPercent <= 0)
        {
            health.TakeDamage(1);
            OnOverheatDamage?.Invoke();
            photonsPercent = data.photonMax;
        }

        wall.StartTimer();
    }

    // A special action where the bounds of the stage bounce the ball for a limited amount of time
    private void CreateBarrier()
    {
        Instantiate(data.barrierPrefab).GetComponent<Barrier>().Setup(bullet.transform);
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

    public void SetRechargeDecrease(float rechargeDecrease)
    {
        this.rechargeDecrease = rechargeDecrease;
    }

    private IEnumerator Recharge()
    {
        while (true)
        {
            yield return new WaitForSeconds(data.rechargeTime - rechargeDecrease);

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

    private void OnBulletDestroyedEventHandler()
    {
        health.TakeDamage(1);
        bullet = Instantiate(data.bulletPrefab, transform.position, Quaternion.identity);
    }

    private void OnExtraBulletDestroyedEventHandler()
    {
        Instantiate(data.extraBulletPrefab, transform.position, Quaternion.identity);
    }

    private void Update()
    {
        if (input.penDown && wall != null)
        {
            DrawWall();
        }

        photonsPercent = Mathf.Min(data.photonMax, photonsPercent + data.photonRefill * Time.deltaTime);
        OnPhotonLoss?.Invoke(photonsPercent);
    }

    private void OnDisable()
    {
        input.onPenDown -= CreateWall;
        input.onPenUp -= FinishWall;
        input.onSpecial -= ActivateSpecial;

        health.OnDeath -= OnDeathEventHandler;
        health.OnDamageTaken -= OnDamageTaken.Invoke; 

        Bullet.OnBulletDestroyed -= OnBulletDestroyedEventHandler;
        ExtraBullet.OnExtraBulletDestroyed -= OnExtraBulletDestroyedEventHandler;
    }
}