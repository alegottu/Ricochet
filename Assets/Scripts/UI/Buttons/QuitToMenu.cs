using UnityEngine;

public class QuitToMenu : MonoBehaviour, IMenuButton
{
    public void OnClick()
    {
        SceneController.Instance.SetTransitionActive(true);
        SceneController.Instance.LoadLevel("Menu");
    }
}
