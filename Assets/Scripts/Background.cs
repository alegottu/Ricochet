using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private Material bg = null;

    [SerializeField] private float speed = 1;

    private void Start()
    {
        bg = gameObject.GetComponent<Renderer>().material;
    }

    private void Update()
    {
        bg.mainTextureOffset += new Vector2(0, speed);
    }
}
