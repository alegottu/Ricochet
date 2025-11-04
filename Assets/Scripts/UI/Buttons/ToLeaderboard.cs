using UnityEngine;

public class ToLeaderboard : MonoBehaviour, IMenuButton
{
    public void OnClick()
    {
        SceneController.Instance.LoadLevel("Static Leaderboard");
    }
}
