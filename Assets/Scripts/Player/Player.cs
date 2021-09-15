using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Health health = null;
    [SerializeField] private PlayerMedia media = null;
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
        Shoot();

        chargesLeft = data.specialCharges;
        StartCoroutine(Recharge());
    }

    private void OnEnable()
    {
        input.onPenDown += CreateWall;
        input.onPenUp += FinishWall;
        input.onSpecial += ActivateSpecial;

        health.OnDeath += OnDeathEventHandler;

        Bullet.OnBulletDestroyed += OnBulletDestroyedEventHandler;
        ExtraBullet.OnExtraBulletDestroyed += OnExtraBulletDestroyedEventHandler;
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
            media.PlayEvent("Overheat", 2);
            photonsPercent = data.photonMax;
        }

        wall.StartTimer(data.maxWallLifetime);
    }

    // A special action where the bounds of the stage bounce the ball for a limited amount of time
    private void CreateBarrier()
    {
        Instantiate(data.barrierPrefab).GetComponent<Barrier>().Setup(bullet.transform);
        CameraController.Instance.StartShake(0.25f);
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
                wall.Attack(data.wallAttackTime, data.wallAttackSpeed);
                CameraController.Instance.StartShake(data.wallAttackTime);
            }

            media.UpdateSpecialMeter("Deplete");
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
                media.UpdateSpecialMeter("Fill");
                media.PlaySound(3);
                chargesLeft++;
            }
        }
    }

    private void OnDeathEventHandler()
    {
        Destroy(this);
    }

    private void Shoot()
    {
        CameraController.Instance.StartKick(new Vector2(0, -1), 2, 0.75f);
        bullet = Instantiate(data.bulletPrefab, transform.position, Quaternion.identity);
        media.PlayRandomSound(4, 2);
    }

    public void ShootExtraBullet()
    {
        Instantiate(data.extraBulletPrefab, transform.position, Quaternion.identity);
        media.PlaySound(4);
    }

    private void OnBulletDestroyedEventHandler()
    {
        health.TakeDamage(1);
        Shoot();
    }

    private void OnExtraBulletDestroyedEventHandler()
    {
        ShootExtraBullet();
    }

    private void Update()
    {
        if (input.penDown && wall != null)
        {
            DrawWall();
        }

        photonsPercent = Mathf.Min(data.photonMax, photonsPercent + data.photonRefill * Time.deltaTime);
        media.UpdatePhotonMeter(photonsPercent);
    }

    private void OnDisable()
    {
        input.onPenDown -= CreateWall;
        input.onPenUp -= FinishWall;
        input.onSpecial -= ActivateSpecial;

        health.OnDeath -= OnDeathEventHandler;

        Bullet.OnBulletDestroyed -= OnBulletDestroyedEventHandler;
        ExtraBullet.OnExtraBulletDestroyed -= OnExtraBulletDestroyedEventHandler;
    }
}