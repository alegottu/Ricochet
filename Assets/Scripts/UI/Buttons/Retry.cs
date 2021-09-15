using UnityEngine;

public class Retry : MonoBehaviour, IMenuButton
{
    public void OnClick()
    {
        SceneController.Instance.UnloadObjects("Main");
        SceneController.Instance.UnloadLevel("Main");
        SceneController.Instance.SetTransitionActive(false);
        SceneController.Instance.LoadLevel("Main");
    }
}
