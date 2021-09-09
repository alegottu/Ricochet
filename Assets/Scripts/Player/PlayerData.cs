using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Player Data", order = 1)]
public class PlayerData : ScriptableObject
{
    [SerializeField] private GameObject _bulletPrefab = null;
    public GameObject bulletPrefab { get { return _bulletPrefab; } }

    [SerializeField] private GameObject _extraBulletPrefab = null;
    public GameObject extraBulletPrefab { get { return _extraBulletPrefab; } }

    [SerializeField] private GameObject _wallPrefab = null;
    public GameObject wallPrefab { get { return _wallPrefab; } }

    [SerializeField] private GameObject _barrierPrefab = null;
    public GameObject barrierPrefab { get { return _barrierPrefab; } }

    [SerializeField] private int _specialCharges = 0;
    public int specialCharges { get { return _specialCharges; } }

    [SerializeField] private int _rechargeTime = 0;
    public int rechargeTime { get { return _rechargeTime; } }

    [SerializeField] private float _maxWallLifetime = 0;
    public float maxWallLifetime { get { return _maxWallLifetime; } }

    [SerializeField] private float _wallAttackTime = 0;
    public float wallAttackTime { get { return _wallAttackTime; } }

    [SerializeField] private float _wallAttackSpeed = 0;
    public float wallAttackSpeed { get { return _wallAttackSpeed; } }

    [SerializeField] private float _photonRefill = 0; // The percentage amount by which the radiation meter refills over time
    public float photonRefill { get { return _photonRefill; } }

    [SerializeField] private float _photonMax = 0; // The photon gauge's max percentage value
    public float photonMax { get { return _photonMax; } }
}
