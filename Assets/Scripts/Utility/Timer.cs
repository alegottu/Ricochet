using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private int time = 1;

    private void Awake()
    {
        StartCoroutine(TimeEnable());
    }

    private IEnumerator TimeEnable()
    {
        yield return new WaitForSeconds(time);

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        transform.DetachChildren();
        Destroy(gameObject);
    }
}
