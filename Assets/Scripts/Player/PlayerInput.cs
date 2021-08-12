using System;
using UnityEngine;

// if only this class uses InputManager, consider not having it be a singleton
public class PlayerInput : MonoBehaviour
{
    public event Action onPenDown;
    public event Action onPenUp;
    public event Action onSpecial;

    public bool penDown { get; private set; }

    [SerializeField] private Camera mainCam = null;

    public Vector2 GetMousePos()
    {
        return mainCam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void Update()
    {
        penDown = InputManager.Instance.GetButton("Draw");

        if (InputManager.Instance.GetButtonDown("Draw"))
        {
            onPenDown?.Invoke();
        }

        if (InputManager.Instance.GetButtonUp("Draw"))
        {
            onPenUp?.Invoke();
        }

        if (InputManager.Instance.GetButtonDown("Special"))
        {
            onSpecial?.Invoke();
        }
    }
}
