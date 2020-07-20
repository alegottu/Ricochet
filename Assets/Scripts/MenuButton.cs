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

    public void menuInfo()
    {
        UIManager.Instance.displayInfo(this.gameObject);
    }

    public void SubmitScore()
    {
        GooglePlayController.Authenticate();
        GooglePlayController.UploadScore(GameManager.Instance.getMode() == GameManager.GameMode.ARCADE ? GameManager.Instance.getScore() : UIManager.Instance.time, GameManager.Instance.getMode());
    }

    public void Test()
    {
        GooglePlayController.Test();
    }
}
