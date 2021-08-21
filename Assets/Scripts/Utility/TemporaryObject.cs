using System.Collections;
using UnityEngine;

public abstract class TemporaryObject : MonoBehaviour
{
    [SerializeField] private Animator anim = null;

    protected IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy();
    }

    protected void Destroy()
    {
        anim.SetTrigger("Warning"); // Make the time of this animation predictable so that it can be cut off from lifetime
    }

    private void OnDestroyAnimationEnd() // For use as an animation event
    {
        Destroy(gameObject);
    }
}
