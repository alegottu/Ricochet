﻿using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemy Data", order = 3)]
public class EnemyData : ScriptableObject
{
    [SerializeField] private float _speed = 1;
    public float speed { get { return _speed; } }

    [SerializeField] protected float _speedFluctation = 1; // The speed of an enemy can be multiplied by 1 up to this value
    public float speedFluctation { get { return _speedFluctation; } }

    [SerializeField] private Vector2 _direction = Vector2.zero; // x direction is randomized with a maximum of the value given here
    public Vector2 direction { get { return _direction; } }
}