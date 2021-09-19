using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Player player = null;
    [SerializeField] private Camera mainCam = null;

#if UNITY_ANDROID || UNITY_IOS

    private Touch currentTouch;

    public Vector2 GetCursorPosition()
    {
        return mainCam.ScreenToWorldPoint(currentTouch.position);
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            currentTouch = Input.GetTouch(0);
            
            if (currentTouch.phase == TouchPhase.Began)
            {
                if (Input.touchCount > 1)
                {
                    player.ActivateSpecial();
                }
                else
                {
                    player.CreateWall();
                }
            }
            else if (currentTouch.phase == TouchPhase.Ended)
            {
                player.FinishWall();
            }
        }
    } 

#elif UNITY_STANDALONE || UNITY_EDITOR

    public Vector2 GetCursorPosition()
    {
        return mainCam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void Update()
    {
        if (InputManager.Instance.GetButtonDown("Draw"))
        {
            player.CreateWall();
        }
        else if (InputManager.Instance.GetButtonUp("Draw"))
        {
            player.FinishWall();
        }

        if (InputManager.Instance.GetButtonDown("Special"))
        {
            player.ActivateSpecial();
        }
    }   

#endif
}
