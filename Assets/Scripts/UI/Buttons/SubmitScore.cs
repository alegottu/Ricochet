using UnityEngine;

public class SubmitScore : MonoBehaviour, IMenuButton
{
    public void OnClick()
    {
        SceneController.Instance.LoadLevel("Leaderboard");
    }
}
