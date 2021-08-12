using System.Collections;
using UnityEngine;

public class TemporaryObject : MonoBehaviour
{
    protected IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
