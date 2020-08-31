using UnityEngine;

public class Arcade : MonoBehaviour, IHasInfo
{
    [SerializeField] private InfoText text = null;

    public void getInfo()
    {
        UIManager.Instance.displayInfo(text.info);
    }

    public void getMode()
    {
        GameManager.Instance.setMode(GameManager.GameMode.ARCADE);
    }
}
