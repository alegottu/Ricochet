using UnityEngine;

public class Retry : MonoBehaviour, IMenuButton
{
    public void OnClick()
    {
        SceneController.Instance.UnloadObjects();
        SceneController.Instance.LoadLevel("Main");
    }
}
