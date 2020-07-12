using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public void menuClick()
    {
        UIManager.Instance.playMode(this.gameObject);
    }

    public void Pause()
    {
        GameManager.Instance.TogglePause();
    }

    public void Restart()
    {
        GameManager.Instance.Restart();
    }

    public void Quit()
    {
        GameManager.Instance.Quit();
    }
}
