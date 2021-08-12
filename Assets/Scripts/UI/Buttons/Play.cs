using UnityEngine;

public class Play : MonoBehaviour, IMenuButton
{
    public void OnClick()
    {
        SceneController.Instance.LoadLevel("Main");
    }
}
