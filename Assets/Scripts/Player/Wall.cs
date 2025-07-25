﻿using System;
using System.Collections;
using UnityEngine;

public class Wall : TemporaryObject
{
    public static event Action<float> OnWallAttack; // float for communicating attack duration

    [SerializeField] private EdgeCollider2D edge = null;
    [SerializeField] private LineRenderer render = null;
    [SerializeField] private AudioSource sfx = null;

    // The distance of this wall turned into a percentage of the greatest possible length of a wall, found by the pythagorean theorem using the stage's scale
    public float GetPercent()
    {
        return Vector2.Distance(edge.points[0], edge.points[1]) / Mathf.Sqrt(Mathf.Pow(Bounds.size.x, 2) + Mathf.Pow(Bounds.size.y, 2));
    }

    public void SetPositions(Vector2 start, Vector2 end)
    {
        edge.points = new Vector2[2] { start, end };
        render.SetPositions(new Vector3[2] { start, end });
    }

    public void SetEnd(Vector2 point)
    {
        edge.points = new Vector2[2] { edge.points[0], point };
        render.SetPosition(1, point);
    }

    public void StartTimer(float maxLifetime)
    {
        sfx.Stop();
        StartCoroutine(Timer( maxLifetime * GetPercent() ));
    }

    public void Attack(float duration, float speed)
    {
        gameObject.layer = 0; // Changes layers so it can collide with Enemies

        StopAllCoroutines();
        StartCoroutine(Cyclone(duration, speed));
        
        OnWallAttack?.Invoke(duration);
    }

    private IEnumerator Cyclone(float duration, float speed)
    {
        Vector3 midpoint = new Vector3((edge.points[0].x + edge.points[1].x) / 2, (edge.points[0].y + edge.points[1].y) / 2);

        for (float time = duration; time > 0; time -= Time.deltaTime)
        {
            transform.RotateAround(midpoint, Vector3.forward, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        Destroy();
    }
}
