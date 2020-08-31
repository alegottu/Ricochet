using UnityEngine;

public class MenuButton : MonoBehaviour
{
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

    #if UNITY_ANDROID
    public void SubmitScore()
    {
        GooglePlayController.Instance.UploadScore(GameManager.Instance.getMode() == GameManager.GameMode.ARCADE ? GameManager.Instance.getScore() : UIManager.Instance.time, GameManager.Instance.getMode());
    }

    public void SignOut()
    {
        GooglePlayController.Instance.SignOut();
    }

    public void ShowLeaderboards()
    {
        GooglePlayController.Instance.ShowLeaderboards();
    }
    #endif
}
