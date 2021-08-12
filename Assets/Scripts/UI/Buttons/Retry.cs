using UnityEngine;

public class Retry : MonoBehaviour, IMenuButton
{
    public void OnClick()
    {
        SceneController.Instance.LoadLevel("Main");
    }
}
