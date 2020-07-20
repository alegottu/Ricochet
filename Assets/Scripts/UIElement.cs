using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class UIElement : MonoBehaviour
{
    [SerializeField] private Vector2 designRes = Vector2.zero;

    void Start()
    {
        gameObject.transform.localScale = Resize2D(gameObject.transform.localScale);
    }

    private Vector3 Resize2D(Vector3 target)
    {
        float factorY = designRes.x / Screen.width;
        float factorX = designRes.y / Screen.height;

        return new Vector3(target.x * factorX, target.x * factorY, target.z);
    }
}
