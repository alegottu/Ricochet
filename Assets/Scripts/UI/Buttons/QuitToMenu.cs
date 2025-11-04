using UnityEngine;

public class QuitToMenu : MonoBehaviour, IMenuButton
{
    public void OnClick()
    {
        SceneController.Instance.LoadLevel("Menu");
    }
}
