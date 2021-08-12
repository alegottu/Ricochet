using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Player Data", order = 1)]
public class PlayerData : ScriptableObject
{
    [SerializeField] private int _specialCharges = 0;
    public int specialCharges { get { return _specialCharges; } }

    [SerializeField] private int _rechargeTime = 0;
    public int rechargeTime { get { return _rechargeTime; } }

    [SerializeField] private float _bulletSpeed = 0;
    public float bulletSpeed { get { return _bulletSpeed; } }

    [SerializeField] private Vector2Int _bulletAngleRange = Vector2Int.zero; // x is the minimum angle and y is the maximum angle for the bullet to come out
    public Vector2Int bulletAngleRange { get { return _bulletAngleRange; } }

    [SerializeField] private float _maxWallLifetime = 0;
    public float maxWallLifetime { get { return _maxWallLifetime; } }

    [SerializeField] private float _wallAttackTime = 0;
    public float wallAttackTime { get { return _wallAttackTime; } }

    [SerializeField] private float _wallAttackSpeed = 0;
    public float wallAttackSpeed { get { return _wallAttackSpeed; } }

    [SerializeField] private float _radDeplete = 0; // The percentage amount by which the radiation meter depletes over time
    public float radDeplete { get { return _radDeplete; } }
}
